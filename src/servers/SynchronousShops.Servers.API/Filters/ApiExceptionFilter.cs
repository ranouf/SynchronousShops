using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SynchronousShops.Libraries.Session;
using SynchronousShops.Servers.API.Filters.Dtos;
using System;
using System.Collections.Generic;
using System.Net;

namespace SynchronousShops.Servers.API.Filters
{

    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public sealed class ApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<ApiExceptionFilter> _logger;
        private readonly IUserSession _session;

        public ApiExceptionFilter(
            IWebHostEnvironment environment,
            ILogger<ApiExceptionFilter> logger,
            IUserSession session)
        {
            _environment = environment;
            _logger = logger;
            _session = session;
        }

        public override void OnException(ExceptionContext context)
        {
            ApiErrorDto apiError;
            var properties = new Dictionary<string, string>{
                { "UserId",  _session.UserId?.ToString() }
            };

            if (context.Exception is KeyNotFoundException)
            {
                _logger.LogWarning(context.Exception.Message, properties);

                apiError = new ApiErrorDto("Not found");
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else if (context.Exception is UnauthorizedAccessException)
            {
                _logger.LogWarning(context.Exception.Message, properties);

                apiError = new ApiErrorDto("Unauthorized Access");
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                _logger.LogError(context.Exception.Message, context.Exception, properties);

                // Unhandled errors
                if (_environment.IsDevelopment())
                {
                    apiError = new ApiErrorDto(
                        context.Exception.GetBaseException().Message,
                        context.Exception.StackTrace
                    );
                }
                else
                {
                    apiError = new ApiErrorDto(
                        "An unhandled error occurred.",
                        null
                    );
                }

                context.HttpContext.Response.StatusCode = 500;
            }

            // always return a JSON result
            context.Result = new JsonResult(apiError);

            base.OnException(context);
        }
    }
}
