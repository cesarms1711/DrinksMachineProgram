using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using System.Linq;

namespace DrinksMachineProgram
{

    public class JsonResponses
    {

        public static JsonResult GetSuccess(
            string message,
            string data = "")
        {
            var value = new
            {
                Success = true,
                SuccessMessage = message,
                Data = data
            };

            return new JsonResult(value);
        }

        public static JsonResult GetError(string message)
        {
            var value = new
            {
                Success = false,
                Error = true,
                ErrorMessage = message
            };

            return new JsonResult(value);
        }

        public static JsonResult GetError(ModelStateDictionary modelState)
        {
            var errores = modelState
                .Where(e => e.Value.Errors.Any())
                .Select(e => new
                {
                    Property = e.Key,
                    Messages = e.Value.Errors.Select(m => m.ErrorMessage)
                });

            var value = new
            {
                Success = false,
                Error = false,
                ErrorList = errores
            };

            return new JsonResult(value);
        }

    }

}
