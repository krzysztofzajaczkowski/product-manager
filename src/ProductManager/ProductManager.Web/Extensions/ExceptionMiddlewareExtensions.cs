using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using ProductManager.Core.Exceptions;
using ProductManager.Web.Responses;

namespace ProductManager.Web.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                var error = new ErrorResponse
                {
                    Message = "Bad Request"
                };
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature == null)
                    {
                        await context.Response.WriteAsJsonAsync(new ErrorResponse
                        {
                            Message = "Internal Server Error."
                        });
                        return;
                    }

                    if (contextFeature.Error != null)
                    {
                        error = new ErrorResponse
                        {
                            Message = contextFeature.Error.Message
                        };

                        // Test Server does not implement CompleteAsync, which is required to allow for 404 response from exception middleware
                        if (contextFeature.Error is NotFoundException)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                            await context.Response.WriteAsJsonAsync(error);
                            await context.Response.CompleteAsync();
                            return;
                        }

                        if (contextFeature.Error is DomainException)
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                            await context.Response.WriteAsJsonAsync(error);
                            return;

                        }

                        
                    }

                    await context.Response.WriteAsJsonAsync(error);
                });
            });
        }
    }
}