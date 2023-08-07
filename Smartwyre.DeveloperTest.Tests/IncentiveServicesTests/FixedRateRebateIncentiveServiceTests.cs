using FluentAssertions;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.IncentiveServices;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.IncentiveServices;

public sealed class FixedRateRebateIncentiveServiceTests
{
    [Theory]
    [InlineData(1,2, 4, true, 1*2*4)]
    [InlineData(1,2, 0, false, 0)]
    [InlineData(1,0, 4, false, 0)]
    [InlineData(0,2, 4, false, 0)]
    public void CalculateRebateTheory(decimal price, decimal percentage, decimal volume, bool expectedIsSucceed,
        decimal expectedRebateAmount)
    {
        // Arrange
        var incentiveService = new FixedRateRebateIncentiveService();
        var rebate = new Rebate
        {
            Percentage = percentage,
        };
        var product = new Product
        {
            Price = price,
        };
        
        // Act
        var isSucceed = incentiveService.TryCalculateRebate(rebate, product, volume, out var rebateAmount);
        
        // Assert
        isSucceed.Should().Be(expectedIsSucceed);
        rebateAmount.Should().Be(expectedRebateAmount);
    }
}