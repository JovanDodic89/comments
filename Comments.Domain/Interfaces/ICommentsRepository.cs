using Comments.Domain.Entities;
using System.Threading;

namespace Comments.Domain.Interfaces
{
    public interface ICommentsRepository
    {
        public Task<Comment> AddComment(Comment comment);
        public Task<Comment> UpdateComment(string id, string text);
        public Task<int> GetCommentDepth(Comment comment);
        public Task<SortedPagedList<CommentWithReplies>> GetComments(string q, string context, int depth, string userId, int page, int pageSize, string sortBy, string sortOrder);
        public Task<CommentWithReplies> GetComment(string id, int depth);
        public Task<bool> DeleteComment(string id);
    }
}
