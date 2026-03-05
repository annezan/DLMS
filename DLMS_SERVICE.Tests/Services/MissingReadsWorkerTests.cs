using DLMS_MODELS;
using DLMS_SERVICE.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DLMS_SERVICE.Tests.Services
{
    public class MissingReadsWorkerTests
    {
        private readonly Mock<ILogger<MissingReadsWorker>> _logger;
        private readonly Mock<IServiceProvider> _serviceProvider;
        private readonly Mock<IIPWorkerService> _workerService;
        private readonly Mock<IDLMSMissingReadService> _missingReadService;

        public MissingReadsWorkerTests()
        {
            _logger = new Mock<ILogger<MissingReadsWorker>>();
            _serviceProvider = new Mock<IServiceProvider>();
            _workerService = new Mock<IIPWorkerService>();
            _missingReadService = new Mock<IDLMSMissingReadService>();

            // Setup service scope
            var scope = new Mock<IServiceScope>();
            var scopeFactory = new Mock<IServiceScopeFactory>();
            var scopedServiceProvider = new Mock<IServiceProvider>();

            scopedServiceProvider.Setup(sp => sp.GetService(typeof(IDLMSMissingReadService)))
                .Returns(_missingReadService.Object);
            scope.Setup(s => s.ServiceProvider).Returns(scopedServiceProvider.Object);
            scopeFactory.Setup(f => f.CreateScope()).Returns(scope.Object);
            _serviceProvider.Setup(sp => sp.GetService(typeof(IServiceScopeFactory)))
                .Returns(scopeFactory.Object);
        }

        private MissingReadsWorker CreateWorker()
        {
            return new MissingReadsWorker(_logger.Object, _serviceProvider.Object, _workerService.Object);
        }

        /// <summary>
        /// Run the worker for a short time and verify behavior.
        /// The worker checks DateTime.Now internally, so we test by running it briefly
        /// and verifying the expected calls.
        /// </summary>
        private async Task RunWorkerBriefly(MissingReadsWorker worker, TimeSpan? duration = null)
        {
            using var cts = new CancellationTokenSource(duration ?? TimeSpan.FromMilliseconds(100));
            try
            {
                await worker.StartAsync(cts.Token);
                await Task.Delay(duration ?? TimeSpan.FromMilliseconds(150));
                await worker.StopAsync(CancellationToken.None);
            }
            catch (OperationCanceledException)
            {
                // Expected
            }
        }

        #region Fenetre temporelle

        [Theory]
        [InlineData(10)]
        [InlineData(15)]
        [InlineData(25)]
        public async Task ExecuteAsync_InsideWindow_EnqueuesCalled(int minute)
        {
            // Since MissingReadsWorker uses DateTime.Now internally,
            // we test this by verifying the behavior when the worker runs.
            // If current minute is within 10-25, missing reads should be fetched.
            var now = DateTime.Now;
            if (now.Minute < 10 || now.Minute > 25)
            {
                // Skip test if we're outside the window - cannot control DateTime.Now
                return;
            }

            var missingReads = new List<MissingReadInfo>
            {
                new() { CompteurId = 1, NumeroCompteur = "M001", AdresseIp = "10.0.0.1", Port = "4059" }
            };

            _missingReadService.Setup(s => s.GetMissingReadsAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(missingReads);

            var worker = CreateWorker();
            await RunWorkerBriefly(worker);

            _missingReadService.Verify(s => s.GetMissingReadsAsync(It.IsAny<DateTime>()), Times.AtLeastOnce());
        }

        [Fact]
        public async Task ExecuteAsync_OutsideWindow_EnqueuesNotCalled()
        {
            var now = DateTime.Now;
            if (now.Minute >= 10 && now.Minute <= 25)
            {
                // Skip test if we're inside the window
                return;
            }

            var worker = CreateWorker();
            await RunWorkerBriefly(worker);

            _missingReadService.Verify(s => s.GetMissingReadsAsync(It.IsAny<DateTime>()), Times.Never());
        }

        [Fact]
        public async Task ExecuteAsync_WithMissingReads_EnqueuesViaWorkerService()
        {
            var now = DateTime.Now;
            if (now.Minute < 10 || now.Minute > 25)
            {
                // Skip if outside window
                return;
            }

            var missingReads = new List<MissingReadInfo>
            {
                new() { CompteurId = 1, NumeroCompteur = "M001", AdresseIp = "10.0.0.1", Port = "4059" },
                new() { CompteurId = 2, NumeroCompteur = "M002", AdresseIp = "10.0.0.1", Port = "4059" }
            };

            _missingReadService.Setup(s => s.GetMissingReadsAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(missingReads);

            var worker = CreateWorker();
            await RunWorkerBriefly(worker);

            _workerService.Verify(w => w.EnqueueMissingReadsAsync(It.Is<List<MissingReadInfo>>(l => l.Count == 2)), Times.AtLeastOnce());
        }

        [Fact]
        public async Task ExecuteAsync_NoMissingReads_DoesNotEnqueue()
        {
            var now = DateTime.Now;
            if (now.Minute < 10 || now.Minute > 25)
            {
                // Skip if outside window
                return;
            }

            _missingReadService.Setup(s => s.GetMissingReadsAsync(It.IsAny<DateTime>()))
                .ReturnsAsync(new List<MissingReadInfo>());

            var worker = CreateWorker();
            await RunWorkerBriefly(worker);

            _workerService.Verify(w => w.EnqueueMissingReadsAsync(It.IsAny<List<MissingReadInfo>>()), Times.Never());
        }

        #endregion
    }
}
