using MassTransit;

namespace SharedLibrary.Abstracts
{
    public interface IPaymentCompletedEvent : CorrelatedBy<Guid>
    {

    }
}
