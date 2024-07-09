using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

class Program
{
    private static readonly HttpClient client = new HttpClient();

    static async Task<double> GetExchangeRate(string apiKey, string fromCurrency, string toCurrency)
    {
        string url = $"https://v6.exchangerate-api.com/v6/{apiKey}/latest/{fromCurrency}";
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        string responseBody = await response.Content.ReadAsStringAsync();

        JObject jsonData = JObject.Parse(responseBody);

        if (jsonData["result"].ToString() != "success")
        {
            Console.WriteLine("Error: " + jsonData["error-type"]);
            return -1;
        }

        double exchangeRate = (double)jsonData["conversion_rates"][toCurrency];
        return exchangeRate;
    }

    static async Task Main(string[] args)
    {
        string apiKey = "your_api_key_here";  // Replace with your actual API key
        Console.Write("Enter the currency you want to convert from (e.g., USD): ");
        string fromCurrency = Console.ReadLine().ToUpper();
        Console.Write("Enter the currency you want to convert to (e.g., EUR): ");
        string toCurrency = Console.ReadLine().ToUpper();
        Console.Write("Enter the amount you want to convert: ");
        double amount = double.Parse(Console.ReadLine());

        double exchangeRate = await GetExchangeRate(apiKey, fromCurrency, toCurrency);
        if (exchangeRate != -1)
        {
            double convertedAmount = amount * exchangeRate;
            Console.WriteLine($"{amount} {fromCurrency} is equal to {convertedAmount:F2} {toCurrency}");
        }
        else
        {
            Console.WriteLine("Failed to get the exchange rate.");
        }
    }
}

