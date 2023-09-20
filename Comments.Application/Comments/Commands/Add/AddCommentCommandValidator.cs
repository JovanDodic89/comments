using FluentValidation;

namespace Comments.Application.Comments.Commands.Add
{
    public class AddCommentCommandValidator : AbstractValidator<AddCommentCommand>
    {
        public AddCommentCommandValidator()
        {
            RuleFor(e => e.Context)
                .NotEmpty()
                .WithErrorCode("required");

            RuleFor(e => e.Context)
                .MaximumLength(1024);

            RuleFor(e => e.Text)
                .NotEmpty()
                .WithErrorCode("required");

            RuleFor(e => e.ParentId)
                .Must(e => int.TryParse(e, out _))
                .When(e => !string.IsNullOrWhiteSpace(e.ParentId))
                .WithMessage("parent-id must be valid number")
                .WithErrorCode("invalid-format");
        }
    }
}
