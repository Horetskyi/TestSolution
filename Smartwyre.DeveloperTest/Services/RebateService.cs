using System.Diagnostics.Contracts;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services.IncentiveServices;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services;

public class RebateService : IRebateService
{
    private readonly IRebateDataStore _rebateDataStore;
    private readonly IProductDataStore _productDataStore;
    private readonly KnownIncentiveServices _knownIncentiveServices;

    public RebateService(
        IRebateDataStore rebateDataStore, 
        IProductDataStore productDataStore,
        KnownIncentiveServices knownIncentiveServices)
    {
        _rebateDataStore = rebateDataStore;
        _productDataStore = productDataStore;
        _knownIncentiveServices = knownIncentiveServices;
    }

    public CalculateRebateResult Calculate(CalculateRebateRequest request)
    {
        var rebate = _rebateDataStore.GetRebate(request.RebateIdentifier);
        var product = _productDataStore.GetProduct(request.ProductIdentifier);
        
        var result = Calculate(rebate, product, request.Volume);
        
        if (result.IsSucceed)
            _rebateDataStore.StoreCalculationResult(rebate, result.RebateAmount);
        
        return result;
    }

    [Pure]
    public CalculateRebateResult Calculate(Rebate rebate, Product product, decimal volume)
    {
        if (rebate == null || product == null)
            return CalculateRebateResult.Failed;
        
        var incentiveService = _knownIncentiveServices.GetIncentiveServiceByType(rebate.Incentive);
        if (incentiveService == null || !product.SupportedIncentives.HasFlag(incentiveService.SupportedIncentiveType))
            return CalculateRebateResult.Failed;

        if (!incentiveService.TryCalculateRebate(rebate, product, volume,  out var rebateAmount))
            return CalculateRebateResult.Failed;

        return CalculateRebateResult.Succeed(rebateAmount);
    }
}
