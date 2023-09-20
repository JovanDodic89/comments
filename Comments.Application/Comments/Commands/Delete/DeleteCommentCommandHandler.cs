using Comments.Application.Exceptions;
using Comments.Domain.Entities;
using Comments.Domain.Interfaces;
using MediatR;

namespace Comments.Application.Comments.Commands.Delete
{
    public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand, bool>
    {
        private readonly ICommentsRepository _commentsRepository;

        public DeleteCommentCommandHandler(
            ICommentsRepository commentsRepository
        )
        {
            _commentsRepository = commentsRepository;
        }

        public async Task<bool> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            var comment = await _commentsRepository.GetComment(request.Id.ToString(), 0);

            if (comment == null)
            {
                throw new NotFoundException(nameof(Comment), request.Id);
            }

            if (request.UserId != comment.UserId)
            {
                throw new UserNotValidException(request.UserId, "Delete");
            }

            await _commentsRepository.DeleteComment(request.Id.ToString());

            return true;
        }

    }
}
