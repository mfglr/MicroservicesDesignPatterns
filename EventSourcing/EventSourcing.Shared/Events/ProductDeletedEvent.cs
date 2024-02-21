namespace EventSourcing.Shared.Events
{
    public class ProductDeletedEvent : IProductEvent
    {
        public Guid Id { get; set; }
    }
}
