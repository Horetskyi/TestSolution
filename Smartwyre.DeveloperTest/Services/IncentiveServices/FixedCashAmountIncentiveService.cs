using System.Diagnostics.Contracts;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.IncentiveServices;

public sealed class FixedCashAmountIncentiveService : IIncentiveService
{
    public IncentiveType IncentiveType => IncentiveType.FixedCashAmount;
    public SupportedIncentiveType SupportedIncentiveType => SupportedIncentiveType.FixedCashAmount;
    
    [Pure]
    public bool TryCalculateRebate(Rebate rebate, Product product, decimal volume, out decimal rebateAmount)
    {
        rebateAmount = rebate.Amount;
        return rebateAmount != 0;
    }
}