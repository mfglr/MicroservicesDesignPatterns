using SharedLibrary.Abstracts;

namespace SharedLibrary.Events
{
    public class OrderCreationRequestCompletedEvent : IOrderCreationRequestCompletedEvent
    {
        public int OrderId { get; set; }
    }
}
