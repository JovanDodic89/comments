
using Comments.Application.Exceptions;
using Comments.Domain.Entities;
using Comments.Domain.Interfaces;
using MediatR;


namespace Comments.Application.Comments.Queries.Get
{
    public class GetCommentQueryHandler : IRequestHandler<GetCommentQuery, CommentWithReplies>
    {
        private readonly ICommentsRepository _commentsRepository;

        public GetCommentQueryHandler(ICommentsRepository commentsRepository)
        {
            _commentsRepository = commentsRepository;
        }

        public async Task<CommentWithReplies> Handle(GetCommentQuery request, CancellationToken cancellationToken)
        {
            CommentWithReplies comment = await _commentsRepository.GetComment(request.Id.ToString(), request.Depth);
            
            if (comment == null)
            {
                throw new NotFoundException(nameof(comment), request.Id);
            }

            return comment;
        }
    }
}
