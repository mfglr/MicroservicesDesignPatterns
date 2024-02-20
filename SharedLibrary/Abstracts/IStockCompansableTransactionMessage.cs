namespace SharedLibrary.Abstracts
{
    public interface IStockCompansableTransactionMessage
    {
        List<OrderItemMessage> OrderItems { get; }
    }
}
