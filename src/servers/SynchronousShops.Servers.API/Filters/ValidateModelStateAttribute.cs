using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace SynchronousShops.Servers.API.Filters
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public sealed class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                throw new Exception(context.ModelState.First().Value.Errors.First().ErrorMessage);
            }
        }
    }
}
