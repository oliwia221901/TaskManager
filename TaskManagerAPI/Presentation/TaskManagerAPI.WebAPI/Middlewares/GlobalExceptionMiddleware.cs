using System.Net;
using System.Text.Json;
using FluentValidation;
using TaskManagerAPI.Application.Common.Exceptions;

namespace TaskManagerAPI.WebAPI.Middlewares
{
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
            catch (ValidationException ex)
            {
                await HandleValidationExceptionAsync(context, ex);
            }
            catch (NotFoundException ex)
            {
                await HandleNotFoundExceptionAsync(context, ex);
            }
            catch (BadRequestException ex)
            {
                await HandleBadRequestExceptionAsync(context, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleUnauthorizedAccessExceptionAsync(context, ex);
            }
            catch (InvalidOperationException ex)
            {
                await HandleInvalidOperationExceptionAsync(context, ex);
            }
            catch (ForbiddenAccessException ex)
            {
                await HandleForbiddenAccessExceptionAsync(context, ex);
            }
            catch (ResourceConflictException ex)
            {
                await HandleResourceConflictExceptionAsync(context, ex);
            }
        }

        private Task HandleValidationExceptionAsync(HttpContext context, ValidationException ex)
        {
            _logger.LogError(ex, "Validation error occurred.");

            var response = new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = "Validation failed",
                Errors = ex.Errors.Select(error => error.ErrorMessage)
            };

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private Task HandleNotFoundExceptionAsync(HttpContext context, NotFoundException ex)
        {
            _logger.LogError(ex, "Not found error occurred.");

            var response = new
            {
                StatusCode = (int)HttpStatusCode.NotFound,
                Message = ex.Message
            };

            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private Task HandleBadRequestExceptionAsync(HttpContext context, BadRequestException ex)
        {
            _logger.LogError(ex, "Bad request error occurred.");

            var response = new
            {
                StatusCode = (int)HttpStatusCode.BadRequest,
                Message = ex.Message
            };

            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private Task HandleUnauthorizedAccessExceptionAsync(HttpContext context, UnauthorizedAccessException ex)
        {
            _logger.LogError(ex, "Unauthorized access error occurred.");

            var response = new
            {
                StatusCode = (int)HttpStatusCode.Unauthorized,
                Message = ex.Message
            };

            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private Task HandleInvalidOperationExceptionAsync(HttpContext context, InvalidOperationException ex)
        {
            _logger.LogError(ex, "Invalid operation error occurred.");

            var response = new
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = ex.Message
            };

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private Task HandleForbiddenAccessExceptionAsync(HttpContext context, ForbiddenAccessException ex)
        {
            _logger.LogError(ex, "Forbidden access error occurred.");

            var response = new
            {
                StatusCode = (int)HttpStatusCode.Forbidden,
                Message = ex.Message
            };

            context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private Task HandleResourceConflictExceptionAsync(HttpContext context, ResourceConflictException ex)
        {
            _logger.LogError(ex, "Resource conflict error occurred.");

            var response = new
            {
                StatusCode = (int)HttpStatusCode.Conflict,
                Message = ex.Message
            };

            context.Response.StatusCode = (int)HttpStatusCode.Conflict;
            context.Response.ContentType = "application/json";

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
