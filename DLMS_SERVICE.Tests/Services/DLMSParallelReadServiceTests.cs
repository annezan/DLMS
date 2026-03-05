using DLMS_COMMUNICATION.Reader;
using DLMS_MODELS;
using DLMS_MODELS.CompteurDomain.Entities;
using DLMS_MODELS.CompteurEquipementDomain.Entities;
using DLMS_SERVICE.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DLMS_SERVICE.Tests.Services
{
    public class DLMSParallelReadServiceTests
    {
        private readonly Mock<IDLMSGuruxSessionFactory> _sessionFactory;
        private readonly Mock<IDLMSKeyService> _keyService;
        private readonly Mock<IDataProcessingServiceFactory> _dataProcessingServiceFactory;
        private readonly Mock<IDLMSHardwareService> _hardwareService;
        private readonly Mock<ILogger<DLMSParallelReadService>> _logger;
        private readonly Mock<IServiceProvider> _serviceProvider;
        private readonly Mock<IDLMSMetricsService> _metricsService;
        private readonly Mock<IMeterHealthTracker> _healthTracker;
        private readonly DLMSParallelReadService _service;

        public DLMSParallelReadServiceTests()
        {
            _sessionFactory = new Mock<IDLMSGuruxSessionFactory>();
            _keyService = new Mock<IDLMSKeyService>();
            _dataProcessingServiceFactory = new Mock<IDataProcessingServiceFactory>();
            _hardwareService = new Mock<IDLMSHardwareService>();
            _logger = new Mock<ILogger<DLMSParallelReadService>>();
            _serviceProvider = new Mock<IServiceProvider>();
            _metricsService = new Mock<IDLMSMetricsService>();
            _healthTracker = new Mock<IMeterHealthTracker>();

            _service = new DLMSParallelReadService(
                _sessionFactory.Object,
                _keyService.Object,
                _dataProcessingServiceFactory.Object,
                _hardwareService.Object,
                _logger.Object,
                _serviceProvider.Object,
                _metricsService.Object,
                _healthTracker.Object);
        }

        private static List<CompteurEquipement> CreateMeters(params string[] serials)
        {
            return serials.Select(s => new CompteurEquipement
            {
                CompteurId = Math.Abs(s.GetHashCode()),
                Compteur = new Compteur { NumeroCompteur = s }
            }).ToList();
        }

        private Mock<IDLMSCommunicationSession> CreateMockSession(bool transportSuccess = true)
        {
            var session = new Mock<IDLMSCommunicationSession>();
            session.Setup(s => s.OpenTransportAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(transportSuccess);
            session.Setup(s => s.Parameters).Returns(new DLMSConnectionParameters
            {
                AddressIp = "10.0.0.1",
                Port = "4059",
                InterfaceType = "HDLC"
            });
            return session;
        }

        private void SetupSessionFactory(Mock<IDLMSCommunicationSession> session)
        {
            _sessionFactory.Setup(f => f.CreateSession(It.IsAny<DLMSConnectionParameters>()))
                .Returns(session.Object);
        }

        private void SetupDefaultHealthTracker()
        {
            _healthTracker.Setup(h => h.GetPriorityScore(It.IsAny<string>())).Returns(0);
            _healthTracker.Setup(h => h.GetCategory(It.IsAny<string>())).Returns(MeterPerformanceCategory.Unknown);
            _healthTracker.Setup(h => h.GetHealthInfo(It.IsAny<string>())).Returns(new MeterHealthInfo
            {
                AdaptiveTimeout = TimeSpan.FromMinutes(3),
                WaitTime = 3000,
                RetryCount = 2,
                PriorityScore = 0,
                Category = MeterPerformanceCategory.Unknown
            });
        }

        private void SetupValidKeys()
        {
            _keyService.Setup(k => k.GetKeysAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new DLMSKeys
                {
                    AuthenticationKey = "AABBCCDD",
                    UnicastKey = "11223344",
                    Password = "pass"
                });
        }

        #region Concentrateur inaccessible

        [Fact]
        public async Task ProcessUmadGroupAsync_TransportFails_ReturnsEarly()
        {
            var session = CreateMockSession(transportSuccess: false);
            SetupSessionFactory(session);
            var meters = CreateMeters("METER_A", "METER_B");

            await _service.ProcessUmadGroupAsync("10.0.0.1", 4059, meters, CancellationToken.None);

            _keyService.Verify(k => k.GetKeysAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
            _healthTracker.Verify(h => h.RecordResult(It.IsAny<string>(), It.IsAny<TimeSpan>(), It.IsAny<bool>()), Times.Never());
        }

        [Fact]
        public async Task ProcessUmadGroupAsync_TransportFails_SessionFactoryCalledOnce()
        {
            var session = CreateMockSession(transportSuccess: false);
            SetupSessionFactory(session);
            var meters = CreateMeters("M1", "M2");

            await _service.ProcessUmadGroupAsync("10.0.0.1", 4059, meters, CancellationToken.None);

            _sessionFactory.Verify(f => f.CreateSession(It.IsAny<DLMSConnectionParameters>()), Times.Once());
        }

        [Fact]
        public async Task ProcessUmadGroupAsync_TransportFails_NoHealthTrackerOrdering()
        {
            var session = CreateMockSession(transportSuccess: false);
            SetupSessionFactory(session);
            _healthTracker.Setup(h => h.GetPriorityScore(It.IsAny<string>())).Returns(0);
            var meters = CreateMeters("M1", "M2", "M3");

            await _service.ProcessUmadGroupAsync("10.0.0.1", 4059, meters, CancellationToken.None);

            _healthTracker.Verify(h => h.GetPriorityScore(It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public async Task ProcessUmadGroupAsync_EmptyMeters_NoException()
        {
            var session = CreateMockSession(transportSuccess: false);
            SetupSessionFactory(session);

            var ex = await Record.ExceptionAsync(() =>
                _service.ProcessUmadGroupAsync("10.0.0.1", 4059, new List<CompteurEquipement>(), CancellationToken.None));

            Assert.Null(ex);
        }

        [Fact]
        public async Task ProcessUmadMissingReadsGroupAsync_TransportFails_ReturnsEarly()
        {
            var session = CreateMockSession(transportSuccess: false);
            SetupSessionFactory(session);

            var missingReads = new List<MissingReadInfo>
            {
                new() { CompteurId = 1, NumeroCompteur = "M001", AdresseIp = "10.0.0.1", Port = "4059" }
            };

            await _service.ProcessUmadMissingReadsGroupAsync("10.0.0.1", 4059, missingReads, CancellationToken.None);

            _keyService.Verify(k => k.GetKeysAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public async Task ProcessUmadCommandGroupAsync_TransportFails_ReturnsEarly()
        {
            var session = CreateMockSession(transportSuccess: false);
            SetupSessionFactory(session);

            var commands = new List<ActiveCommandInfo>
            {
                new() { CompteurId = 1, NumeroCompteur = "M001", AdresseIp = "10.0.0.1", Port = "4059", CommandType = "read" }
            };

            await _service.ProcessUmadCommandGroupAsync("10.0.0.1", 4059, commands, CancellationToken.None);

            _keyService.Verify(k => k.GetKeysAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never());
        }

        #endregion

        #region Session creation parameters

        [Fact]
        public async Task ProcessUmadGroupAsync_PassesCorrectIPAndPort()
        {
            var session = CreateMockSession(transportSuccess: false);
            SetupSessionFactory(session);
            var meters = CreateMeters("M1");

            await _service.ProcessUmadGroupAsync("192.168.1.100", 5050, meters, CancellationToken.None);

            _sessionFactory.Verify(f => f.CreateSession(It.Is<DLMSConnectionParameters>(p =>
                p.AddressIp == "192.168.1.100" && p.Port == "5050")), Times.Once());
        }

        [Fact]
        public async Task ProcessUmadMissingReadsGroupAsync_PassesCorrectIPAndPort()
        {
            var session = CreateMockSession(transportSuccess: false);
            SetupSessionFactory(session);
            var missingReads = new List<MissingReadInfo>
            {
                new() { CompteurId = 1, NumeroCompteur = "M001", AdresseIp = "10.0.0.1", Port = "4059" }
            };

            await _service.ProcessUmadMissingReadsGroupAsync("192.168.1.50", 4060, missingReads, CancellationToken.None);

            _sessionFactory.Verify(f => f.CreateSession(It.Is<DLMSConnectionParameters>(p =>
                p.AddressIp == "192.168.1.50" && p.Port == "4060")), Times.Once());
        }

        #endregion

        #region Tri intelligent des compteurs

        [Fact]
        public async Task ProcessUmadGroupAsync_MetersOrderedByPriorityScore()
        {
            var session = CreateMockSession(transportSuccess: true);
            SetupSessionFactory(session);
            SetupDefaultHealthTracker();
            SetupValidKeys();

            // Set up priority scores: METER_C=0 (best), METER_A=5, METER_B=10 (worst)
            _healthTracker.Setup(h => h.GetPriorityScore("METER_A")).Returns(5);
            _healthTracker.Setup(h => h.GetPriorityScore("METER_B")).Returns(10);
            _healthTracker.Setup(h => h.GetPriorityScore("METER_C")).Returns(0);

            // InitializeMeterClient will throw to stop processing early (we only care about ordering)
            var serialsProcessed = new List<string>();
            session.Setup(s => s.InitializeMeterClient(It.IsAny<DLMSConnectionParameters>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Callback<DLMSConnectionParameters, int?, int?>((p, _, _) =>
                {
                    serialsProcessed.Add(p.SerialNumber);
                    throw new Exception("Stop processing");
                });

            var meters = CreateMeters("METER_A", "METER_B", "METER_C");

            await _service.ProcessUmadGroupAsync("10.0.0.1", 4059, meters, CancellationToken.None);

            // GetPriorityScore should have been called for ordering
            _healthTracker.Verify(h => h.GetPriorityScore(It.IsAny<string>()), Times.AtLeast(3));

            // First meter processed (canary) should be METER_C (lowest priority score = highest priority)
            Assert.NotEmpty(serialsProcessed);
            Assert.Equal("METER_C", serialsProcessed[0]);
        }

        #endregion

        #region Budget temps

        [Fact]
        public async Task ProcessUmadGroupAsync_SkipsMetersWhenBudgetExpired()
        {
            var session = CreateMockSession(transportSuccess: true);
            SetupSessionFactory(session);
            SetupDefaultHealthTracker();
            SetupValidKeys();

            // Canary: InitializeMeterClient throws to simulate failure (canary consumed, then loop starts)
            session.Setup(s => s.InitializeMeterClient(It.IsAny<DLMSConnectionParameters>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Throws(new Exception("Canary failed"));

            var meters = CreateMeters("M1", "M2", "M3");

            // cycleStartTime = now - 55 minutes → budget already expired (deadline = cycleStartTime + 55min = now)
            var expiredCycleStart = DateTime.Now.AddMinutes(-55);
            await _service.ProcessUmadGroupAsync("10.0.0.1", 4059, meters, CancellationToken.None, cycleStartTime: expiredCycleStart);

            // After canary (M1), remaining meters (M2, M3) should be skipped due to budget
            // InitializeMeterClient called only once for canary
            session.Verify(s => s.InitializeMeterClient(It.IsAny<DLMSConnectionParameters>(), It.IsAny<int?>(), It.IsAny<int?>()), Times.Once());
        }

        #endregion

        #region Test canari

        [Fact]
        public async Task ProcessUmadGroupAsync_CanaryFailure_DegradedMode()
        {
            var session = CreateMockSession(transportSuccess: true);
            SetupSessionFactory(session);
            SetupDefaultHealthTracker();
            SetupValidKeys();

            // Track all InitializeMeterClient calls with their parameters
            var initCalls = new List<(DLMSConnectionParameters Params, int? WaitTime, int? RetryCount)>();
            session.Setup(s => s.InitializeMeterClient(It.IsAny<DLMSConnectionParameters>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Callback<DLMSConnectionParameters, int?, int?>((p, w, r) =>
                {
                    initCalls.Add((p, w, r));
                    throw new Exception("Connection failed");
                });

            var meters = CreateMeters("CANARY", "M2", "M3");

            await _service.ProcessUmadGroupAsync("10.0.0.1", 4059, meters, CancellationToken.None);

            // Canary should be first call
            Assert.NotEmpty(initCalls);
            Assert.Equal("CANARY", initCalls[0].Params.SerialNumber);

            // After canary failure, remaining meters should still be attempted (degraded mode with shorter timeouts)
            // All meters attempted (canary + 2 remaining)
            Assert.Equal(3, initCalls.Count);
        }

        [Fact]
        public async Task ProcessUmadGroupAsync_SingleMeter_NoCanaryTest()
        {
            var session = CreateMockSession(transportSuccess: true);
            SetupSessionFactory(session);
            SetupDefaultHealthTracker();

            // With only 1 meter, canary test is skipped (condition: orderedMeters.Count > 1)
            _keyService.Setup(k => k.GetKeysAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((DLMSKeys)null);

            var meters = CreateMeters("SOLO");

            await _service.ProcessUmadGroupAsync("10.0.0.1", 4059, meters, CancellationToken.None);

            // GetKeysAsync called for the single meter (goes through normal loop, not canary)
            _keyService.Verify(k => k.GetKeysAsync(It.IsAny<string>(), "SOLO", It.IsAny<string>()), Times.Once());
        }

        #endregion

        #region Health tracker alimentation

        [Fact]
        public async Task ReadMeter_ConnectionFailure_RecordsToHealthTracker()
        {
            var session = CreateMockSession(transportSuccess: true);
            SetupSessionFactory(session);
            SetupDefaultHealthTracker();

            // Keys are valid
            _keyService.Setup(k => k.GetKeysAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new DLMSKeys
                {
                    AuthenticationKey = "AABBCCDD",
                    UnicastKey = "11223344",
                    Password = "pass"
                });

            // InitializeMeterClient throws → connection failure → RecordResult(false)
            session.Setup(s => s.InitializeMeterClient(It.IsAny<DLMSConnectionParameters>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Throws(new Exception("DLMS association failed"));

            var meters = CreateMeters("FAIL_METER1", "FAIL_METER2");

            await _service.ProcessUmadGroupAsync("10.0.0.1", 4059, meters, CancellationToken.None);

            // Health tracker should record failure for meters where InitializeMeterClient threw
            _healthTracker.Verify(
                h => h.RecordResult(It.IsAny<string>(), It.IsAny<TimeSpan>(), false),
                Times.AtLeastOnce());
        }

        [Fact]
        public async Task ReadMeter_InvalidKeys_NoInitializeMeterClient()
        {
            var session = CreateMockSession(transportSuccess: true);
            SetupSessionFactory(session);
            SetupDefaultHealthTracker();

            // Keys are null → should skip InitializeMeterClient entirely
            _keyService.Setup(k => k.GetKeysAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((DLMSKeys)null);

            var meters = CreateMeters("NO_KEYS_METER1", "NO_KEYS_METER2");

            await _service.ProcessUmadGroupAsync("10.0.0.1", 4059, meters, CancellationToken.None);

            // InitializeMeterClient never called because keys are invalid
            session.Verify(s => s.InitializeMeterClient(It.IsAny<DLMSConnectionParameters>(), It.IsAny<int?>(), It.IsAny<int?>()), Times.Never());
        }

        #endregion
    }
}
