using Comments.Domain.Entities;
using MediatR;

namespace Comments.Application.Comments.Queries.Get
{
    public class GetCommentQuery: IRequest<CommentWithReplies>
    {
        public int Depth { get; set; } = 1;
        public string Id { get; set; }
    }
}
