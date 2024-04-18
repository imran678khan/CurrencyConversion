using CurrencyConversion.Services.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Models;
using Newtonsoft.Json;
using Services.IService;
namespace Services
{
    public class ConversionService : IConversion
    {

        private readonly IHostingEnvironment _hostingEnvironment;

        public ConversionService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public ConversionResponseDto GetCurrencyRates(string sourceCurrency, string targetCurrency, decimal amount)
        {

            if (string.IsNullOrEmpty(sourceCurrency))
                throw new BadRequestException("sourceCurrency is not valid");

            if (string.IsNullOrEmpty(targetCurrency))
                throw new BadRequestException("targetCurrency is not valid");


            var rootPath = _hostingEnvironment.ContentRootPath;

            var fullPath = Path.Combine(rootPath, "exchangeRates.json");

            var jsonData = System.IO.File.ReadAllText(fullPath);

            var currencyRates = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonData);

            string key = $"{sourceCurrency}_TO_{targetCurrency}".ToUpper();

            var exchangeRate = Convert.ToDecimal(currencyRates?.Where(x => x.Key == key).Select(x => x.Value).FirstOrDefault());

            if (exchangeRate == 0)
                throw new NotFoundException($"{sourceCurrency.ToUpper()} To {targetCurrency.ToUpper()} exchange rate not found.");

            exchangeRate = CheckExchangeRateFromEnviroment(key, exchangeRate);

            return new ConversionResponseDto { ConvertedAmount = amount * exchangeRate, ExchangeRate = exchangeRate };

        }

        // to override exchange rate if its define in env variables
        private decimal CheckExchangeRateFromEnviroment(string key, decimal exchangeRate)
        {
            string enviromentConfigValue = Environment.GetEnvironmentVariable($"{key}");

            if (string.IsNullOrEmpty(enviromentConfigValue))
                return exchangeRate;
            else
                return Convert.ToDecimal(enviromentConfigValue);

        }


        public void UpdateConversionRate(List<ConversionRequestDto> conversionRequestDtos)
        {
            Dictionary<string, decimal> keyValuePairs = new Dictionary<string, decimal>();

            foreach (var c in conversionRequestDtos)
            {
                keyValuePairs.Add(c.ConversionKey, c.ExchangeRate);
            }

            var rootPath = _hostingEnvironment.ContentRootPath;

            var fullPath = Path.Combine(rootPath, "exchangeRates.json");

            var content = JsonConvert.SerializeObject(keyValuePairs);

            using (StreamWriter sw = new StreamWriter(fullPath))
                sw.Write(content);


        }


    }
}
