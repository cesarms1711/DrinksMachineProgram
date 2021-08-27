using DrinksMachineProgram.BusinessLayer;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System.IO;
using System.Threading.Tasks;

namespace DrinksMachineProgram.Controllers
{

    public abstract class BaseController<TEntity, TIndex> : Controller
    {

        #region Private Properties

        /// <summary>
        /// Object of type ICompositeViewEngine that allows generating views from the controller as strings.
        /// </summary>
        private ICompositeViewEngine ViewEngine { get; }

        protected IEntityBL<TEntity, TIndex> EntityBL;

        #endregion Private Properties

        #region CTOR

        protected BaseController(
            ICompositeViewEngine viewEngine,
            IEntityBL<TEntity, TIndex> entityBL)
        {
            ViewEngine = viewEngine;
            EntityBL = entityBL;
        }

        #endregion CTOR

        #region Protected Methods

        protected JsonResult GetJsonError(string message)
        {
            return JsonResponses.GetError(message);
        }

        protected ViewResult GetErrorModelState(
            string message,
            ViewResult view)
        {
            ModelState.AddModelError("UnexpectedError", message);

            return view;
        }

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

        #endregion Protected Methods

    }

}
