using SharedLibrary.Abstracts;

namespace SharedLibrary.Events
{
    public class PaymentCompletedEvent : IPaymentCompletedEvent
    {
        public Guid CorrelationId { get; set; }
    }
}
