using FluentValidation;

public class PostValidator : AbstractValidator<PostDto>
{
    public PostValidator()
    {
        RuleFor(p => p.Title).NotEmpty().MaximumLength(100);
        RuleFor(p => p.Content).NotEmpty();
    }
}
