using CurrencyConversion.Services.Exceptions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Moq;
using Services;
using System.Threading;

namespace CurrencyConversion.UnitTests
{
    public class ConversionServiceTest
    {

        [Fact]
        public void GetCurrencyRates_Success()
        {
            string sourecCurrency = "";
            string targetCurrency = "INR";
            decimal amount = 100;

            var ihostingMoq = new Mock<Microsoft.AspNetCore.Hosting.IHostingEnvironment>();

            var conversionService = new ConversionService(ihostingMoq.Object);

            Assert.Throws<BadRequestException>(() => conversionService.GetCurrencyRates(sourecCurrency, targetCurrency, amount));


        }


    }
}