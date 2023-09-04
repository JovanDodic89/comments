using Comments.Domain.Entities;
using System.Threading;

namespace Comments.Domain.Interfaces
{
    public interface ICommentsRepository
    {
        public Task<Comment> AddComment(Comment comment);
        public Task<Comment> UpdateComment(string id, string text);
        public Task<Comment> GetComment(string context);
        public Task<int> GetCommentDepth(Comment comment);
        public Task<bool> DeleteComment(string id);
    }
}
