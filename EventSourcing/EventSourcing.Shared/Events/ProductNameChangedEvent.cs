namespace EventSourcing.Shared.Events
{
    public class ProductNameChangedEvent : IProductEvent
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
