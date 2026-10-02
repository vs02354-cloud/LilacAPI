using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using LilacTechSys.Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace LilacTechSys.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred during HTTP request execution: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var errors = new System.Collections.Generic.List<string>();
            var current = exception;
            while (current != null)
            {
                if (!string.IsNullOrWhiteSpace(current.Message) && !errors.Contains(current.Message))
                {
                    errors.Add(current.Message);
                }
                current = current.InnerException;
            }

            if (errors.Count == 0)
            {
                errors.Add("An unknown internal server exception occurred.");
            }

            var response = ApiResponse.ErrorResult(
                "An unexpected server error occurred. Our engineering team has been alerted.",
                errors
            );

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return context.Response.WriteAsync(json);
        }
    }
}
