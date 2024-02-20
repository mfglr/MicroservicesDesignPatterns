using MassTransit;

namespace SharedLibrary.Abstracts
{
    public interface IStockNotReservedEvent : CorrelatedBy<Guid>
    {
        string Message { get; }
    }
}
