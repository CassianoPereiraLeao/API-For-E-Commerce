using FluentValidation;
using Project.src.Routes.Request.Login;

namespace Project.src.Routes.Validations.User;

public class LoginUserValidator : AbstractValidator<UserRequestLogin>
{
    public LoginUserValidator()
    {
    RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.")
        .MinimumLength(10).WithMessage("Email must be at least 10 characters long.")
        .EmailAddress().WithMessage("Invalid Email");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required.")
        .MinimumLength(8).WithMessage("Password must be at least 8 characters long.");
    }
}
