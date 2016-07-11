using System;

namespace Restrictions
{
    public partial class Environment<T>
    {
        public interface IRestriction : IComparable<IRestriction>
        {
            bool IsEmpty { get; }
            Direction Direction { get; }
            T Value { get; }
            bool IncludeValue { get; }
        }
    }
}