namespace Comments.Persistance.Entities
{
    public class CommentEntity
    {
        public long Id { get; set; }
        public string ContextIdentifier { get; set; }
        public string UserIdentifier { get; set; }
        public string UserDisplayName { get; set; }
        public DateTime CommentTime { get; set; }
        public string CommentText { get; set; }
        public long? ParentId { get; set; }
        public CommentEntity Parent { get; set; }
        public List<CommentEntity> Children { get; set; }
        public DateTime? LastModified { get; set; }
        public bool Deleted { get; set; }
    }
}
