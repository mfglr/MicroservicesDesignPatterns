namespace SharedLibrary.Abstracts
{
    public interface IOrderCreationRequestFailedEvent
    {
        int OrderId { get; }
        string Message { get; }
    }
}
