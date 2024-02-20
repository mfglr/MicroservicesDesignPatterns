using MassTransit;

namespace SharedLibrary.Abstracts
{
    public interface IPaymentFailedEvent : CorrelatedBy<Guid>
    {
        string Message { get; }
        List<OrderItemMessage> OrderItems { get; }
    }
}
