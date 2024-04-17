using Microsoft.AspNetCore.Mvc;
using Models;
using Services.IService;

namespace CurrencyConversion.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ConvertController : ControllerBase
    {

        private readonly IConversion _conversionService;

        public ConvertController(IConversion conversionService)
        {
            _conversionService = conversionService;
        }


        /// <summary>
        /// Converts a specified amount from a source currency to a target currency.
        /// </summary>
        /// <returns>{exchangeRate: (decimal), convertedAmount : (decimal)} .</returns>
        // GET: Convert

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ConversionResponseDto> Get(string sourceCurrency, string targetCurrency, decimal amount)
        {
            var response = _conversionService.GetCurrencyRates(sourceCurrency, targetCurrency, amount);

            return Ok(response);
        }


        /// <summary>
        /// Update exchange values in exchangeRates.json file so that we dont need to restart our application.
        /// </summary>
        /// <returns>200.</returns>
        // GET: Convert
        [HttpPost("update-exchange-rate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<ConversionResponseDto> Post(List<ConversionRequestDto> conversionRequestDtos)
        {
            _conversionService.UpdateConversionRate(conversionRequestDtos);

            return Ok();
        }

    }
}
