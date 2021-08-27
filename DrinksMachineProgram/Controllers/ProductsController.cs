using DrinksMachineProgram.BusinessLayer;
using DrinksMachineProgram.Entities;
using DrinksMachineProgram.Resources;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

using System;
using System.Collections.Generic;

namespace DrinksMachineProgram.Controllers
{

    public class ProductsController : BaseController<Product, short>
    {

        #region CTOR

        public ProductsController(ICompositeViewEngine viewEngine) : base(viewEngine, ProductsBL.Instance) { }

        #endregion CTOR

        #region Public Methods

        // GET: /Products/List
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        // POST: /Products/Table/
        [HttpPost]
        public JsonResult Table()
        {
            List<Product> products = EntityBL.List();

            string view = GetView("../Products/_Table", products);

            return JsonResponses.GetSuccess(TextResources.MessajeSuccessListGenerated, view);
        }

        // GET: /Products/Detail/5
        [HttpGet]
        public ActionResult Detail([FromQuery] short id)
        {

            try
            {
                Product product = EntityBL.Detail(id);

                return PartialView("_Detail", product);
            }
            catch (Exception)
            {
                return PartialView("_ModalError");
            }

        }

        // GET: /Products/Create
        [HttpGet]
        public ActionResult Create()
        {
            return PartialView("_Create", new Product());
        }

        // POST: /Products/Create
        [HttpPost]
        public ActionResult Create([FromBody] Product product)
        {
            if (ModelState.IsValid == false) return JsonResponses.GetError(ModelState);

            try
            {
                EntityBL.Create(product);

                return JsonResponses.GetSuccess(TextResources.MessageSucccessRecordAdded);
            }
            catch (Exception)
            {
                return GetJsonError(TextResources.MessageErrorAddingRecord);
            }

        }

        // GET: /Products/Edit/5
        [HttpGet]
        public ActionResult Edit([FromQuery] short id)
        {

            try
            {
                Product product = EntityBL.Detail(id);

                return PartialView("_Edit", product);
            }
            catch (Exception)
            {
                return PartialView("_ModalError");
            }

        }

        // POST: /Products/Edit
        [HttpPost]
        public ActionResult Edit([FromBody] Product product)
        {
            if (ModelState.IsValid == false) return JsonResponses.GetError(ModelState);

            try
            {
                EntityBL.Edit(product);

                return JsonResponses.GetSuccess(TextResources.MessageSucccessRecordUpdated);
            }
            catch (Exception)
            {
                return GetJsonError(TextResources.MessageErrorUpdatingRecord);
            }

        }

        // POST: /Products/Delete
        [HttpPost]
        public JsonResult Delete([FromBody] Product product)
        {

            try
            {
                EntityBL.Delete(product.Id);

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
