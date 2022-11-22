using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using FluentValidation;
using GainVocab.API.Core.Exceptions;
using GainVocab.API.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace GainVocab.API.Core.Extensions.Errors
{

    public class ExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<ExceptionHandler> Logger;
        private readonly IHostEnvironment Environment;

        public ExceptionHandler(ILogger<ExceptionHandler> logger,
            IHostEnvironment environment)
        {
            Logger = logger;
            Environment = environment;
        }

        public async Task<ErrorResponse> HandleException(Exception ex)
        {
            var error = ex switch
            {
                ValidationException validationException => HandleValidationException(validationException),
                BadRequestException domainException => HandleDomainException(domainException),
                NotFoundException resourceNotFoundException => HandleResourceNotFoundException(resourceNotFoundException),
                UnauthorizedAccessException unauthorizedException => HandleUnauthorizedException(unauthorizedException),
                _ => HandleUnhandledExceptions(ex)
            };

            if (Environment.IsDevelopment())
            {
                error.Exception = ex.ToString();
            }

            return error;
        }

        private ErrorResponse HandleResourceNotFoundException(NotFoundException notFoundException)
        {
            Logger.LogInformation(notFoundException, notFoundException.Message);

            return new ErrorResponse
            {
                Title = notFoundException.Message,
                StatusCode = HttpStatusCode.NotFound,
            };
        }

        private ErrorResponse HandleValidationException(ValidationException validationException)
        {
            Logger.LogInformation(validationException, validationException.Message);

            var error = new ErrorResponse
            {
                Title = validationException.Message,
                StatusCode = HttpStatusCode.BadRequest
            };

            if (validationException.Errors != null && validationException.Errors.Any())
            {
                error.Errors = new List<ErrorEntry>();

                error.Errors.AddRange(validationException.Errors.Select(validationError =>
                    new ErrorEntry
                    {
                        Code = validationError.ErrorCode,
                        Title = validationError.ErrorMessage,
                        Source = validationError.PropertyName
                    }));
            }

            return error;
        }

        private ErrorResponse HandleDomainException(BadRequestException badRequestException)
        {
            Logger.LogInformation(badRequestException, badRequestException.Message);

            return new ErrorResponse
            {
                Title = badRequestException.Message,
                StatusCode = HttpStatusCode.BadRequest
            };
        }

        private ErrorResponse HandleUnauthorizedException(UnauthorizedAccessException unauthorizedException)
        {
            Logger.LogInformation(unauthorizedException, unauthorizedException.Message);

            return new ErrorResponse
            {
                Title = unauthorizedException.Message,
                StatusCode = HttpStatusCode.Unauthorized
            };
        }

        private ErrorResponse HandleUnhandledExceptions(Exception exception)
        {
            Logger.LogError(exception, exception.Message);

            return new ErrorResponse
            {
                Title = "An unhandled error occurred while processing this request",
                StatusCode = HttpStatusCode.InternalServerError
            };
        }
    }
}
