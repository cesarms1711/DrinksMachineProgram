using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

using System;
using System.Linq;
using System.Linq.Expressions;

namespace DrinksMachineProgram
{

    public static class HtmlHelpers
    {

        public static IHtmlContent GetValidationSummary(this IHtmlHelper htmlHelper)
        {
            object htmlAttributes = new
            {
                @class = "text-danger",
                id = "ValidationSummary"
            };

            return htmlHelper.ValidationSummary(false, string.Empty, htmlAttributes);
        }

        public static IHtmlContent GetValidationMessageFor<TModel, TValue>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression)
        {
            object htmlAttributes = new
            {
                @class = "alert alert-danger"
            };

            return htmlHelper.ValidationMessageFor(expression, string.Empty, htmlAttributes);
        }

        public static IHtmlContent GetLabelFor<TModel, TValue>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression,
            string classes)
        {
            object htmlAttributes = new
            {
                @class = classes + " form-label"
            };

            return htmlHelper.LabelFor(expression, htmlAttributes);
        }

        public static IHtmlContent GetDisplayFor<TModel, TValue>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression)
        {
            ModelExpressionProvider modelExpressionProvider = (ModelExpressionProvider) htmlHelper.ViewContext.HttpContext.RequestServices.GetService(typeof(IModelExpressionProvider));
            ModelMetadata metaData = modelExpressionProvider
                .CreateModelExpression(htmlHelper.ViewData, expression)
                .Metadata;

            object htmlAttributes = new
            {
                @class = "form-control-plaintext",
                @readonly = "true",
                id = metaData.PropertyName
            };

            return htmlHelper.TextBoxFor(expression, htmlAttributes);
        }

        public static IHtmlContent GetPasswordFor<TModel, TValue>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression)
        {
            ModelExpressionProvider modelExpressionProvider = (ModelExpressionProvider)htmlHelper.ViewContext.HttpContext.RequestServices.GetService(typeof(IModelExpressionProvider));
            ModelMetadata metaData = modelExpressionProvider
                .CreateModelExpression(htmlHelper.ViewData, expression)
                .Metadata;

            object htmlAttributes = new
            {
                id = metaData.PropertyName,
                @class = "form-control"
            };

            return htmlHelper.PasswordFor(expression, htmlAttributes);
        }

        /// <summary>
        /// Returns HTML markup for the <paramref name="expression" />, using a editor template and specified additional view data.
        /// Optionally, the id, placeholder and data-type attributes can be specified.
        /// </summary>
        /// <createddate>03-07-2019</createddate>
        /// <creator>César Mendoza Solera</creator>
        /// <lastmodificationdate></lastmodificationdate>
        /// <lastmodifier></lastmodifier>
        /// <param name="htmlHelper"></param>
        /// <param name="expression">An expression to be evaluated against the current model.</param>
        /// <param name="id">The id attribute of the HTML element.</param>
        /// <param name="placeHolder">The placeholder attribute of the HTML element.</param>
        /// <param name="dataType">The data-type attribute of the HTML element.</param>
        /// <returns>HTML element generated for the <paramref name="expression"/>.</returns>
        public static IHtmlContent GetEditorFor<TModel, TValue>(
            this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TValue>> expression,
            string id = "",
            string placeHolder = "",
            string dataType = "",
            string classes = "")
        {
            classes += " form-control";

            ModelExpressionProvider modelExpressionProvider = (ModelExpressionProvider)htmlHelper.ViewContext.HttpContext.RequestServices.GetService(typeof(IModelExpressionProvider));
            string propertyName = modelExpressionProvider
                .CreateModelExpression(htmlHelper.ViewData, expression)
                .Metadata
                .PropertyName;

            if (htmlHelper.ViewData.ModelState[propertyName]?.Errors != null &&
                htmlHelper.ViewData.ModelState[propertyName].Errors.Any())
            {
                classes += " invalid";
            }

            object viewData = new {
                htmlAttributes = new
                {
                    id,
                    @class = classes,
                    placeholder = placeHolder,
                    data_type = dataType
                }
            };

            return htmlHelper.EditorFor(expression, viewData);
        }

    }

}
