using System.Diagnostics.Contracts;

namespace Smartwyre.DeveloperTest.Types;

public readonly struct CalculateRebateResult
{
    public static readonly CalculateRebateResult Failed = new(false, default);
    
    public bool IsSucceed { get; }
    public decimal RebateAmount { get; }

    private CalculateRebateResult(bool isSucceed, decimal rebateAmount)
    {
        IsSucceed = isSucceed;
        RebateAmount = rebateAmount;
    }
    
    [Pure]
    public static CalculateRebateResult Succeed(decimal rebateAmount) => new(true, rebateAmount);

    public override string ToString()
    {
        return $"{nameof(IsSucceed)}: {IsSucceed}, {nameof(RebateAmount)}: {RebateAmount}";
    }
}
