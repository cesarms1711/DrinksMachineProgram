using System.Collections.Generic;

namespace DrinksMachineProgram.Models
{
    public class OrderModel
    {

        public List<ProductDetailModel> Products { get; set; }

        public List<CoinDetailModel> Coins { get; set; }

        public bool StatusOk { get; set; }

        public string StatusMessaje { get; set; }

        public decimal Pay { get; set; }

        public decimal Total { get; set; }

        public decimal Change { get; set; }

        public string UpdatedView { get; set; }

    }

}
