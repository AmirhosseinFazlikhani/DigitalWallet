using DigitalWallet.Domain.Base;
using DigitalWallet.Domain.Events;
using DigitalWallet.Infrastructure.Internals;
using DigitalWallet.Infrastructure.Repositories;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace DigitalWallet.Infrastructure.UnitTests
{
    public class RepositoryTest
    {
        [Fact]
        public void FindAsync()
        {
            var walletId = Guid.NewGuid();
            var stream = new List<IDomainEvent>
            {
                new WalletCreated
                {
                    Id = walletId,
                    Owner = Guid.NewGuid().ToString()
                },
                new CreditAdded
                {
                    Id = walletId,
                    Value = 100
                },
                new CreditRemoved
                {
                    Id = walletId,
                    Value = 20
                }
            };

            var eventSource = new Mock<IEventSource>();
            eventSource.Setup(e => e.ReadStreamAsync(It.IsAny<string>(), int.MaxValue, false))
                .Returns(stream.ToAsyncEnumerable().GetAsyncEnumerator());

            var repository = new Repository(eventSource.Object);
            var wallet = repository.FindAsync(walletId).Result;

            Assert.Equal(walletId, wallet.Id);
            Assert.Equal(80, wallet.Credit.Value);
        }
    }
}
