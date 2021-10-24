using DigitalWallet.Domain.Base;
using DigitalWallet.Domain.Events;
using DigitalWallet.Domain.ValueObjects;
using System;

namespace DigitalWallet.Domain.Entities
{
    public class Wallet : Entity
    {
        public UserId Owner { get; private set; }

        public Money Credit { get; private set; }

        private Wallet() { }

        public Wallet(UserId owner)
        {
            Causes(new WalletCreated
            {
                Id = Guid.NewGuid(),
                Owner = owner
            });
        }

        public void Deposit(Money money)
        {
            if (money <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(money));
            }

            Causes(new CreditAdded
            {
                Id = Id,
                Value = money
            });
        }

        public void Withdraw(Money money)
        {
            if (money <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(money));
            }

            if (money > Credit)
            {
                throw new InvalidOperationException("Wallet has not enough credit.");
            }

            Causes(new CreditRemoved
            {
                Id = Id,
                Value = money
            });
        }

        private void Causes(IDomainEvent @event)
        {
            _changes.Add(@event);
            Apply(@event);
        }

        protected override void Apply(IDomainEvent @event)
        {
            When((dynamic)@event);
            Version++;
        }

        private void When(CreditAdded @event)
        {
            Credit = Credit.Add(@event.Value);
        }

        private void When(CreditRemoved @event)
        {
            Credit = Credit.Remove(@event.Value);
        }

        private void When(WalletCreated @event)
        {
            Id = @event.Id;
            Owner = new UserId(@event.Owner);
            Credit = new Money(0);
        }
    }
}
