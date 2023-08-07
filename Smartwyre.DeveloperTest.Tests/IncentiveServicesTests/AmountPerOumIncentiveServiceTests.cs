using FluentAssertions;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.IncentiveServices;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests.IncentiveServices;

public sealed class AmountPerOumIncentiveServiceTests
{
    [Theory]
    [InlineData(2, 4, true, 2*4)]
    [InlineData(3, 9, true, 3*9)]
    [InlineData(0, 9, false, 0)]
    [InlineData(3, 0, false, 0)]
    [InlineData(0, 0, false, 0)]
    public void CalculateRebateTheory(decimal inputRebateAmount, decimal inputVolume, bool expectedIsSucceed,
        decimal expectedRebateAmount)
    {
        // Arrange
        var incentiveService = new AmountPerUomIncentiveService();
        var rebate = new Rebate
        {
            Amount = inputRebateAmount,
        };
        var product = new Product();
        
        // Act
        var isSucceed = incentiveService.TryCalculateRebate(rebate, product, inputVolume, out var rebateAmount);
        
        // Assert
        isSucceed.Should().Be(expectedIsSucceed);
        rebateAmount.Should().Be(expectedRebateAmount);
    }
}