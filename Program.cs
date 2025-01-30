using System;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        const string apiUrl = "https://www.cbr.ru/scripts/XML_daily.asp";
        var fetcher = new CurrencyApiFetcher(apiUrl);

        Console.Write("Enter currency code (e.g., USD): ");
        string currencyCode = Console.ReadLine()?.Trim().ToUpper();

        try
        {
            decimal? rate = await fetcher.FetchCurrencyRateAsync(currencyCode);

            if (rate.HasValue)
            {
                Console.WriteLine($"{currencyCode}: {rate.Value} RUB");
            }
            else
            {
                Console.WriteLine("Currency not found.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}