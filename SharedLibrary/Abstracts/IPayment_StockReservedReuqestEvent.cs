using MassTransit;

namespace SharedLibrary.Abstracts
{
    public interface IPayment_StockReservedReuqestEvent : CorrelatedBy<Guid>
    {
        PaymentMessage Payment { get; }
        List<OrderItemMessage> OrderItems { get; }
    }
}
