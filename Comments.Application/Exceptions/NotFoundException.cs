namespace Comments.Application.Exceptions
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string entity, object identifier) : base($"Entity '{entity}' not found for identifier '{identifier}'"){ }
    }
}
