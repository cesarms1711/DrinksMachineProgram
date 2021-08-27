using DrinksMachineProgram.BusinessLayer;
using DrinksMachineProgram.Entities;
using DrinksMachineProgram.Models;
using DrinksMachineProgram.Resources;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DrinksMachineProgram.Controllers
{

    [AllowAnonymous]
    public class HomeController : Controller
    {

        #region Private Properties

        /// <summary>
        /// Object of type ICompositeViewEngine that allows generating views from the controller as strings.
        /// </summary>
        private ICompositeViewEngine ViewEngine { get; }

        #endregion Private Properties

        #region CTOR

        public HomeController(ICompositeViewEngine viewEngine)
        {
            ViewEngine = viewEngine;
        }

        #endregion CTOR


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


        [HttpPost]
        public JsonResult GetDrinks([FromBody] OrderModel order)
        {
            bool statusOk = false;

            if (ValidateProducts(ref order) && 
                ValidatePay(ref order) && 
                CalculateChange(ref order))
            {
                order.Products
                    .Where(p => p.Quantity > 0)
                    .ToList()
                    .ForEach(p => ProductsBL.Instance.Edit(p.Product));

                order.Coins
                    .Where(c => 
                        c.Coin.QuantityAdded > 0 ||
                        c.Coin.QuantityRemoved > 0)
                    .ToList()
                    .ForEach(c => CoinsBL.Instance.Edit(c.Coin));

                statusOk = true;
            }

            string view = GetView("_OrderResult", order);

            return statusOk ? 
                JsonResponses.GetSuccess(TextResources.MessageSuccessOrder, view, order) :
                JsonResponses.GetError(order.StatusMessaje);
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

        /// <summary>
        /// Method that allows to generate a view as a string.
        /// </summary>
        /// <createddate>09-17-2019</createddate>
        /// <creator>César Mendoza Solera</creator>
        /// <param name="view">Name or location of the view.</param>
        /// <param name="model">Object used as the model object of the view.</param>
        /// <returns>View rendered as a string.</returns>
        protected string GetView(
            string view,
            object model)
        {
            ViewData.Model = model;

            StringWriter stringWriter = new();

            ViewEngineResult viewResult = ViewEngine.FindView(ControllerContext, view, false);
            ViewContext viewContext = new(
                ControllerContext,
                viewResult.View,
                ViewData,
                TempData,
                stringWriter,
                new HtmlHelperOptions());
            Task thread = viewResult.View.RenderAsync(viewContext);

            thread.Wait();

            return stringWriter.GetStringBuilder().ToString();
        }

        public bool ValidateProducts(ref OrderModel order)
        {
            order.Products.ForEach(orderProduct =>
            {
                orderProduct.Product.QuantityAvailable -= orderProduct.Quantity;
            });

            bool insufficientProducts = order.Products.Any(p => p.Product.QuantityAvailable < 0);

            if (insufficientProducts)
            {
                RestoreProducts(ref order);

                order.StatusMessaje = TextResources.MessageErrorDrinkSoldOut;

                return false;
            }

            return true;
        }

        public bool ValidatePay(ref OrderModel order)
        {
            order.Pay = order.Coins
                .Where(c => c.Quantity > 0)
                .Sum(c => c.Quantity * c.Coin.Value);
            order.Total = order.Products
                .Where(p => p.Quantity > 0)
                .Sum(p => p.Quantity * p.Product.Cost);

            if (order.Pay < order.Total)
            {
                RestoreProducts(ref order);

                order.StatusMessaje = TextResources.MessageNotEnoughMoney;

                return false;
            }

            return true;
        }

        public bool CalculateChange(ref OrderModel order)
        {
            decimal change = order.Pay - order.Total;

            order.Change = change;

            order.Coins.ForEach(orderCoin =>
            {
                orderCoin.Coin.QuantityAvailable += orderCoin.Quantity;
                orderCoin.Coin.QuantityAdded = orderCoin.Quantity;
                orderCoin.Quantity = 0;

                if (orderCoin.Coin.QuantityAvailable > 0 &&
                    orderCoin.Coin.Value <= change)
                {

                    do
                    {
                        change -= orderCoin.Coin.Value;

                        orderCoin.Coin.QuantityAvailable--;
                        orderCoin.Coin.QuantityRemoved++;
                        orderCoin.Quantity++;
                    }
                    while (orderCoin.Coin.QuantityAvailable > 0 &&
                        orderCoin.Coin.Value <= change);
                }

            });

            if (change > 0)
            {
                RestoreProducts(ref order);
                RestoreCoins(ref order);

                order.StatusMessaje = TextResources.MessageNotSufficientChange;

                return false;
            }

            return true;
        }

        private void RestoreCoins(ref OrderModel order)
        {
            order.Coins.ForEach(orderCoin =>
            {
                orderCoin.Quantity = orderCoin.Coin.QuantityAdded;

                orderCoin.Coin.QuantityAvailable -= orderCoin.Coin.QuantityAdded;
                orderCoin.Coin.QuantityAvailable += orderCoin.Coin.QuantityRemoved;
            });
        }

        private void RestoreProducts(ref OrderModel order)
        {
            order.Products.ForEach(orderProduct =>
            {
                orderProduct.Quantity = orderProduct.Quantity;
                orderProduct.Product.QuantityAvailable += orderProduct.Quantity;
            });
        }

        #endregion Private Methods

    }

}
