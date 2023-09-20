using FluentValidation;


namespace Comments.Application.Comments.Queries.Get
{
    public class GetCommentQueryValidator : AbstractValidator<GetCommentQuery>
    {
        public GetCommentQueryValidator()
        {
            RuleFor(e => e.Id)
                .NotNull();

            RuleFor(e => e.Id)
                .Must((x) => long.TryParse(x, out var value) && value >= 0)
                .WithMessage("'Id' must be a valid number and less than 0.");
        }
    }
}
