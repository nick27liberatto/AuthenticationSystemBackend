namespace Application.Extensions
{
    using FluentResults;
    using Microsoft.AspNetCore.Mvc;

    public static class ResultExtensions
    {
        public static IActionResult ToActionResult<T>(this Result<T> result, string? endpoint = null)
        {
            if (result.IsSuccess)
            {
                if (endpoint == "Register")
                    return new CreatedResult(string.Empty, result.Value);

                return new OkObjectResult(result.Value);
            }

            if (endpoint == "Login" && result.Errors.Any(e => e.Message.Contains("InvalidCredentials")))
                return new UnauthorizedObjectResult(new { errors = result.Errors.Select(e => e.Message) });

            return new BadRequestObjectResult(new { errors = result.Errors.Select(e => e.Message) });
        }

        public static IActionResult ToActionResult(this Result result)
        {
            if (result.IsSuccess) return new OkResult();

            return new BadRequestObjectResult(new { errors = result.Errors.Select(e => e.Message) });
        }
    }

}
