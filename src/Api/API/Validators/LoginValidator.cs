using API.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

public class LoginValidator : AbstractValidator<LoginDto>
{
    public LoginValidator()
    {
        RuleFor(user => user.UserName).NotEmpty().WithMessage("Username is required");
        RuleFor(user => user.Password).NotEmpty().WithMessage("Password is required");
    }
}
