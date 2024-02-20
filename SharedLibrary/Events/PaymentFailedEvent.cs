using SharedLibrary.Abstracts;

namespace SharedLibrary.Events
{
    public class PaymentFailedEvent : IPaymentFailedEvent
    {
        public List<OrderItemMessage> OrderItems { get; set; }
        public Guid CorrelationId { get; set; }
        public string Message { get; set; }
    }
}
