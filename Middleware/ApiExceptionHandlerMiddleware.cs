using Newtonsoft.Json;
using RainfallREST.Model.ErrorModel;
using System.Net;
using RainfallREST.Model.ExceptionModel;
using Microsoft.AspNetCore.Mvc;

namespace RainfallREST.Middleware
{

    public class ApiExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public ApiExceptionHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode code = HttpStatusCode.InternalServerError; 
            var errorResponse = new Error
            {
                Message = "An error occurred on the server.",
                Detail = new List<ErrorDetail>()
            };

            switch (exception)
            {
                case ParameterException paramEx:
                    code = HttpStatusCode.BadRequest; 
                    errorResponse.Message = paramEx.Message;
                    errorResponse.Detail.Add(new ErrorDetail
                    {
                        PropertyName = paramEx.ParameterName,
                        Message = paramEx.DetailError,
                    });
                    break;
                case KeyNotFoundException notFoundException:
                    code = HttpStatusCode.NotFound; 
                    errorResponse.Message = notFoundException.Message;
                    
                    break;
                default:
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            var result = JsonConvert.SerializeObject(errorResponse);
            return context.Response.WriteAsync(result);
        }
    }

}

