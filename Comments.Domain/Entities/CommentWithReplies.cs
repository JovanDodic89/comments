namespace Comments.Domain.Entities
{
    public class CommentWithReplies : Comment
    {
        public List<CommentWithReplies> Replies { get; set; }
    }
}
