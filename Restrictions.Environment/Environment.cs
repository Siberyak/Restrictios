using System;

namespace Restrictions
{
    public partial class Environment<T>
        where T : IComparable<T>, IEquatable<T>
    {
        public Func<T, T, T> AddFunc;
        public Func<T, T, T> SubstractFunc;
        public T Epsilon;
        public static readonly T Zerro = default(T);

        public IRestrictionsInterval Restrict()
        {
            return new RestrictionsInterval(this);
        }
    }
}