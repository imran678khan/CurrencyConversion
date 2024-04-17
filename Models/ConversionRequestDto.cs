
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class ConversionRequestDto
    {
        [Required]
        public string ConversionKey { get; set; }

        [Required]
        public decimal ExchangeRate { get; set; }

    }
}
