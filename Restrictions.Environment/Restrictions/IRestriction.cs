using System;
using System.Collections.Generic;

namespace Restrictions
{
    public partial class Environment<T>
    {
        public interface IRestriction : IEventedValue
        {
            IRestrictionOwner Owner { get; }
            IRestrictionSubject Subject { get; }

            T Offset(T value);

            Direction Direction { get; }
            bool IncludeValue { get; }
        }

        public interface IRestrictionOwner : IEventedValue
        {
            IEnumerable<IRestriction> OwnedRestrictions { get; }

            IRestriction Restrict(IRestrictionSubject subject, T realtiveValue, Direction direction, bool included = true);

        }

        public interface IRestrictionSubject : IEventedValue
        {
            IEnumerable<IRestriction> SubjectForRestrictions { get; } 
        }
    }
}