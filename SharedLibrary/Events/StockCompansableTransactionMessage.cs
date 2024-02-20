using SharedLibrary.Abstracts;

namespace SharedLibrary.Events
{
    public class StockCompansableTransactionMessage : IStockCompansableTransactionMessage
    {
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
