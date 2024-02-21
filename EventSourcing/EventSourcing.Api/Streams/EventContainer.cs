using EventSourcing.Shared.Events;
using EventStore.ClientAPI;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace EventSourcing.Api.Streams
{
    public abstract class EventContainer<TEvent> where TEvent : IEvent
    {
        private readonly IEventStoreConnection _connection;
        private readonly string _streamName;
        public EventContainer(string streamName, IEventStoreConnection connection)
        {
            _streamName = streamName;
            _connection = connection;
        }

        private readonly List<IEvent> _events = new();
        public void AddEvent(TEvent @event) => _events.Add(@event);
        public async Task SaveAsync()
        {
            var even = _events.First();


            var events = _events
                .Select(
                    x => new EventData(
                        Guid.NewGuid(),
                        x.GetType().Name,
                        true,
                        Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(x)),
                        Encoding.UTF8.GetBytes(x.GetType().FullName)
                    )
                )
                .ToList();

            await _connection.AppendToStreamAsync(
                _streamName,
                ExpectedVersion.Any,
                events
            );

            _events.Clear();
        }
    }
}
