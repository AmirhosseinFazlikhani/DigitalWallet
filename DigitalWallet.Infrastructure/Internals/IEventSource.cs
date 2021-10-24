using DigitalWallet.Domain.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigitalWallet.Infrastructure.Internals
{
    public interface IEventSource
    {
        Task CreateStreamAsync(string name, IEnumerable<IDomainEvent> events);

        Task AppendToStreamAsync(string streamName, IEnumerable<IDomainEvent> events);

        IAsyncEnumerator<IDomainEvent> ReadStreamAsync(string streamName, int limit = int.MaxValue, bool backward = false);
    }
}
