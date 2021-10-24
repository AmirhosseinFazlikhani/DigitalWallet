using DigitalWallet.Domain.Base;
using System;
using System.Collections.Generic;

namespace DigitalWallet.Domain.ValueObjects
{
    public class Money : ValueObject
    {
        public decimal Value { get; }

        public Money(decimal value)
        {
            Value = value;
        }

        public Money Add(decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            return new(Value + value);
        }

        public Money Remove(decimal value)
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(value));
            }

            return new(Value - value);
        }

        public static implicit operator decimal(Money money)
        {
            return money.Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
