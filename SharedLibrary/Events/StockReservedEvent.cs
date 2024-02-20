using SharedLibrary.Abstracts;

namespace SharedLibrary.Events
{
    public class StockReservedEvent : IStockReservedEvent
    {
        public List<OrderItemMessage> OrderItems { get; set; }

        public Guid CorrelationId { get; set; }
    }
}
