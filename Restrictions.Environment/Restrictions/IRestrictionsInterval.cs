using System;
using System.ComponentModel;

namespace Restrictions
{
    public partial class Environment<T>
    {

        public interface IRestrictionsInterval : INotifyPropertyChanged
        {
            Environment<T> Environment { get; }
            IRestrictionsInterval Owner { get; }
            IRestriction Left { get; }
            IRestriction Right { get; }
            void Restrict(T value, Direction direction, bool includeValue = true);
            IRestrictionsInterval Restrict();

            void AddRestriction(IRestrictionsInterval restriction);
            void RemoveRestriction(IRestrictionsInterval restriction);


            void Clear(Direction? direction = default(Direction?));


            void Offsets(T value, out T leftOffset, out T rightOffset);
            bool Contains(T value);
        }
    }
}