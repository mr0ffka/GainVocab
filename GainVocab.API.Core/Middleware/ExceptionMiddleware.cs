using GainVocab.API.Core.Exceptions;
using GainVocab.API.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace GainVocab.API.Core.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate Next;
        private readonly IExceptionHandler ExceptionHandler;
        private readonly ILogger<ExceptionMiddleware> Logger;

        public ExceptionMiddleware(RequestDelegate next, IExceptionHandler exceptionHandler, ILogger<ExceptionMiddleware> logger)
        {
            this.Next = next;
            this.ExceptionHandler = exceptionHandler;
            this.Logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await Next(httpContext);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, $"Something Went wrong while processing {httpContext.Request.Path}");
                var error = await ExceptionHandler.HandleException(ex);
                if (!httpContext.Response.HasStarted)
                {
                    httpContext.Response.Clear();
                    httpContext.Response.ContentType = MediaTypeNames.Application.Json;
                    httpContext.Response.StatusCode = (int)error.StatusCode;
                    await httpContext.Response.WriteAsync(JsonSerializer.Serialize(
                        error));
                }
            }
        }
    }
}
