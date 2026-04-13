using Project.src.Routes.Response;

namespace Project.src.Routes.Validations;

public static class ErrorHandler
{
    public static List<ApiErrorHandler> ErrorsHandle(this FluentValidation.Results.ValidationResult result)
    {
        return result.Errors.Select(error => new ApiErrorHandler
        {
            Field = error.PropertyName,
            Message = error.ErrorMessage
        }).ToList();
    }
}
