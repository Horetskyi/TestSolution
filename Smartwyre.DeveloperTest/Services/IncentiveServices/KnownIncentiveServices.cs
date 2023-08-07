using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Services.IncentiveServices;

public sealed class KnownIncentiveServices
{
    // List of IncentiveServices can be collected automatically via Reflection... if needed.
    private static readonly List<IIncentiveService> knownIncentiveServices = new()
    {
        new AmountPerUomIncentiveService(),
        new FixedRateRebateIncentiveService(),
        new FixedCashAmountIncentiveService(),
    };
    
    private static readonly Dictionary<IncentiveType, IIncentiveService> incentiveServicesDictionary = knownIncentiveServices
        .ToDictionary(service => service.IncentiveType);

    [Pure]
    public IIncentiveService GetIncentiveServiceByType(IncentiveType incentiveType)
    {
        return incentiveServicesDictionary.GetValueOrDefault(incentiveType);
    }
}