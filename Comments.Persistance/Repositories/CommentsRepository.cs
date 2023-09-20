using Comments.Domain.Entities;
using Comments.Domain.Interfaces;
using Comments.Persistance.Entities;
using Comments.Persistance.Entities.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Comments.Persistance.Repositories
{
    public class CommentsRepository : ICommentsRepository
    {
        private readonly CommentsDbContext _commentsDbContext;

        public CommentsRepository(CommentsDbContext commentsDbContext)
        {
            _commentsDbContext = commentsDbContext;
        }

        public async Task<Comment> AddComment(Comment comment)
        {
            var commentEntity = comment.ToEntity();
            await _commentsDbContext.AddAsync(commentEntity);
            await _commentsDbContext.SaveChangesAsync();
            return commentEntity.ToModel();
        }

        public async Task<bool> DeleteComment(string id)
        {
            var comment = await _commentsDbContext.Comments.Where(x => x.Id.Equals(long.Parse(id)))
                    .FirstOrDefaultAsync();
            if (comment == null)
            {
                return false;
            }
            _commentsDbContext.Comments.Remove(comment);
            await RemoveChildrens(id);
            await _commentsDbContext.SaveChangesAsync();
            return true;
        }

        private async Task RemoveChildrens(string Id)
        {
            var children = await _commentsDbContext.Comments
                .Where(c => c.ParentId == long.Parse(Id))
                .ToListAsync();

            foreach (var child in children)
            {
                await RemoveChildrens(child.Id.ToString());
                _commentsDbContext.Remove(child);
            }
        }

        private async Task<CommentEntity> GetCommentEntity(string id, int depth)
        {
            var comment = await _commentsDbContext.Comments.Where(x => x.Id.Equals(long.Parse(id)))
                   .FirstOrDefaultAsync();
            return comment;
        }
        public async Task<CommentWithReplies> GetComment(string id, int depth)
        {
            var comment = await GetCommentEntity(id, depth);
            if (comment == null)
            {
                return null;
            }
            var commentModel = comment.ToWithRepliesModel();
            if (depth > 0)
            {
                GetReplies(commentModel, depth);
            }
            return commentModel;
        }
        private void GetReplies(CommentWithReplies commentWithReplies, int depth)
        {
            List<CommentWithReplies> rootCommentReplies = _commentsDbContext.Comments
                .Where(co => co.ParentId == long.Parse(commentWithReplies.Id))
                .Select(x => x.ToWithRepliesModel())
                .ToList();
            commentWithReplies.Replies = rootCommentReplies;
            commentWithReplies.ReplyCount = rootCommentReplies.Count;
            if (depth > 1)
            {
                foreach (CommentWithReplies reply in rootCommentReplies)
                {
                    GetReplies(reply, depth - 1);
                }
            }
        }
        public async Task<int> GetCommentDepth(Comment comment)
        {
            if (comment.ParentId == null)
            {
                return 0;
            }
            var depth = 1;
            var ret = await _commentsDbContext.Comments
                .Where(x => x.Id.Equals(comment.ParentId))
                .Select(x => x.Id)
                .FirstOrDefaultAsync();
            while (ret != 0)
            {
                ret = await _commentsDbContext.Comments
                    .Where(co => co.Id == ret)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();
                depth++;
            }
            return depth;
        }

        public async Task<SortedPagedList<CommentWithReplies>> GetComments(string q, string context, int depth, string userId, int page, int pageSize, string sortBy, string sortOrder)
        {
            IQueryable<CommentEntity> matches = _commentsDbContext.Comments.Where(co => co.Parent == null && co.ContextIdentifier == context.Trim());
      
            if (!string.IsNullOrEmpty(userId))
            {
                matches = matches.Where(c => c.UserIdentifier.Contains(userId));
            }
            else if (!string.IsNullOrEmpty(q))
            {
                matches = matches.Where(c => c.CommentText.Contains(q));
            }
            if (string.IsNullOrWhiteSpace(sortBy))
            {
                if (sortOrder == "asc")
                {
                    matches = matches.OrderBy(it => it.CommentTime);
                }
                else
                {
                    matches = matches.OrderByDescending(it => it.CommentTime);
                    sortOrder = "desc"; 
                }
                sortBy = "created-on";
            }
            else
            {
                sortBy = sortBy.Trim().ToLowerInvariant();
                System.Linq.Expressions.Expression<Func<CommentEntity, object>> sort = (it => it.CommentTime);
                sort = sortBy switch
                {
                    "contextIdentifier" => it => it.ContextIdentifier,
                    "context" => it => it.ContextIdentifier,
                    "userIdentifier" => it => it.UserIdentifier,
                    "userId" => it => it.UserIdentifier,
                    "commentText" => it => it.CommentText,
                    "text" => it => it.CommentText,
                    _ => it => it.CommentTime,
                };
                if (sortOrder == "asc")
                {
                    matches = matches.OrderBy(it => it.CommentTime);
                }
                else
                {
                    matches = matches.OrderByDescending(it => it.CommentTime);
                    sortOrder = "desc"; 
                }
            }

        
            int pgSize = pageSize;
            int total = matches.Count();
            int totalPages = (int)Math.Ceiling(total * 1.0 / pgSize);


            matches = matches.Skip((page - 1) * pgSize).Take(pgSize);

            var matchResult = await matches.Include(inc => inc.Parent)
                .ToListAsync();

            List<CommentWithReplies> commentWithReplies = new List<CommentWithReplies>();

            foreach (var comment in matchResult)
            {
                var ret = comment.ToWithRepliesModel();
                if (depth > 0)
                {
                    GetReplies(ret, depth);
                }
                commentWithReplies.Add(ret);
            }

            SortedPagedList<CommentWithReplies> result = new SortedPagedList<CommentWithReplies>
            {
                Items = commentWithReplies,
                Page = page,
                PageSize = pageSize,
                TotalCount = total,
                TotalPages = totalPages,
                SortBy = sortBy,
                SortOrder = sortOrder
            };

            return result;
        }

        public async Task<Comment> UpdateComment(string id, string text)
        {
            var comment = await GetCommentEntity(id, 0);
            comment.CommentText = text;
            comment.LastModified = DateTime.UtcNow;
            await _commentsDbContext.SaveChangesAsync();
            return comment.ToModel();
        }
    }
}
