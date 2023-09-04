namespace Comments.Domain.Entities
{
    public class Comment
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Context { get; set; }
        public string UserId { get; set; }
        public string UserDisplayName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Text { get; set; }
        public DateTime? LastModified { get; set; }
        public int ReplyCount { get; set; }
    }
}
