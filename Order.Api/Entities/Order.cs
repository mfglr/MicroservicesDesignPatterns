namespace Order.Api.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int BuyerId  { get; set; }
        public List<OrderItem> Items { get; set; }
        public OrderState State { get; set; }
        public Address Address { get; set; }
    }
}
