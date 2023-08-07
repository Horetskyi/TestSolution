using System;
using System.Diagnostics.Contracts;
using Smartwyre.DeveloperTest.Data;
using Smartwyre.DeveloperTest.Services;
using Smartwyre.DeveloperTest.Services.IncentiveServices;
using Smartwyre.DeveloperTest.Types;

namespace Smartwyre.DeveloperTest.Runner;

sealed class Program
{
    static void Main(string[] args)
    {
        KnownIncentiveServices knownIncentiveServices = new KnownIncentiveServices();
        IProductDataStore productDataStore = new ProductDataStore();
        IRebateDataStore rebateDataStore = new RebateDataStore();
        IRebateService rebateService = new RebateService(rebateDataStore, productDataStore, knownIncentiveServices);

        Console.WriteLine("PaymentService running...");

        if (!TryParseRebateRequest(args, out var rebateRequest))
            rebateRequest = ConsoleReadRebateRequest();

        Console.WriteLine("Calculating rebate...");
        
        var rebateResult = rebateService.Calculate(rebateRequest);
        
        Console.WriteLine($"Rebate result: {rebateResult}");
        Console.WriteLine("Press enter to exit:");
        Console.ReadLine();
    }

    private static CalculateRebateRequest ConsoleReadRebateRequest()
    {
        Console.WriteLine("Enter rebate identifier:");
        var rebateIdentifier = Console.ReadLine();

        Console.WriteLine("Enter product identifier:");
        var productIdentifier = Console.ReadLine();

        Console.WriteLine("Enter volume:");
        decimal.TryParse(Console.ReadLine() ?? "0", out var volume);

        return new CalculateRebateRequest
        {
            RebateIdentifier = rebateIdentifier,
            ProductIdentifier = productIdentifier,
            Volume = volume,
        };
    }
    
    [Pure]
    private static bool TryParseRebateRequest(string[] args, out CalculateRebateRequest rebateRequest)
    {
        if (args == null || args.Length != 3)
        {
            rebateRequest = default;
            return false;
        }
        decimal.TryParse(args[2], out var volume);
        rebateRequest = new CalculateRebateRequest
        {
            RebateIdentifier = args[0],
            ProductIdentifier = args[1],
            Volume = volume,
        };
        return true;
    }
}


