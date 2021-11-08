using DigitalWallet.Domain.Base;
using DigitalWallet.Domain.Entities;
using DigitalWallet.Domain.Repositories;
using DigitalWallet.Infrastructure.Internals;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace DigitalWallet.Infrastructure.Repositories
{
    public class Repository : IRepository
    {
        private readonly IEventSource _eventSource;

        public Repository(IEventSource eventSource)
        {
            _eventSource = eventSource;
        }

        public async Task AddAsync(Wallet wallet)
        {
            await _eventSource.CreateStreamAsync(GetStreamName(wallet.Id), wallet.Changes);
            ClearChanges(wallet);
        }

        public async Task<Wallet> FindAsync(Guid id)
        {
            var applyMethod = typeof(Wallet).GetMethod("Apply", BindingFlags.NonPublic | BindingFlags.Instance);

            var consructor = typeof(Wallet).GetConstructor(
                BindingFlags.Instance | BindingFlags.NonPublic, null,
                new Type[] { },
                null);

            var wallet = consructor.Invoke(new object[] { }) as Wallet;

            var stream = _eventSource.ReadStreamAsync(GetStreamName(id));

            while (await stream.MoveNextAsync())
            {
                applyMethod.Invoke(wallet, new object[] { stream.Current });
            }

            return wallet;
        }

        public async Task SaveChangesAsync(Wallet wallet)
        {
            await _eventSource.AppendToStreamAsync(GetStreamName(wallet.Id), wallet.Changes);
            ClearChanges(wallet);
        }

        private string GetStreamName(Guid id)
        {
            return $"{typeof(Wallet)} - {id}";
        }

        private void ClearChanges(Wallet wallet)
        {
            wallet.MarkChangesAsCommitted();
        }
    }
}
