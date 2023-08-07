using FluentAssertions;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.IncentiveServices;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.IncentiveServices;

public sealed class FixedCashAmountIncentiveServiceTests
{
    [Theory]
    [InlineData(2, true, 2)]
    [InlineData(3, true, 3)]
    [InlineData(0, false, 0)]
    public void CalculateRebateTheory(decimal inputRebateAmount, bool expectedIsSucceed,
        decimal expectedRebateAmount)
    {
        // Arrange
        var incentiveService = new FixedCashAmountIncentiveService();
        var rebate = new Rebate
        {
            Amount = inputRebateAmount,
        };
        var product = new Product();
        
        // Act
        var isSucceed = incentiveService.TryCalculateRebate(rebate, product, 0, out var rebateAmount);
        
        // Assert
        isSucceed.Should().Be(expectedIsSucceed);
        rebateAmount.Should().Be(expectedRebateAmount);
    }
}