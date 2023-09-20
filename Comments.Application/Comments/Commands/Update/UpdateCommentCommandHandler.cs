using Comments.Application.Exceptions;
using Comments.Domain.Entities;
using Comments.Domain.Interfaces;
using MediatR;

namespace Comments.Application.Comments.Commands.Update
{
    public class UpdateCommentCommandHandler : IRequestHandler<UpdateCommentCommand, Comment>
    {
        private readonly ICommentsRepository _commentsRepository;

        public UpdateCommentCommandHandler(ICommentsRepository commentsRepository)
        {
            _commentsRepository = commentsRepository;
        }

        public async Task<Comment> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            Comment comment = await _commentsRepository.GetComment(request.Id.ToString(), 0);

            if (comment == null)
            {
                throw new NotFoundException(nameof(Comment), request.Id);
            }

            if (!comment.UserId.Equals(request.UserId))
            {
                throw new UserNotValidException(request.UserId, "Update");
            }

            var updatedComment = await _commentsRepository.UpdateComment(request.Id.ToString(), request.Text);
            
            return updatedComment;
        }
    }
}
