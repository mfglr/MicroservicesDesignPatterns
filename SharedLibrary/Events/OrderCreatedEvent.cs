using SharedLibrary.Abstracts;

namespace SharedLibrary.Events
{
    public class OrderCreatedEvent : IOrderCreatedEvent
    {
        public Guid CorrelationId { get; set; }

        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
