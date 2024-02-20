using MassTransit;

namespace SharedLibrary.Abstracts
{
    public interface IOrderCreatedEvent : CorrelatedBy<Guid>
    {
        List<OrderItemMessage> OrderItems { get; }
    }
}
