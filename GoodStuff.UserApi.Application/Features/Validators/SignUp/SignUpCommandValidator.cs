using FluentValidation;
using GoodStuff.UserApi.Application.Features.Commands.SignUp;

namespace GoodStuff.UserApi.Application.Features.Validators.SignUp;

public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("You have to provide your Name")
            .Matches(@"^[a-zA-Z]{3,}$").WithMessage("Name can't contains numbers and special characters.");

        RuleFor(x => x.Surname)
            .NotEmpty().WithMessage("You have to provide your Surname")
            .Matches(@"^[a-zA-Z]{3,}$").WithMessage("Name can't contains numbers and special characters.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("You have to provide your Email")
            .EmailAddress().WithMessage("Invalid Email format");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("You have to provide your Password")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$")
            .WithMessage("Password must contains one: upercase, numer, special character and be at least 8 long.");
    }
}