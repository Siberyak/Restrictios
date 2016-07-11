using System;
using System.ComponentModel;

namespace Restrictions
{
    public partial class Environment<T>
    {

        public class Restriction : IRestriction, IRestrictionsInterval
        {
            public IRestrictionsInterval Owner { get; }
            public Environment<T> Environment { get; }
            private static readonly Restriction EmptyLeft = new Restriction(null, default(T), Direction.Left, true);
            private static readonly Restriction EmptyRight = new Restriction(null, default(T), Direction.Right, true);

            public bool IsEmpty => ReferenceEquals(this, EmptyLeft) || ReferenceEquals(this, EmptyRight);

            internal Restriction(IRestrictionsInterval owner, T value, Direction direction, bool includeValue)
            {
                Owner = owner;
                Value = value;
                Direction = direction;
                IncludeValue = includeValue;
            }
            public static Restriction Empty(Direction direction)
            {
                return direction == Direction.Left ? EmptyLeft : EmptyRight;
            }

            public virtual bool IncludeValue { get; protected set; } = true;
            public virtual Direction Direction { get; protected set; }
            public virtual T Value { get; protected set; }

            public int CompareTo(IRestriction other)
            {
                return NormalCompareTo(other);
            }

            private int NormalCompareTo(IRestriction other)
            {
                other = other ?? Empty(Direction);

                if (ReferenceEquals(other, this))
                    return 0;

                if (other.Direction != Direction)
                    return Direction == Direction.Left ? -1 : 1;

                if (other.IsEmpty)
                    return Direction == Direction.Left ? -1 : 1;
                if (IsEmpty)
                    return Direction == Direction.Left ? 1 : -1;


                var result = Value.CompareTo(other.Value) * -1;
                return result == 0 && IncludeValue != other.IncludeValue ? Direction == Direction.Left ? IncludeValue ? -1 : 1 : IncludeValue ? 1 : -1 : result;
            }

            public override string ToString()
            {
                return ReferenceEquals(this, Empty(Direction)) ? $"{Direction} infinity".ToLower() : $"{(Direction == Direction.Left ? ">" : "<")}{(IncludeValue ? "=" : "")} {Value}";
            }

            IRestriction IRestrictionsInterval.Left => Direction == Direction.Left ? this : EmptyLeft;

            IRestriction IRestrictionsInterval.Right => Direction == Direction.Right ? this : EmptyRight;

            void IRestrictionsInterval.Restrict(T value, Direction direction, bool includeValue = true)
            {
                throw new NotImplementedException();
            }

            IRestrictionsInterval IRestrictionsInterval.Restrict()
            {
                throw new NotImplementedException();
            }

            public void AddRestriction(IRestrictionsInterval restriction)
            {
                throw new NotImplementedException();
            }

            public void RemoveRestriction(IRestrictionsInterval restriction)
            {
                throw new NotImplementedException();
            }

            public void Clear(Direction? direction = default(Direction?))
            {
                throw new NotImplementedException();
            }

            public void Offsets(T value, out T leftOffset, out T rightOffset)
            {
                throw new NotImplementedException();
            }
            public bool Contains(T value)
            {
                throw new NotImplementedException();
            }

            private event PropertyChangedEventHandler PropertyChanged;

            event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
            {
                add { PropertyChanged += value; }
                remove { PropertyChanged -= value; }
            }

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}