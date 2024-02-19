namespace SharedLibrary.Abstracts
{
    public interface IOrderCreatedRequestEvent
    {
        int OrderId { get; }
        int BuyerId { get; }
        PaymentMessage Payment { get; }
        List<OrderItemMessage> OrderItems { get; }
    }
}
