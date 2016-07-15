using System;
using System.Collections.Generic;
using System.Linq;

namespace Restrictions
{
    public static class Extender
    {
        public static T? Offset<T>(this IEnumerable<Environment<T>.IRestriction> restrictions, Direction direction, T value)
            where T:struct, IComparable<T>, IEquatable<T>
        {
            var enumerable = restrictions.Where(x => x.Direction == direction).OrderBy(x => x.Value);
            var r = direction == Direction.Left ? enumerable.FirstOrDefault() : enumerable.LastOrDefault();
            var offset = r?.Offset(value);
            return offset;
        }
    }
}