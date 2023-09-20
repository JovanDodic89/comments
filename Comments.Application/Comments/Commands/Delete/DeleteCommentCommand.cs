using MediatR;

namespace Comments.Application.Comments.Commands.Delete
{
    public class DeleteCommentCommand: IRequest<bool>
    {
        public string Id { get; set; }
        public string UserId { get; set; }
    }
}
