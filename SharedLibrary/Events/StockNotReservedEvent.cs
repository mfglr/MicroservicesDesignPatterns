using SharedLibrary.Abstracts;

namespace SharedLibrary.Events
{
    public class StockNotReservedEvent : IStockNotReservedEvent
    {
        public Guid CorrelationId { get; set; }
        public string Message { get; set; }
    }
}
