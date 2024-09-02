


using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
        catch (FluentValidation.ValidationException ex)
        {
            var errors = ex.Errors
                .Select(error => new { error.PropertyName, error.ErrorMessage })
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToList());

            await HandleExceptionAsync(HttpStatusCode.BadRequest, ex.Message, context, errors);
        }

        
        catch (Exception ex)
        {

            // await HandleExceptionAsync(context, ex);
            var (statusCode, message) = GetExceptionDetails(ex);
            await HandleExceptionAsync(statusCode, message, context);
        }
    }
    // func to get status code and message
    private static (HttpStatusCode StatusCode, string Message) GetExceptionDetails(Exception ex)
    {
        if (ex is ApiException apiEx)
        {
            return (apiEx.StatusCode, apiEx.Message);
        }

        return (HttpStatusCode.InternalServerError, "An unexpected error occurred."+ex.Message);
    }

    private Task HandleExceptionAsync(HttpStatusCode statusCode, string message,  HttpContext context,  object data = null)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var response = new ApiResponse<string>
        {
            Success = false,
            Message = message

        };

        var responseJson = JsonSerializer.Serialize(response);
        return context.Response.WriteAsync(responseJson);
    }
}







// // using Microsoft.AspNetCore.Http;
// // using Microsoft.Extensions.Logging;
// // using System;
// // using System.Collections.Generic;
// // using System.Net;
// // using System.Text.Json;
// // using System.Threading.Tasks;
// // // using API.Responses;
// // // using API.Exceptions;

// // public class GlobalExceptionMiddleware
// // {
// //     private readonly RequestDelegate _next;
// //     private readonly ILogger<GlobalExceptionMiddleware> _logger;

// //     private static readonly Dictionary<Type, (HttpStatusCode StatusCode, string Message)> ExceptionMappings = new()
// //     {
// //         { typeof(NotFoundException), (HttpStatusCode.NotFound, "Resource not found.") },
// //         { typeof(BadRequestException), (HttpStatusCode.BadRequest, "Invalid input provided.") },
// //         { typeof(UnauthorizedAccessException), (HttpStatusCode.Unauthorized, "Access is denied.") },
// //         { typeof(NotImplementedException), (HttpStatusCode.NotImplemented, "This feature is not implemented.") }
// //     };

// //     public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
// //     {
// //         _next = next;
// //         _logger = logger;
// //     }

// //     public async Task InvokeAsync(HttpContext context)
// //     {
// //         try
// //         {
// //             await _next(context);
// //         }
// //         catch (Exception ex)
// //         {
// //             await HandleExceptionAsync(context, ex);
// //         }
// //     }

// //     private Task HandleExceptionAsync(HttpContext context, Exception ex)
// //     {
// //         var (statusCode, message) = GetExceptionDetails(ex);

// //         context.Response.ContentType = "application/json";
// //         context.Response.StatusCode = (int)statusCode;

// //         var response = new ApiResponse<string>
// //         {
// //             Success = false,
// //             Message = message
// //         };

// //         var responseJson = JsonSerializer.Serialize(response);
// //         return context.Response.WriteAsync(responseJson);
// //     }

// //     private (HttpStatusCode StatusCode, string Message) GetExceptionDetails(Exception ex)
// //     {
// //         if (ExceptionMappings.TryGetValue(ex.GetType(), out var details))
// //         {
// //             return details;
// //         }

// //         _logger.LogError(ex, "An unhandled exception occurred.");
// //         return (HttpStatusCode.InternalServerError, "An unexpected error occurred.");
// //     }
// // }
