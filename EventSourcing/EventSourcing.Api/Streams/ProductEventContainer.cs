using EventSourcing.Shared.Events;
using EventStore.ClientAPI;

namespace EventSourcing.Api.Streams
{
    public class ProductEventContainer : EventContainer<IProductEvent>
    {
        public ProductEventContainer(string streamName, IEventStoreConnection client) : base(streamName,client)
        {
        }
    }
}
