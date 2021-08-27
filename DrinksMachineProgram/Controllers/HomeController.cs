using DrinksMachineProgram.BusinessLayer;
using DrinksMachineProgram.Entities;
using DrinksMachineProgram.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;

using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DrinksMachineProgram.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        #region Public Methods

        public IActionResult Index()
        {
            List<Product> products = ProductsBL.Instance.List();
            List<Coin> coins = CoinsBL.Instance.List();

            OrderModel order = new()
            {
                Products = products
                    .Select(p => new ProductDetailModel
                    {
                        Product = p,
                        Quantity = 0
                    })
                    .ToList(),
                Coins = coins
                    .Select(c => new CoinDetailModel
                    {
                        Coin = c,
                        Quantity = 0
                    })
                    .ToList(),
            };

            return View("Index", order);
        }

        public IActionResult GetDrinks(OrderModel order)
        {
            if (ModelState.IsValid == false) return View("Index", order);

            return View("Index", order);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        #endregion Public Methods

        #region Private Methods

        #endregion Private Methods

    }

}
