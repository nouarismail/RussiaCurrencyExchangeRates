using System;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;

public class CurrencyApiFetcher
{
    private readonly string _apiUri;

    public CurrencyApiFetcher(string apiUri)
    {
        _apiUri = apiUri ?? throw new ArgumentNullException(nameof(apiUri));
    }

    public async Task<decimal?> FetchCurrencyRateAsync(string currencyCode)
    {
        using HttpClient client = new HttpClient();
        byte[] responseBytes = await client.GetByteArrayAsync(_apiUri);
        string xmlData = Encoding.GetEncoding(1251).GetString(responseBytes);
        XDocument doc = XDocument.Parse(xmlData);

        var currency = doc.Descendants("Valute")
            .FirstOrDefault(v => v.Element("CharCode")?.Value == currencyCode.ToUpper());

        if (currency != null)
        {
            string valueStr = currency.Element("Value")?.Value.Replace(",", ".");
            return decimal.TryParse(valueStr, out decimal rate) ? rate : null;
        }

        return null; // Currency not found
    }
}