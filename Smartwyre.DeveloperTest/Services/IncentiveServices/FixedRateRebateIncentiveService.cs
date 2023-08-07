using System.Diagnostics.Contracts;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.IncentiveServices;

public sealed class FixedRateRebateIncentiveService : IIncentiveService
{
    public IncentiveType IncentiveType => IncentiveType.FixedRateRebate;
    public SupportedIncentiveType SupportedIncentiveType => SupportedIncentiveType.FixedRateRebate;
    
    [Pure]
    public bool TryCalculateRebate(Rebate rebate, Product product, decimal volume, out decimal rebateAmount)
    {
        if (rebate.Percentage == 0 || product.Price == 0 || volume == 0)
        {
            rebateAmount = default;
            return false;
        }
        rebateAmount = product.Price * rebate.Percentage * volume;
        return true;
    }
}