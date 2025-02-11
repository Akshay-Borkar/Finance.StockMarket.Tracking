using Finance.StockMarket.Api.Models;
using Finance.StockMarket.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;

namespace Finance.StockMarket.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e) 
            {
                await HandleExceptionAsync(httpContext, e);
            }

        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;
            CustomValidationProblemDetails problem = new();

            switch (e) {
                case BadRequestException badRequestException:
                    httpStatusCode = HttpStatusCode.BadRequest;
                    problem = new CustomValidationProblemDetails
                    {
                        Title = badRequestException.Message,
                        Status = (int)httpStatusCode,
                        Detail = badRequestException.InnerException?.Message,
                        Type = nameof(BadRequestException),
                        Errors = badRequestException.ValidationErrors
                    };
                    break;
                case NotFoundException NotFound:
                    httpStatusCode = HttpStatusCode.NotFound;
                    problem = new CustomValidationProblemDetails
                    {
                        Title = NotFound.Message,
                        Status = (int)httpStatusCode,
                        Type = nameof(NotFoundException),
                        Detail = NotFound.InnerException?.Message,
                    };
                    break;
                default:
                    problem = new CustomValidationProblemDetails
                    {
                        Title = e.Message,
                        Status = (int)httpStatusCode,
                        Type = nameof(HttpStatusCode.InternalServerError),
                        Detail = e.StackTrace,
                    };
                    break;
            }

            httpContext.Response.StatusCode = (int)httpStatusCode;
            var logMessage = JsonConvert.SerializeObject(problem);
            _logger.LogError(logMessage);
            await httpContext.Response.WriteAsJsonAsync(problem);
        }
    }
}
