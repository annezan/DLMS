using DLMS_SERVICE.Services;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DLMS_SERVICE.Tests.Services
{
    public class MeterHealthTrackerTests
    {
        private readonly MeterHealthTracker _tracker;

        public MeterHealthTrackerTests()
        {
            var logger = new Mock<ILogger<MeterHealthTracker>>();
            _tracker = new MeterHealthTracker(logger.Object);
        }

        #region Categorisation des compteurs

        [Fact]
        public void Unknown_WhenNoData()
        {
            var category = _tracker.GetCategory("METER_NEVER_SEEN");
            Assert.Equal(MeterPerformanceCategory.Unknown, category);
        }

        [Fact]
        public void Fast_WhenLowResponseTimeAndHighSuccess()
        {
            // <30s avg + >80% success
            for (int i = 0; i < 10; i++)
                _tracker.RecordResult("FAST_METER", TimeSpan.FromSeconds(20), true);

            Assert.Equal(MeterPerformanceCategory.Fast, _tracker.GetCategory("FAST_METER"));
        }

        [Fact]
        public void Medium_WhenModerateStats()
        {
            // 30-90s average, >80% success -> Medium (not Fast because avg >= 30s)
            for (int i = 0; i < 10; i++)
                _tracker.RecordResult("MEDIUM_METER", TimeSpan.FromSeconds(50), true);

            Assert.Equal(MeterPerformanceCategory.Medium, _tracker.GetCategory("MEDIUM_METER"));
        }

        [Fact]
        public void Slow_WhenHighResponseTime()
        {
            // >90s avg -> Slow
            for (int i = 0; i < 5; i++)
                _tracker.RecordResult("SLOW_METER", TimeSpan.FromSeconds(100), true);

            Assert.Equal(MeterPerformanceCategory.Slow, _tracker.GetCategory("SLOW_METER"));
        }

        [Fact]
        public void Slow_WhenLowSuccessRate()
        {
            // <50% success -> Slow (alternate to avoid 3+ consecutive failures triggering Failing)
            // Pattern: F S F F S F F S F F -> 3 success / 10 = 30%, max 2 consecutive failures
            bool[] pattern = { false, true, false, false, true, false, false, true, false, false };
            for (int i = 0; i < pattern.Length; i++)
                _tracker.RecordResult("LOW_SUCCESS", TimeSpan.FromSeconds(20), pattern[i]);

            Assert.Equal(MeterPerformanceCategory.Slow, _tracker.GetCategory("LOW_SUCCESS"));
        }

        [Fact]
        public void Failing_WhenConsecutiveFailures()
        {
            // 3+ consecutive failures -> Failing
            _tracker.RecordResult("FAILING_METER", TimeSpan.FromSeconds(10), true);
            _tracker.RecordResult("FAILING_METER", TimeSpan.FromSeconds(10), false);
            _tracker.RecordResult("FAILING_METER", TimeSpan.FromSeconds(10), false);
            _tracker.RecordResult("FAILING_METER", TimeSpan.FromSeconds(10), false);

            Assert.Equal(MeterPerformanceCategory.Failing, _tracker.GetCategory("FAILING_METER"));
        }

        [Fact]
        public void Failing_TakesPriorityOverSlow()
        {
            // Even with slow stats, 3+ consecutive failures -> Failing
            for (int i = 0; i < 5; i++)
                _tracker.RecordResult("FAIL_SLOW", TimeSpan.FromSeconds(100), true);

            _tracker.RecordResult("FAIL_SLOW", TimeSpan.FromSeconds(100), false);
            _tracker.RecordResult("FAIL_SLOW", TimeSpan.FromSeconds(100), false);
            _tracker.RecordResult("FAIL_SLOW", TimeSpan.FromSeconds(100), false);

            Assert.Equal(MeterPerformanceCategory.Failing, _tracker.GetCategory("FAIL_SLOW"));
        }

        #endregion

        #region Timeout adaptatif par categorie

        [Fact]
        public void Fast_Returns60sTimeout()
        {
            for (int i = 0; i < 10; i++)
                _tracker.RecordResult("FAST", TimeSpan.FromSeconds(20), true);

            var info = _tracker.GetHealthInfo("FAST");
            Assert.Equal(TimeSpan.FromSeconds(60), info.AdaptiveTimeout);
        }

        [Fact]
        public void Medium_Returns90sTimeout()
        {
            for (int i = 0; i < 10; i++)
                _tracker.RecordResult("MED", TimeSpan.FromSeconds(50), true);

            var info = _tracker.GetHealthInfo("MED");
            Assert.Equal(TimeSpan.FromSeconds(90), info.AdaptiveTimeout);
        }

        [Fact]
        public void Slow_Returns45sTimeout()
        {
            for (int i = 0; i < 5; i++)
                _tracker.RecordResult("SLW", TimeSpan.FromSeconds(100), true);

            var info = _tracker.GetHealthInfo("SLW");
            Assert.Equal(TimeSpan.FromSeconds(45), info.AdaptiveTimeout);
        }

        [Fact]
        public void Failing_Returns20sTimeout()
        {
            _tracker.RecordResult("FAIL", TimeSpan.FromSeconds(10), false);
            _tracker.RecordResult("FAIL", TimeSpan.FromSeconds(10), false);
            _tracker.RecordResult("FAIL", TimeSpan.FromSeconds(10), false);

            var info = _tracker.GetHealthInfo("FAIL");
            Assert.Equal(TimeSpan.FromSeconds(20), info.AdaptiveTimeout);
        }

        [Fact]
        public void Unknown_Returns120sTimeout()
        {
            var info = _tracker.GetHealthInfo("NEVER_SEEN");
            Assert.Equal(TimeSpan.FromSeconds(120), info.AdaptiveTimeout);
        }

        #endregion

        #region WaitTime et RetryCount par categorie

        [Fact]
        public void Fast_Returns3000msWaitTime_And2Retries()
        {
            for (int i = 0; i < 10; i++)
                _tracker.RecordResult("FAST_W", TimeSpan.FromSeconds(20), true);

            var info = _tracker.GetHealthInfo("FAST_W");
            Assert.Equal(3000, info.WaitTime);
            Assert.Equal(2, info.RetryCount);
        }

        [Fact]
        public void Slow_Returns3000msWaitTime_And1Retry()
        {
            for (int i = 0; i < 5; i++)
                _tracker.RecordResult("SLW_W", TimeSpan.FromSeconds(100), true);

            var info = _tracker.GetHealthInfo("SLW_W");
            Assert.Equal(3000, info.WaitTime);
            Assert.Equal(1, info.RetryCount);
        }

        [Fact]
        public void Failing_Returns2000msWaitTime_And1Retry()
        {
            _tracker.RecordResult("FAIL_W", TimeSpan.FromSeconds(10), false);
            _tracker.RecordResult("FAIL_W", TimeSpan.FromSeconds(10), false);
            _tracker.RecordResult("FAIL_W", TimeSpan.FromSeconds(10), false);

            var info = _tracker.GetHealthInfo("FAIL_W");
            Assert.Equal(2000, info.WaitTime);
            Assert.Equal(1, info.RetryCount);
        }

        #endregion

        #region Scoring de priorite

        [Fact]
        public void Unknown_ReturnsPriority0()
        {
            Assert.Equal(0, _tracker.GetPriorityScore("UNKNOWN_P"));
        }

        [Fact]
        public void Fast_ReturnsPriority1()
        {
            for (int i = 0; i < 10; i++)
                _tracker.RecordResult("FAST_P", TimeSpan.FromSeconds(20), true);

            Assert.Equal(1, _tracker.GetPriorityScore("FAST_P"));
        }

        [Fact]
        public void Medium_ReturnsPriority2()
        {
            for (int i = 0; i < 10; i++)
                _tracker.RecordResult("MED_P", TimeSpan.FromSeconds(50), true);

            Assert.Equal(2, _tracker.GetPriorityScore("MED_P"));
        }

        [Fact]
        public void Slow_ReturnsPriority3()
        {
            for (int i = 0; i < 5; i++)
                _tracker.RecordResult("SLW_P", TimeSpan.FromSeconds(100), true);

            Assert.Equal(3, _tracker.GetPriorityScore("SLW_P"));
        }

        [Fact]
        public void Failing_ReturnsPriority4()
        {
            _tracker.RecordResult("FAIL_P", TimeSpan.FromSeconds(10), false);
            _tracker.RecordResult("FAIL_P", TimeSpan.FromSeconds(10), false);
            _tracker.RecordResult("FAIL_P", TimeSpan.FromSeconds(10), false);

            Assert.Equal(4, _tracker.GetPriorityScore("FAIL_P"));
        }

        #endregion

        #region Circular buffer

        [Fact]
        public void ResponseTimes_LimitedTo20()
        {
            // Insert 25 entries with 100s, then the buffer should only hold the last 20
            for (int i = 0; i < 25; i++)
                _tracker.RecordResult("BUF_RT", TimeSpan.FromSeconds(100), true);

            // Now insert 20 entries with 10s to replace the buffer entirely
            // After 25 x 100s: buffer has 20 x 100s
            // The category should be Slow (avg 100s > 90s)
            Assert.Equal(MeterPerformanceCategory.Slow, _tracker.GetCategory("BUF_RT"));

            // Now add 20 fast reads to replace the entire buffer
            for (int i = 0; i < 20; i++)
                _tracker.RecordResult("BUF_RT", TimeSpan.FromSeconds(10), true);

            // Now the buffer should only have 20 x 10s -> avg 10s, all success -> Fast
            Assert.Equal(MeterPerformanceCategory.Fast, _tracker.GetCategory("BUF_RT"));
        }

        [Fact]
        public void Results_LimitedTo50()
        {
            // Insert 60 failures: buffer keeps last 50
            for (int i = 0; i < 60; i++)
                _tracker.RecordResult("BUF_R", TimeSpan.FromSeconds(20), false);

            // Success rate = 0/50 = 0% -> Slow (also Failing due to consecutive failures)
            var cat = _tracker.GetCategory("BUF_R");
            Assert.Equal(MeterPerformanceCategory.Failing, cat);

            // Now insert 50 successes to fill the buffer with successes
            for (int i = 0; i < 50; i++)
                _tracker.RecordResult("BUF_R", TimeSpan.FromSeconds(20), true);

            // Buffer should now be 50 successes -> 100% success rate
            // Avg 20s, 100% success -> Fast
            Assert.Equal(MeterPerformanceCategory.Fast, _tracker.GetCategory("BUF_R"));
        }

        [Fact]
        public void ConsecutiveFailures_ResetOnSuccess()
        {
            _tracker.RecordResult("RESET", TimeSpan.FromSeconds(10), false);
            _tracker.RecordResult("RESET", TimeSpan.FromSeconds(10), false);
            _tracker.RecordResult("RESET", TimeSpan.FromSeconds(10), false);

            Assert.Equal(MeterPerformanceCategory.Failing, _tracker.GetCategory("RESET"));

            // One success should reset consecutive failures
            _tracker.RecordResult("RESET", TimeSpan.FromSeconds(10), true);

            // No longer Failing (consecutive failures = 0)
            Assert.NotEqual(MeterPerformanceCategory.Failing, _tracker.GetCategory("RESET"));
        }

        #endregion

        #region Edge cases

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void NullOrEmptySerial_ReturnsUnknown(string? serial)
        {
            Assert.Equal(MeterPerformanceCategory.Unknown, _tracker.GetCategory(serial!));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void RecordResult_WithNullOrEmptySerial_DoesNotThrow(string? serial)
        {
            var ex = Record.Exception(() =>
                _tracker.RecordResult(serial!, TimeSpan.FromSeconds(10), true));

            Assert.Null(ex);
        }

        #endregion
    }
}
