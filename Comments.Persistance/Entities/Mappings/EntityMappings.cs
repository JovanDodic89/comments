using Comments.Domain.Entities;

namespace Comments.Persistance.Entities.Mappings
{
    public static class EntityMappings
    {
        public static CommentWithReplies ToWithRepliesModel(this CommentEntity comment)
        {
            return new CommentWithReplies
            {
                Context = comment.ContextIdentifier,
                CreatedOn = comment.CommentTime,
                LastModified = comment.LastModified,
                Id = comment.Id.ToString(),
                ParentId = comment.ParentId.ToString(),
                UserId = comment.UserIdentifier,
                UserDisplayName = comment.UserDisplayName ?? null,
                Text = comment.CommentText
            };
        }
        public static Comment ToModel(this CommentEntity comment)
        {
            return new Comment
            {
                Context = comment.ContextIdentifier,
                CreatedOn = comment.CommentTime,
                LastModified = comment.LastModified,
                Id = comment.Id.ToString(),
                ParentId = comment.ParentId.ToString(),
                UserId = comment.UserIdentifier,
                UserDisplayName = comment.UserDisplayName ?? null,
                Text = comment.CommentText
            };
        }
        public static CommentEntity ToEntity(this Comment comment)
        {
            return new CommentEntity
            {
                CommentText = comment.Text,
                CommentTime = comment.CreatedOn,
                LastModified = comment.LastModified,
                Deleted = false,
                ContextIdentifier = comment.Context,
                UserIdentifier = comment.UserId,
                UserDisplayName = comment.UserDisplayName ?? null,
                ParentId = (comment.ParentId != null) ? long.Parse(comment.ParentId) : null as long?
            };
        }
    }
}
