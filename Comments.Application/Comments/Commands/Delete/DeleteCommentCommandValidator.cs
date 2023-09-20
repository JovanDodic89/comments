using FluentValidation;

namespace Comments.Application.Comments.Commands.Delete
{
    public class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
    {
        public DeleteCommentCommandValidator()
        {
            RuleFor(e => e.Id)
                .NotNull();

            RuleFor(e => e.UserId)
                .NotEmpty();

            RuleFor(e => e.Id)
                .Must((x) => long.TryParse(x, out var value) && value >= 0)
                .WithMessage("'Id' must be a valid number and less than 0.");
        }
    }
}
