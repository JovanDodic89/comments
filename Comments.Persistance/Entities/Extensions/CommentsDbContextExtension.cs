using Microsoft.EntityFrameworkCore;

namespace Comments.Persistance.Entities.Extensions
{
    internal static class CommentsDbContextExtension
    {
        public static async Task<CommentEntity> TryGetComment(this DbSet<CommentEntity> comments, long commentId, bool includeParent = false)
        {
            CommentEntity comment = includeParent? await comments.Include(inc => inc.Parent)
                .FirstOrDefaultAsync(cm => cm.Id == commentId) : await comments.FindAsync(commentId);

            if(comment == null)
            {
                throw new Exception("Could not be found");
            }

            return comment;
        }
    }
}
