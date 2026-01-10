using Core.Application.Common.Result;
using Microsoft.AspNetCore.Mvc;

namespace API.Extensions;

public static class ResultToActionResultExtensions
{
    public static IActionResult ToActionResult(this Result result)
    {
        if (result.IsSuccess)
        {
            return new OkResult();
        }

        return result.Error!.Type switch
        {
            ErrorType.NotFound => new NotFoundObjectResult(new { error = result.Error.Code, message = result.Error.Message }),
            ErrorType.Validation => new BadRequestObjectResult(new { error = result.Error.Code, message = result.Error.Message }),
            ErrorType.Conflict => new ConflictObjectResult(new { error = result.Error.Code, message = result.Error.Message }),
            ErrorType.Unauthorized => new UnauthorizedObjectResult(new { error = result.Error.Code, message = result.Error.Message }),
            ErrorType.Forbidden => new ObjectResult(new { error = result.Error.Code, message = result.Error.Message }) { StatusCode = 403 },
            _ => new ObjectResult(new { error = result.Error.Code, message = result.Error.Message }) { StatusCode = 500 }
        };
    }

    public static IActionResult ToActionResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return new OkObjectResult(result.Value);
        }

        return result.Error!.Type switch
        {
            ErrorType.NotFound => new NotFoundObjectResult(new { error = result.Error.Code, message = result.Error.Message }),
            ErrorType.Validation => new BadRequestObjectResult(new { error = result.Error.Code, message = result.Error.Message }),
            ErrorType.Conflict => new ConflictObjectResult(new { error = result.Error.Code, message = result.Error.Message }),
            ErrorType.Unauthorized => new UnauthorizedObjectResult(new { error = result.Error.Code, message = result.Error.Message }),
            ErrorType.Forbidden => new ObjectResult(new { error = result.Error.Code, message = result.Error.Message }) { StatusCode = 403 },
            _ => new ObjectResult(new { error = result.Error.Code, message = result.Error.Message }) { StatusCode = 500 }
        };
    }
}
