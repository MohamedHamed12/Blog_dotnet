using API.Models;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

public class RegisterValidator : AbstractValidator<RegisterDto>
{
    public RegisterValidator()
    {
        RuleFor(user => user.UserName).NotEmpty().WithMessage("Username is required");
        RuleFor(user => user.Password).NotEmpty().WithMessage("Password is required");
        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Invalid email address");

        // vaildate email should be unique
        // username should be unique
    }
}

// public class RegisterValidator : AbstractValidator<RegisterDto>
// {
//     private readonly UserManager<ApplicationUser> _userManager;
//
//     public RegisterValidator(UserManager<ApplicationUser> userManager)
//     {
//         _userManager = userManager;
//
//         RuleFor(user => user.UserName)
//             .NotEmpty()
//             .WithMessage("Username is required")
//             .MustAsync(BeUniqueUsername)
//             .WithMessage("Username already exists");
//
//         RuleFor(user => user.Password).NotEmpty().WithMessage("Password is required");
//
//         RuleFor(user => user.Email)
//             .NotEmpty()
//             .WithMessage("Email is required")
//             .EmailAddress()
//             .WithMessage("Invalid email address")
//             .MustAsync(BeUniqueEmail)
//             .WithMessage("Email already exists");
//     }
//
//     // Custom async method to check if the username is unique
//     private async Task<bool> BeUniqueUsername(string username, CancellationToken cancellationToken)
//     {
//         Console.WriteLine("*********debug");
//         var user = await _userManager.FindByNameAsync(username);
//         return user == null; // Return true if no user exists with the same username
//     }
//
//     // Custom async method to check if the email is unique
//     private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
//     {
//         var user = await _userManager.FindByEmailAsync(email);
//         return user == null; // Return true if no user exists with the same email
//     }
// }
