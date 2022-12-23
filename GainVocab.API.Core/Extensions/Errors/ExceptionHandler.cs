using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using FluentValidation;
using GainVocab.API.Core.Exceptions;
using GainVocab.API.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ValidationException = FluentValidation.ValidationException;

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
                UnauthorizedException unauthorizedException => HandleUnauthorizedException(unauthorizedException),
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

            var error = new ErrorResponse
            {
                Title = notFoundException.Message,
                StatusCode = HttpStatusCode.NotFound,
            };

            error.Errors = new List<ErrorEntry>();
            error.Errors.Add(new ErrorEntry
            {
                Code = HttpStatusCode.NotFound.ToString(),
                Title = notFoundException.Message,
                Source = ""
            });

            return error;
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

            var error = new ErrorResponse
            {
                Title = badRequestException.Message,
                StatusCode = HttpStatusCode.BadRequest
            };

            if (badRequestException.Errors != null && badRequestException.Errors.Any())
            {
                error.Errors = new List<ErrorEntry>();
                error.Errors.AddRange(badRequestException.Errors.Select(e => e).ToList());
            }

            return error;
        }

        private ErrorResponse HandleUnauthorizedException(UnauthorizedException unauthorizedException)
        {
            Logger.LogInformation(unauthorizedException, unauthorizedException.Message);

            var error = new ErrorResponse
            {
                Title = unauthorizedException.Message,
                StatusCode = HttpStatusCode.BadRequest,
            };

            error.Errors = new List<ErrorEntry>();
            error.Errors.Add(new ErrorEntry
            {
                Code = ((int)HttpStatusCode.Unauthorized).ToString(),
                Title = unauthorizedException.Message,
                Source = ""
            });

            return error;
        }

        private ErrorResponse HandleUnhandledExceptions(Exception exception)
        {
            Logger.LogError(exception, exception.Message);

            var error = new ErrorResponse
            {
                Title = "An unhandled error occurred while processing this request",
                StatusCode = HttpStatusCode.InternalServerError,
            };

            error.Errors = new List<ErrorEntry>();
            error.Errors.Add(new ErrorEntry
            {
                Code = ((int)HttpStatusCode.InternalServerError).ToString(),
                Title = "An unhandled error occurred while processing this request",
                Source = ""
            });

            return error;
        }
    }
}
