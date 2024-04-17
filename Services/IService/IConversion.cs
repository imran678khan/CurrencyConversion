using Models;

namespace Services.IService
{
    public interface IConversion
    {
        ConversionResponseDto GetCurrencyRates(string sourceCurrency, string targetCurrency, decimal amount);
        void UpdateConversionRate(List<ConversionRequestDto> conversionRequestDtos);
    }
}
