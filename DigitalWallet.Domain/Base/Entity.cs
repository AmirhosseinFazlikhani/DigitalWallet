using System;
using System.Collections.Generic;

namespace DigitalWallet.Domain.Base
{
    public abstract class Entity : IEquatable<Entity>
    {
        public Guid Id { get; protected set; }

        public int Version { get; protected set; }

        protected List<IDomainEvent> _changes = new List<IDomainEvent>();

        public IReadOnlyCollection<IDomainEvent> Changes => _changes;

        protected abstract void Apply(IDomainEvent @event);

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals(obj as Entity);
        }

        public bool Equals(Entity other)
        {
            return other.Id.Equals(Id);
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }
    }
}
