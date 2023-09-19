using Comments.Application.Exceptions;
using Comments.Domain.Entities;
using Comments.Domain.Interfaces;
using MediatR;


namespace Comments.Application.Comments.Commands.Add
{
    public class AddCommentCommandHandler : IRequestHandler<AddCommentCommand, Comment>
    {
        private readonly ICommentsRepository _commentsRepository;

        public AddCommentCommandHandler(ICommentsRepository commentsRepository)
        {
            _commentsRepository = commentsRepository;
        }

        public async Task<Comment> Handle(AddCommentCommand request, CancellationToken cancellationToken)
        {         
            var commentRequest = new Comment
            {
                Context = request.Context,
                Text = request.Text,
                UserId = request.UserId,
                UserDisplayName = request.TokenUserDisplayName,
                CreatedOn = DateTime.UtcNow,
                LastModified = DateTime.UtcNow,
                ParentId = request.ParentId
            };

            if (!string.IsNullOrWhiteSpace(request.ParentId))
            {
                var requestParent = await _commentsRepository.GetComment(request.ParentId, 0);
                
                if (requestParent == null)
                {
                    throw new NotFoundException(nameof(Comment), request.ParentId);
                }
            }

            var comment = await _commentsRepository.AddComment(commentRequest);
            
            return comment;
        }
    }
}
