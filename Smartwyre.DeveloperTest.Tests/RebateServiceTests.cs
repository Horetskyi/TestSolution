using FluentAssertions;
using NSubstitute;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.IncentiveServices;
using Smartwyre.DeveloperTest.Types;
using Xunit;

namespace Smartwyre.DeveloperTest.Tests;

public sealed class RebateServiceTests
{
    [Fact]
    public void StoreCalculationResultShouldNotBeCalledWhenFailed()
    {
        // Arrange
        var calculateRebateRequest = new CalculateRebateRequest();
        var rebateDataStore = Substitute.For<IRebateDataStore>();
        var productDataStore = Substitute.For<IProductDataStore>();
        var rebateService = CreateRebateService(rebateDataStore, productDataStore);
        rebateDataStore.GetRebate(Arg.Any<string>()).Returns(new Rebate());
        productDataStore.GetProduct(Arg.Any<string>()).Returns(new Product());
        
        // Act
        var result = rebateService.Calculate(calculateRebateRequest);

        // Assert
        result.IsSucceed.Should().BeFalse();
        result.RebateAmount.Should().Be(0);
        rebateDataStore.DidNotReceiveWithAnyArgs().StoreCalculationResult(Arg.Any<Rebate>(), Arg.Any<decimal>());
    }
    
    [Fact]
    public void StoreCalculationResultShouldBeCalledWhenSucceed()
    {
        // Arrange
        var rebateIdentifier = "Rebate_1";
        var productIdentifier = "Product_1";
        var testRebate = new Rebate
        {
            Identifier = rebateIdentifier,
            Amount = 15,
            Percentage = 40,
            Incentive = IncentiveType.FixedRateRebate
        };
        var testProduct = new Product
        {
            Id = 1,
            Identifier = productIdentifier,
            SupportedIncentives = SupportedIncentiveType.FixedRateRebate,
            Price = 2,
        };
        var calculateRebateRequest = new CalculateRebateRequest
        {
            Volume = 10,
            ProductIdentifier = productIdentifier,
            RebateIdentifier = rebateIdentifier,
        };
        var expectedRebateAmount = 40*2*10;
        
        var rebateDataStore = Substitute.For<IRebateDataStore>();
        var productDataStore = Substitute.For<IProductDataStore>();
        var rebateService = CreateRebateService(rebateDataStore, productDataStore);
        rebateDataStore.GetRebate(rebateIdentifier).Returns(testRebate);
        productDataStore.GetProduct(productIdentifier).Returns(testProduct);
        
        // Act
        var result = rebateService.Calculate(calculateRebateRequest);

        // Assert
        result.IsSucceed.Should().BeTrue();
        result.RebateAmount.Should().Be(expectedRebateAmount);
        rebateDataStore.Received().StoreCalculationResult(Arg.Is(testRebate), expectedRebateAmount);
    }
    
    [Theory]
    [InlineData(IncentiveType.AmountPerUom, 
        SupportedIncentiveType.AmountPerUom | SupportedIncentiveType.FixedCashAmount,
        true)]
    [InlineData(IncentiveType.AmountPerUom, 
        SupportedIncentiveType.AmountPerUom,
        true)]
    [InlineData(IncentiveType.FixedCashAmount, 
        SupportedIncentiveType.FixedCashAmount | SupportedIncentiveType.AmountPerUom,
        true)]
    [InlineData(IncentiveType.FixedCashAmount, 
        SupportedIncentiveType.FixedRateRebate,
        false)]
    [InlineData(IncentiveType.FixedCashAmount, 
        SupportedIncentiveType.FixedRateRebate | SupportedIncentiveType.AmountPerUom,
        false)]
    public void OnlySupportiveIncentiveTypeShouldSucceed(IncentiveType incentiveType,
        SupportedIncentiveType supportedIncentiveType, bool expectedIsSucceed)
    {
        // Arrange
        var rebateService = CreateRebateService();
        var product = new Product
        {
            SupportedIncentives = supportedIncentiveType,
        };
        decimal somePositiveDecimal = 10;
        var rebate = new Rebate
        {
            Incentive = incentiveType,
            Amount = somePositiveDecimal,
            Percentage = somePositiveDecimal,
        };
        
        // Act
        var rebateResult = rebateService.Calculate(rebate, product, somePositiveDecimal);
        
        // Assert
        rebateResult.IsSucceed.Should().Be(expectedIsSucceed);
    }

    private RebateService CreateRebateService()
    {
        return CreateRebateService(new RebateDataStore(), new ProductDataStore());
    }
    
    private RebateService CreateRebateService(IRebateDataStore rebateDataStore, IProductDataStore productDataStore)
    {
        var knownIncentiveServices = new KnownIncentiveServices();
        return new RebateService(rebateDataStore, productDataStore, knownIncentiveServices);
    }
}
