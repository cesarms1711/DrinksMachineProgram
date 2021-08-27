using DrinksMachineProgram.BusinessLayer;
using DrinksMachineProgram.Entities;
using DrinksMachineProgram.Resources;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace DrinksMachineProgram.Controllers
{

    public class UsersController : BaseController<User, short>
    {

        #region CTOR

        public UsersController(ICompositeViewEngine viewEngine) : base(viewEngine, UsersBL.Instance) { }

        #endregion CTOR

        #region Public Methods

        // GET: /Users/List
        [HttpGet]
        public ActionResult List()
        {
            return View();
        }

        // POST: /Users/Table/
        [HttpPost]
        public JsonResult Table()
        {
            List<User> users = EntityBL.List();

            string view = GetView("../Users/_Table", users);

            return JsonResponses.GetSuccess(TextResources.MessajeSuccessListGenerated, view);
        }

        // GET: /Users/Detail/5
        public ActionResult Detail(short id)
        {

            try
            {
                User user = EntityBL.Detail(id);

                return View("Detail", user);
            }
            catch (Exception)
            {
                return PartialView("_ModalError");
            }

        }

        // GET: /Users/Create
        public ActionResult Create()
        {
            return PartialView("_Create", new User());
        }

        // POST: /Users/Create
        [HttpPost]
        public JsonResult Create([FromBody] User user)
        {
            if (ModelState.IsValid == false) return JsonResponses.GetError(ModelState);

            ModelState.Remove("Password");

            if (user.Password != user.PasswordConfirmation)
            {
                ModelState.AddModelError("Password", "The password and password confirmation must be the same.");

                return JsonResponses.GetError(ModelState);
            }

            var hasNumbers = new Regex(@"[0-9]+");
            var hasUpperCase = new Regex(@"[A-Z]+");
            var hasLowerCase = new Regex(@"[a-z]+");
            var hasMinimunLength = new Regex(@".{8,}");
            var hasSymbols = new Regex(@"[^A-Za-z0-9]+");

            if (hasUpperCase.IsMatch(user.Password) == false)
            {
                ModelState.AddModelError(TextResources.LabelPassword, TextResources.MessageErrorUserPasswordUppercases);

                return JsonResponses.GetError(ModelState);
            }

            if (hasLowerCase.IsMatch(user.Password) == false)
            {
                ModelState.AddModelError(TextResources.LabelPassword, TextResources.MessageErrorUserPasswordLowercases);

                return JsonResponses.GetError(ModelState);
            }

            if (hasNumbers.IsMatch(user.Password) == false)
            {
                ModelState.AddModelError(TextResources.LabelPassword, TextResources.MessageErrorUserPasswordNumbers);

                return JsonResponses.GetError(ModelState);
            }

            if (hasMinimunLength.IsMatch(user.Password) == false)
            {
                ModelState.AddModelError(TextResources.LabelPassword, TextResources.MessageErrorUserPasswordLength);

                return JsonResponses.GetError(ModelState);
            }
          
            if (hasSymbols.IsMatch(user.Password) == false)
            {
                ModelState.AddModelError(TextResources.LabelPassword, TextResources.MessageErrorUserPasswordSymbols);

                return JsonResponses.GetError(ModelState);
            }

            try
            {
                var cryptoProvider = new MD5CryptoServiceProvider();
                var hashCode = cryptoProvider.ComputeHash(Encoding.ASCII.GetBytes(user.Password));

                user.CreationDate = DateTime.Now;
                user.PasswordHash = hashCode;

                EntityBL.Create(user);

                return JsonResponses.GetSuccess(TextResources.MessageSucccessRecordAdded);
            }
            catch (Exception)
            {
                return GetJsonError(TextResources.MessageErrorAddingRecord);
            }

        }

        // GET: /Users/Edit/5
        public ActionResult Edit(short id)
        {

            try
            {
                User user = EntityBL.Detail(id);

                return PartialView("_Edit", user);
            }
            catch (Exception)
            {
                return PartialView("_ModalError");
            }

        }

        // POST: /Users/Edit
        [HttpPost]
        public JsonResult Edit([FromBody] User user)
        {
            ModelState.Remove("Password");
            ModelState.Remove("PasswordConfirmation");

            if (ModelState.IsValid == false) return JsonResponses.GetError(ModelState);

            try
            {
                EntityBL.Edit(user);

                return JsonResponses.GetSuccess(TextResources.MessageSucccessRecordUpdated);
            }
            catch (Exception)
            {
                return GetJsonError(TextResources.MessageErrorUpdatingRecord);
            }

        }

        // POST: /Users/Delete
        [HttpPost]
        public JsonResult Delete([FromBody] User user)
        {

            try
            {
                EntityBL.Delete(user.Id);

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