using Comments.Domain.Entities;
using MediatR;

namespace Comments.Application.Comments.Commands.Add
{
    public class AddCommentCommand : IRequest<Comment>
    {
        public string Context { get; set; }
        public string Text { get; set; }
        public string ParentId { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public string TokenUserId { get; set; }
        public string TokenUserDisplayName { get; set; }
    }
}
