using Comments.Domain.Entities;
using MediatR;

namespace Comments.Application.Comments.Commands.Update
{
    public class UpdateCommentCommand: IRequest<Comment>
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string UserId { get; set; }
    }
}
