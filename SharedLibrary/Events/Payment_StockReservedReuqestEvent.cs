using MassTransit;
using SharedLibrary.Abstracts;

namespace SharedLibrary.Events
{
    public class Payment_StockReservedReuqestEvent : IPayment_StockReservedReuqestEvent, CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
        public PaymentMessage Payment {  get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
