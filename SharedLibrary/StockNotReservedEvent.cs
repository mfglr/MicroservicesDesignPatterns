namespace SharedLibrary
{
    public class StockNotReservedEvent
    {
        public int OrderId { get; set; }
        public int BuyerId { get; set; }
        public string Message { get; set; }
    }
}
