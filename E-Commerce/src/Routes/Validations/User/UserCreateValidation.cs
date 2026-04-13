using FluentValidation;
using Project.src.Routes.Request.Create;

namespace Project.src.Routes.Validations.User;

public class CreateUserValidator : AbstractValidator<UserRequestCreate>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.")
        .MinimumLength(3).WithMessage("Name must be at least 3 characters long.");

        RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
        .MinimumLength(10).WithMessage("Email must be at least 10 characters long.")
        .EmailAddress().WithMessage("Invalid Email");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.")
        .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
    }
}
