using Microsoft.AspNetCore.Mvc;
using RiffBackend.Core.Shared;

namespace RiffBackend.API.Extensions
{
    public static class ResultExtensions
    {
        public static IActionResult ToActionResult<T>(this Result<T> result, Func<T, IActionResult> success)
        {
            return result.Match(
                success,
                error => error.Type switch
                {
                    ErrorType.NotFound => new NotFoundObjectResult(error),
                    ErrorType.Conflict => new ConflictObjectResult(error),
                    ErrorType.Validation => new BadRequestObjectResult(error),
                    ErrorType.Internal => new ObjectResult(error)
                    {
                        StatusCode = 500,
                    },
                    _ => new BadRequestObjectResult(error)
                }
            );
        }
    }
}
