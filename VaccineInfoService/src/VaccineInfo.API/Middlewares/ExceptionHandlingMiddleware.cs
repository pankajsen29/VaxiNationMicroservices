using Microsoft.Extensions.Hosting;
using System.Net;
using VaccineInfo.Api.Exceptions;
using VaccineInfo.Core.Exceptions;
using VaccineInfo.Infrastructure.Exceptions;

namespace VaccineInfo.Api.Middlewares
{
    public class ExceptionHandlingMiddleware : IMiddleware
    {
        private readonly IHostEnvironment _env;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(IHostEnvironment env, ILogger<ExceptionHandlingMiddleware> logger)
        {
            this._env = env;
            this._logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (CoreNotFoundException ex)
            {
                _logger.LogError("CoreNotFoundException, Message: ", ex.Message);
                _logger.LogError("CoreNotFoundException, StakeTrace: ", ex.StackTrace);

                HttpStatusCode statusCode = HttpStatusCode.NotFound;                
                context.Response.StatusCode = (int)statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(PrepareApiError(statusCode, ex.Message, ex.StackTrace).ToString());
            }
            catch (CoreValidationException ex)
            {
                _logger.LogError("CoreValidationException, Message: ", ex.Message);
                _logger.LogError("CoreValidationException, StakeTrace: ", ex.StackTrace);

                HttpStatusCode statusCode = HttpStatusCode.BadRequest;
                context.Response.StatusCode = (int)statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(PrepareApiError(statusCode, ex.Message, ex.StackTrace).ToString());
            }
            catch (InfrastructureDBConnectionException ex)
            {
                _logger.LogError("InfrastructureDBConnectionException, Message: ", ex.Message);
                _logger.LogError("InfrastructureDBConnectionException, StakeTrace: ", ex.StackTrace);

                HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
                context.Response.StatusCode = (int)statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(PrepareApiError(statusCode, ex.Message, ex.StackTrace).ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception Message: ", ex.Message);
                _logger.LogError("Exception StakeTrace: ", ex.StackTrace);

                HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
                context.Response.StatusCode = (int)statusCode;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(PrepareApiError(statusCode, ex.Message, ex.StackTrace).ToString());
            }
        }

        private ApiError PrepareApiError(HttpStatusCode statusCode, string errorMessage, string errorDetails)
        {
            ApiError response;
            if (_env.IsDevelopment())
            {
                response = new ApiError((int)statusCode, errorMessage, errorDetails);
            }
            else
            {
                response = new ApiError((int)statusCode, errorMessage);
            }
            return response;
        }
    }
}