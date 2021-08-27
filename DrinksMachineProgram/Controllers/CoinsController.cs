using DrinksMachineProgram.BusinessLayer;
using DrinksMachineProgram.Entities;
using DrinksMachineProgram.Resources;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

using System;
using System.Collections.Generic;

namespace DrinksMachineProgram.Controllers
{

    public class CoinsController : BaseController<Coin, short>
    {

        #region CTOR

        public CoinsController(ICompositeViewEngine viewEngine) : base(viewEngine, CoinsBL.Instance) { }

        #endregion CTOR

        #region Public Methods

        // GET: /Coins/List
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        // POST: /Coins/Table/
        [HttpPost]
        public JsonResult Table()
        {
            List<Coin> coins = EntityBL.List();

            string view = GetView("../Coins/_Table", coins);

            return JsonResponses.GetSuccess(TextResources.MessajeSuccessListGenerated, view);
        }

        // GET: /Coins/Detail/5
        [HttpGet]
        public ActionResult Detail([FromQuery] short id)
        {

            try
            {
                Coin coin = EntityBL.Detail(id);

                return PartialView("_Detail", coin);
            }
            catch (Exception)
            {
                return PartialView("_ModalError");
            }

        }

        // GET: /Coins/Create
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("_Create", new Coin());
        }

        // POST: /Coins/Create
        [HttpPost]
        public ActionResult Create([FromBody] Coin coin)
        {
            if (ModelState.IsValid == false) return JsonResponses.GetError(ModelState);

            try
            {
                EntityBL.Create(coin);

                return JsonResponses.GetSuccess(TextResources.MessageSucccessRecordAdded);
            }
            catch (Exception)
            {
                return GetJsonError(TextResources.MessageErrorAddingRecord);
            }

        }

        // GET: /Coins/Edit/5
        [HttpGet]
        public ActionResult Edit([FromQuery] short id)
        {

            try
            {
                Coin coin = EntityBL.Detail(id);

                return PartialView("_Edit", coin);
            }
            catch (Exception)
            {
                return PartialView("_ModalError");
            }

        }

        // POST: /Coins/Edit
        [HttpPost]
        public ActionResult Edit([FromBody] Coin coin)
        {
            if (ModelState.IsValid == false) return JsonResponses.GetError(ModelState);

            try
            {
                EntityBL.Edit(coin);

                return JsonResponses.GetSuccess(TextResources.MessageSucccessRecordUpdated);
            }
            catch (Exception)
            {
                return GetJsonError(TextResources.MessageErrorUpdatingRecord);
            }

        }

        // POST: /Coins/Delete
        [HttpPost]
        public JsonResult Delete([FromBody] Coin coin)
        {

            try
            {
                EntityBL.Delete(coin.Id);

                return JsonResponses.GetSuccess(TextResources.MessajeSuccessRecordDeleted);
            }
            catch (Exception)
            {
                return GetJsonError(TextResources.MessageErrorDeletingRecord);
            }

        }

        #endregion Public Methods

    }

}
