using DigitalWallet.Domain.Base;
using EventStore.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DigitalWallet.Infrastructure.Internals
{
    internal class EventSource : IEventSource
    {
        private readonly EventStoreClient _eventStore;

        public EventSource(EventStoreClient eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task AppendToStreamAsync(string streamName, IEnumerable<IDomainEvent> events)
        {
            var data = Serialize(events);
            await _eventStore.AppendToStreamAsync(streamName, StreamState.StreamExists, data);
        }

        public async Task CreateStreamAsync(string name, IEnumerable<IDomainEvent> events)
        {
            var data = Serialize(events);
            await _eventStore.AppendToStreamAsync(name, StreamState.NoStream, data);
        }

        public async IAsyncEnumerator<IDomainEvent> ReadStreamAsync(
            string streamName,
            int limit = int.MaxValue,
            bool backward = false)
        {
            var direction = backward ? Direction.Backwards : Direction.Forwards;
            var position = backward ? StreamPosition.End : StreamPosition.Start;

            var result = _eventStore.ReadStreamAsync(direction, streamName, position, limit);

            if (await result.ReadState == ReadState.StreamNotFound)
            {
                yield break;
            }
            
            var enumerator = result.GetAsyncEnumerator();

            while (await enumerator.MoveNextAsync())
            {
                yield return Deserialize(enumerator.Current);
            }
        }

        private IEnumerable<EventData> Serialize(IEnumerable<IDomainEvent> events)
        {
            foreach (var @event in events)
            {
                var eventId = Uuid.NewUuid();
                var eventName = @event.GetType().Name;
                var eventBytes = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType());
                var metadata = new EventMetadata { ClrType = @event.GetType().AssemblyQualifiedName };
                var metadataBytes = JsonSerializer.SerializeToUtf8Bytes(metadata);

                yield return new EventData(eventId, eventName, eventBytes, metadataBytes);
            }
        }

        private IDomainEvent Deserialize(ResolvedEvent @event)
        {
            var metadata = JsonSerializer.Deserialize<EventMetadata>(@event.Event.Metadata.Span);
            var eventType = Type.GetType(metadata.ClrType);
            var eventBody = JsonSerializer.Deserialize(@event.Event.Data.Span, eventType);

            return eventBody as IDomainEvent;
        }

        private class EventMetadata
        {
            public string ClrType { get; set; }
        }
    }
}
