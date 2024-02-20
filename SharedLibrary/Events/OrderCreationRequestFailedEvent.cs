using SharedLibrary.Abstracts;

namespace SharedLibrary.Events
{
    public class OrderCreationRequestFailedEvent : IOrderCreationRequestFailedEvent
    {
        public int OrderId { get; set; }
        public string Message {get; set;}
    }
}
