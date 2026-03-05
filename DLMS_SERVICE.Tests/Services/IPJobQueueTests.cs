using DLMS_SERVICE.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DLMS_SERVICE.Tests.Services
{
    public class IPJobQueueTests
    {
        private readonly IPJobQueue _queue;

        public IPJobQueueTests()
        {
            var logger = new Mock<ILogger<IPJobQueue>>();
            _queue = new IPJobQueue(logger.Object);
        }

        #region Operations de base

        [Fact]
        public async Task Enqueue_Dequeue_ReturnsJob()
        {
            var job = new DLMSJob
            {
                IP = "192.168.1.1",
                Port = 4059,
                Type = JobType.Hourly
            };

            await _queue.EnqueueAsync(job);
            var result = await _queue.DequeueAsync("192.168.1.1", 4059, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(job.Id, result.Id);
            Assert.Equal(JobType.Hourly, result.Type);
        }

        [Fact]
        public async Task Dequeue_EmptyQueue_ReturnsNull()
        {
            var result = await _queue.DequeueAsync("10.0.0.1", 4059, CancellationToken.None);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetQueueCount_ReturnsCorrectCount()
        {
            for (int i = 0; i < 3; i++)
            {
                await _queue.EnqueueAsync(new DLMSJob
                {
                    IP = "192.168.1.1",
                    Port = 4059,
                    Type = JobType.Hourly
                });
            }

            var count = await _queue.GetQueueCountAsync("192.168.1.1", 4059);
            Assert.Equal(3, count);
        }

        #endregion

        #region Priorite

        [Fact]
        public async Task Dequeue_ReturnsHighestPriority()
        {
            // Command(1) before Hourly(2) before Missing(3)
            await _queue.EnqueueAsync(new DLMSJob { IP = "10.0.0.1", Port = 4059, Type = JobType.Missing });
            await _queue.EnqueueAsync(new DLMSJob { IP = "10.0.0.1", Port = 4059, Type = JobType.Hourly });
            await _queue.EnqueueAsync(new DLMSJob { IP = "10.0.0.1", Port = 4059, Type = JobType.Command });

            var first = await _queue.DequeueAsync("10.0.0.1", 4059, CancellationToken.None);
            var second = await _queue.DequeueAsync("10.0.0.1", 4059, CancellationToken.None);
            var third = await _queue.DequeueAsync("10.0.0.1", 4059, CancellationToken.None);

            Assert.Equal(JobType.Command, first!.Type);
            Assert.Equal(JobType.Hourly, second!.Type);
            Assert.Equal(JobType.Missing, third!.Type);
        }

        [Fact]
        public async Task MixedPriorities_CommandFirst()
        {
            await _queue.EnqueueAsync(new DLMSJob { IP = "10.0.0.1", Port = 4059, Type = JobType.Missing });
            await _queue.EnqueueAsync(new DLMSJob { IP = "10.0.0.1", Port = 4059, Type = JobType.Hourly });
            await _queue.EnqueueAsync(new DLMSJob { IP = "10.0.0.1", Port = 4059, Type = JobType.Command });

            var first = await _queue.DequeueAsync("10.0.0.1", 4059, CancellationToken.None);
            Assert.Equal(JobType.Command, first!.Type);
        }

        #endregion

        #region Isolation par IP

        [Fact]
        public async Task DifferentIPs_HaveSeparateQueues()
        {
            await _queue.EnqueueAsync(new DLMSJob { IP = "10.0.0.1", Port = 4059, Type = JobType.Hourly });
            await _queue.EnqueueAsync(new DLMSJob { IP = "10.0.0.2", Port = 4059, Type = JobType.Command });

            var fromIP1 = await _queue.DequeueAsync("10.0.0.1", 4059, CancellationToken.None);
            var fromIP2 = await _queue.DequeueAsync("10.0.0.2", 4059, CancellationToken.None);

            Assert.Equal(JobType.Hourly, fromIP1!.Type);
            Assert.Equal(JobType.Command, fromIP2!.Type);

            // Each queue should be empty now
            Assert.Equal(0, await _queue.GetQueueCountAsync("10.0.0.1", 4059));
            Assert.Equal(0, await _queue.GetQueueCountAsync("10.0.0.2", 4059));
        }

        [Fact]
        public async Task SameIP_DifferentPort_SeparateQueues()
        {
            await _queue.EnqueueAsync(new DLMSJob { IP = "10.0.0.1", Port = 4059, Type = JobType.Hourly });
            await _queue.EnqueueAsync(new DLMSJob { IP = "10.0.0.1", Port = 4060, Type = JobType.Command });

            Assert.Equal(1, await _queue.GetQueueCountAsync("10.0.0.1", 4059));
            Assert.Equal(1, await _queue.GetQueueCountAsync("10.0.0.1", 4060));
        }

        #endregion

        #region CycleStartTime

        [Fact]
        public async Task Job_PreservesCycleStartTime()
        {
            var cycleStart = new DateTime(2026, 3, 5, 10, 0, 0);
            var job = new DLMSJob
            {
                IP = "10.0.0.1",
                Port = 4059,
                Type = JobType.Hourly,
                CycleStartTime = cycleStart
            };

            await _queue.EnqueueAsync(job);
            var dequeued = await _queue.DequeueAsync("10.0.0.1", 4059, CancellationToken.None);

            Assert.NotNull(dequeued);
            Assert.Equal(cycleStart, dequeued.CycleStartTime);
        }

        #endregion
    }
}
