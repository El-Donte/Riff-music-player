using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using RiffBackend.API.Responses;
using RiffBackend.Core.Shared;

namespace RiffBackend.API.Extensions;

public static class ResponseExtensions
{
    public static IActionResult ToValidationErrorResponse(this ValidationResult result)
    {
        if (result.IsValid)
        {
            throw new InvalidOperationException("Result cant be success");
        }
        
        var validationErrors = result.Errors;

        var responseErrors = from validationError in validationErrors
                             let error = Error.Deserialize(validationError.ErrorMessage)
                             let responseError = new ResponseError(error.Code, error.Message, validationError.PropertyName)
                             select responseError;

        return new ObjectResult(Envelope.Error(responseErrors.ToList()))
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    }
}
