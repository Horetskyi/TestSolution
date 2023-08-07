using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.IncentiveServices;

public interface IIncentiveService
{
    public IncentiveType IncentiveType { get; }
    public SupportedIncentiveType SupportedIncentiveType { get; }
    
    bool TryCalculateRebate(Rebate rebate, Product product, decimal volume, out decimal rebateAmount);
}