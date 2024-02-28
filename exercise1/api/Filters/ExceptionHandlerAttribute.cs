using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using StargateAPI.Business.Data.Helpers;
using StargateAPI.Controllers;
using System;
using System.Data.Entity.Core;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace StargateAPI.Filters
{
    //This attribute simplifies exception handling and provides for an easier system of scalability as more complex exceptions arise in the future
    public class ExceptionHandlerAttribute : Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute
    {
        private readonly ILogger<ExceptionHandlerAttribute> _logger;
        private readonly IExceptionLoggingHelper _exceptionLoggingHelper;

        public ExceptionHandlerAttribute(ILogger<ExceptionHandlerAttribute> logger, IExceptionLoggingHelper exceptionLoggingHelper)
        {
            _logger = logger;
            _exceptionLoggingHelper = exceptionLoggingHelper;
        }

        public override void OnException(ExceptionContext context)
        {
            var statusCode = (int)HttpStatusCode.InternalServerError;
            //Add checks for new exceptions as they are added in the future
            if(context.Exception.GetType() == typeof(ObjectNotFoundException) || 
                context.Exception.GetType() == typeof(InvalidOperationException) ||
                context.Exception.GetType() == typeof(ArgumentException))
            {
                statusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (context.Exception.GetType() == typeof(UnauthorizedAccessException))
                statusCode = (int)HttpStatusCode.Unauthorized; //In case auth is implemented

            var httpResponse = new ObjectResult(new {Success = false, Message = $"Failure: {context.Exception.Message}", ResponseCode = statusCode});
            httpResponse.StatusCode = statusCode;

            // Log the exception
            _logger.LogError("Exception occurred while executing request: {ex}", context.Exception);
            
            _exceptionLoggingHelper.PersistException(context.Exception);
            context.Result = httpResponse;
            context.HttpContext.Response.StatusCode = statusCode;
        }
    }
}