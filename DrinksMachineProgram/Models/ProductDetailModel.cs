using DrinksMachineProgram.Entities;

using System.ComponentModel.DataAnnotations;

namespace DrinksMachineProgram.Models
{
    public class ProductDetailModel
    {

        public Product Product { get; set; }

        [Required(ErrorMessage = "The value is required.")]
        [Range(0, short.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public short Quantity { get; set; }

    }

}
