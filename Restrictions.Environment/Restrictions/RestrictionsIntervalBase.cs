using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Restrictions
{
    public partial class Environment<T>
    {

        public abstract class RestrictionsIntervalBase : IRestrictionsInterval
        {
            public IRestrictionsInterval Owner { get; }
            public Environment<T> Environment { get; }

            protected readonly BindingList<IRestrictionsInterval> Restrictions = new BindingList<IRestrictionsInterval>();

            protected RestrictionsIntervalBase(Environment<T> environment)
            {
                Environment = environment;
                Restrictions.ListChanged += RestrictionsListChanged;
            }
            protected RestrictionsIntervalBase(IRestrictionsInterval owner) : this(owner.Environment)
            {
                Owner = owner;
            }

            private void RestrictionsListChanged(object sender, ListChangedEventArgs e)
            {
                Reset(Direction.Left, Direction.Right);
            }

            public void Restrict(T value, Direction direction, bool includeValue = true)
            {
                var restriction = new Restriction(this, value, direction, includeValue);
                Add(restriction, direction);
            }

            public IRestrictionsInterval Restrict()
            {
                var interval = new RestrictionsInterval(Environment);
                Add(interval, Direction.Left, Direction.Right);
                return interval;
            }

            public void Clear(Direction? direction = default(Direction?))
            {

                IEnumerable<IRestriction> enumerable = Restrictions.OfType<IRestriction>();
                if (direction.HasValue)
                    enumerable = enumerable.Where(x => x.Direction == direction);

                var remove = enumerable.OfType<IRestrictionsInterval>().ToArray();
                foreach (var restriction in remove)
                {
                    Restrictions.Remove(restriction);
                }

                switch (direction)
                {
                    case Direction.Left:
                        Reset(Direction.Left);
                        break;
                    case Direction.Right:
                        Reset(Direction.Right);
                        break;
                    default:
                        Reset(Direction.Left, Direction.Right);
                        break;
                }

            }

            public void Offsets(T value, out T leftOffset, out T rightOffset)
            {
                var left = Left.Value;
                if (!Left.IncludeValue)
                    left = Environment.AddFunc(left, Environment.Epsilon);

                var right = Right.Value;
                if (!Right.IncludeValue)
                    right = Environment.SubstractFunc(right, Environment.Epsilon);

                leftOffset = Environment.SubstractFunc(value, left);
                rightOffset = Environment.SubstractFunc(right, value);
            }

            public bool Contains(T value)
            {
                T leftOffset;
                T rightOffset;
                Offsets(value, out leftOffset, out rightOffset);


                return leftOffset.CompareTo(Zerro) >= 0 && rightOffset.CompareTo(Zerro) >= 0;
            }

            public void AddRestriction(IRestrictionsInterval restriction)
            {
                Restrictions.Add(restriction);
                if(!restriction.Left.IsEmpty && !restriction.Right.IsEmpty)
                    Reset(Direction.Left,Direction.Right);
                else if (!restriction.Left.IsEmpty)
                    Reset(Direction.Left);
                else if (!restriction.Right.IsEmpty)
                    Reset(Direction.Right);
            }

            public void RemoveRestriction(IRestrictionsInterval restriction)
            {
                if(!Restrictions.Remove(restriction))
                    return;

                if (!restriction.Left.IsEmpty && !restriction.Right.IsEmpty)
                    Reset(Direction.Left, Direction.Right);
                else if (!restriction.Left.IsEmpty)
                    Reset(Direction.Left);
                else if (!restriction.Right.IsEmpty)
                    Reset(Direction.Right);
            }

            private void Add(IRestrictionsInterval restriction, params Direction[] directions)
            {
                if (restriction == null)
                    return;

                Restrictions.Add(restriction);
                Reset(directions);
            }

            private void Reset(params Direction[] directions)
            {
                foreach (var direction in directions)
                {
                    ResetDirection(direction);
                }

                OnPropertyChanged("#");
            }

            private IRestriction _left;
            private IRestriction _right;

            public IRestriction Left => _left ?? (_left = Restrictions.Select(x => x.Left).OrderBy(x => x).FirstOrDefault() ?? Restriction.Empty(Direction.Left));
            public IRestriction Right => _right ?? (_right = Restrictions.Select(x => x.Right).OrderBy(x => x).LastOrDefault() ?? Restriction.Empty(Direction.Right));

            private void ResetDirection(Direction direction)
            {
                switch (direction)
                {
                    case Direction.Left:
                        _left = null;
                        break;
                    case Direction.Right:
                        _right = null;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                }
            }

            private bool IsPosible => Left.IsEmpty || Right.IsEmpty || Left.Value.CompareTo(Right.Value) <= 0;
            public override string ToString()
            {
                return $"{(IsPosible ? "" : "IMPOSIBLE ")}({Left};{Right})";
            }

            public event PropertyChangedEventHandler PropertyChanged;

            protected virtual void OnPropertyChanged(string propertyName)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}