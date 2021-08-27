using System.Collections.Generic;

namespace DrinksMachineProgram.Models
{
    public class OrderModel
    {

        public List<ProductDetailModel> Products { get; set; }

        public List<CoinDetailModel> Coins { get; set; }

    }

}
