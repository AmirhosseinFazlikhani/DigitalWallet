using DigitalWallet.Domain.Base;
using System;

namespace DigitalWallet.Domain.Events
{
    public class CreditAdded : IDomainEvent
    {
        public Guid Id { get; set; }

        public decimal Value { get; set; }
    }
}
