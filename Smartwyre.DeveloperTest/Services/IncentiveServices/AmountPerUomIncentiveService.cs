using System.Diagnostics.Contracts;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.IncentiveServices;

public sealed class AmountPerUomIncentiveService : IIncentiveService
{
    public IncentiveType IncentiveType => IncentiveType.AmountPerUom;
    public SupportedIncentiveType SupportedIncentiveType => SupportedIncentiveType.AmountPerUom;
    
    [Pure]
    public bool TryCalculateRebate(Rebate rebate, Product product, decimal volume, out decimal rebateAmount)
    {
        if (rebate.Amount == 0 || volume == 0)
        {
            rebateAmount = default;
            return false;
        }
        
        rebateAmount = rebate.Amount * volume;
        return true;
    }
}