namespace SharedLibrary
{
    public class OrderCreatedEvent
    {
        public int OrderId { get; set; }
        public int BuyerId { get; set; }
        public PaymentMessage Payment {  get; set; }
        public List<OrderItemMessage> OrderItems { get; set; }
    }
}
