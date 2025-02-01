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

        while (true)
        {
            Console.Write("Enter currency code (e.g., USD) or type 'exit' to quit: ");
            string currencyCode = Console.ReadLine()?.Trim().ToUpper();

            if (currencyCode == "EXIT")
            {
                Console.WriteLine("Exiting...");
                break;
            }

            if (string.IsNullOrWhiteSpace(currencyCode))
            {
                Console.WriteLine("Error: Currency code cannot be empty. Try again.\n");
                continue;
            }

            try
            {
                decimal? rate = await fetcher.FetchCurrencyRateAsync(currencyCode);

                if (rate.HasValue)
                {
                    Console.WriteLine($"{currencyCode}: {rate.Value} RUB\n");
                }
                else
                {
                    Console.WriteLine($"Currency '{currencyCode}' not found.\n");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}\n");
            }
        }
    }
}