using Ecommerce.API.Models;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;
using ECommerce.Application.Exceptions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        _ = app.UseExceptionHandler(appError => appError.Run(async context =>
        {
            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
            Error error = null;
            if (contextFeature != null)
            {
                var exception = contextFeature.Error;
                HttpStatusCode statusCode;
                switch (exception)
                {

                    case ECommerce.Application.Exceptions.ValidationException:
                    case BadRequestException:
                        statusCode = HttpStatusCode.BadRequest;
                        break;
                    case NotFoundException:
                        statusCode = HttpStatusCode.NotFound;
                        break;
                    case Exception:
                        statusCode = HttpStatusCode.InternalServerError;
                        break;
                    default:
                        statusCode = HttpStatusCode.InternalServerError;
                        break;
                }

                // Prepare Generic Error
                var apiError = new ApiError(contextFeature.Error.Message, error);

                // Set Response Details
                context.Response.StatusCode = (int)statusCode;
                context.Response.ContentType = "application/json";

                // Return the Serialized Generic Error
                await context.Response.WriteAsync(JsonConvert.SerializeObject(apiError));
            }
        }));

        return app;
    }
}
