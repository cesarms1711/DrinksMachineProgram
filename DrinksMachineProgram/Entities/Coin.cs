using System;
using System.ComponentModel.DataAnnotations;

namespace DrinksMachineProgram.Entities
{

    public class Coin
    {

        #region Properties

        [Key]
        public short Id { get; set; }

        [Required(ErrorMessage = "The name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The value is required.")]
        [Range(0, short.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public decimal Value { get; set; }

        [Required(ErrorMessage = "The quantity available is required.")]
        [Range(0, short.MaxValue, ErrorMessage = "The quantity available must be greater than 0")]
        public short QuantityAvailable { get; set; }

        #endregion Properties

        #region Methods


        #endregion Methods

    }

}
