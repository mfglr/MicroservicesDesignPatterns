namespace EventSourcing.Shared.Events
{
    public class ProductPriceChangedEvent : IProductEvent
    {
        public Guid Id { get; set; }
        public decimal Price { get; set; }
    }
}
