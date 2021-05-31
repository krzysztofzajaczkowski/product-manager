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
                ErrorResponse error;
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

                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                    if (contextFeature.Error is DomainException)
                    {

                        error = new ErrorResponse
                        {
                            Message = contextFeature.Error.Message
                        };
                    }

                    await context.Response.WriteAsJsonAsync(new ErrorResponse
                    {
                        Message = "Bad Request"
                    });

                });
            });
        }
    }
}