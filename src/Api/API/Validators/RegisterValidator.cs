using API.Models;
using FluentValidation;

public class RegisterValidator : AbstractValidator<RegisterRequest>
{
    public RegisterValidator()
    {
        RuleFor(user => user.Username).NotEmpty().WithMessage("Username is required");
        RuleFor(user => user.Password).NotEmpty().WithMessage("Password is required");
        RuleFor(user => user.Email).NotEmpty().WithMessage("Email is required").EmailAddress().WithMessage("Invalid email address");
    }
}
