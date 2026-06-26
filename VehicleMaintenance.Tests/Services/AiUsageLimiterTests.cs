using FluentAssertions;
using VehicleMaintenance.Services.RateLimiting;
using Xunit;

namespace VehicleMaintenance.Tests.Services
{
    public class AiUsageLimiterTests
    {
        [Fact]
        public void TryConsume_AllowsUpToDailyLimit_ThenRejects()
        {
            var limiter = new AiUsageLimiter();
            const string user = "u1";

            for (var i = 1; i <= 3; i++)
                limiter.TryConsume(user, dailyLimit: 3, out _).Should().BeTrue($"call {i} is within the limit");

            limiter.TryConsume(user, dailyLimit: 3, out var status).Should().BeFalse("the 4th call exceeds the limit of 3");
            status.Used.Should().Be(3);
            status.Remaining.Should().Be(0);
        }

        [Fact]
        public void TryConsume_ZeroLimit_IsUnlimited()
        {
            var limiter = new AiUsageLimiter();

            for (var i = 0; i < 100; i++)
                limiter.TryConsume("u2", dailyLimit: 0, out _).Should().BeTrue();

            limiter.Check("u2", dailyLimit: 0).Unlimited.Should().BeTrue();
        }

        [Fact]
        public void Check_DoesNotConsume()
        {
            var limiter = new AiUsageLimiter();

            limiter.Check("u3", dailyLimit: 5).Used.Should().Be(0);
            limiter.Check("u3", dailyLimit: 5).Used.Should().Be(0);

            limiter.TryConsume("u3", dailyLimit: 5, out _);
            limiter.Check("u3", dailyLimit: 5).Used.Should().Be(1);
        }
    }
}
