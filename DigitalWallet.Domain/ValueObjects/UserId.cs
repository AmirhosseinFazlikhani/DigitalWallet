using DigitalWallet.Domain.Base;
using System.Collections.Generic;

namespace DigitalWallet.Domain.ValueObjects
{
    public class UserId : ValueObject
    {
        public string Value { get; }

        public UserId(string value)
        {
            Value = value;
        }

        public static implicit operator string(UserId userId)
        {
            return userId.Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
