using System.Net;
using System.Text.Json;
using CitizenFileManagement.Core.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace CitizenFileManagement.API.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
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

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        var response = context.Response;
        var problemDetails = new ProblemDetails();

        switch (ex)
        {
            case ApplicationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest; // Client-side error
                problemDetails.Detail = ex.Message;
                problemDetails.Title = "Application Error";
                _logger.LogWarning($"Client-side error: {problemDetails.Title}", problemDetails.Detail);
                break;

            case KeyNotFoundException:
            case NotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound; // Client-side error
                problemDetails.Detail = ex.Message;
                problemDetails.Title = "Resource Not Found";
                _logger.LogWarning($"Client-side error: Resource not found, {problemDetails.Title}", problemDetails.Detail);
                break;

            case ValidationException exc:
                response.StatusCode = (int)HttpStatusCode.BadRequest; // Client-side error
                problemDetails = new ValidationProblemDetails(exc.Errors);
                problemDetails.Detail = ex.Message;
                problemDetails.Extensions.Add("invalidParams", exc.Errors);
                problemDetails.Title = "Validation Error";
                _logger.LogWarning($"Client-side error: Validation failed, {problemDetails.Title}", problemDetails.Detail);
                break;

            case UnAuthorizedException:
                response.StatusCode = (int)HttpStatusCode.Unauthorized; // Client-side error
                problemDetails.Detail = ex.Message;
                problemDetails.Title = "Unauthorized";
                _logger.LogWarning($"Client-side error: Unauthorized access, {problemDetails.Title}", problemDetails.Detail);
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError; // Server-side error
                problemDetails.Detail = "An unexpected error occurred.";
                problemDetails.Title = "Server Error";
                _logger.LogError(ex, $"Server-side error: {problemDetails.Title}", problemDetails.Detail);
                break;
        }

        var result = JsonSerializer.Serialize(problemDetails);
        await context.Response.WriteAsync(result);
    }
}