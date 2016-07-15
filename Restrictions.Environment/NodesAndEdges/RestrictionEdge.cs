using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using Restrictions.Graph;

namespace Restrictions
{
    public partial class Environment<T>
    {
        public class RestrictionEdge : EnvironmentEdge, IRestriction
        {

            Environment<T> IValue.Environment => Environment;

            internal RestrictionEdge(Environment<T> environment, AnchorNode @from, AnchorNode to) : base(environment, @from, to)
            {
            }

            public T RelativeValue
            {
                get { return _relativeValue; }
                set
                {
                    var offset = Environment.SubstractFunc(_relativeValue, value);
                    Move(offset);
                }
            }

            public Direction Direction { get; set; }
            public bool IncludeValue { get; set; }

            private T _relativeValue;

            //private T? _value;
            //public T Value => (_value ?? (_value = Environment.AddFunc(Owner.Value, RelativeValue))).Value;
            public T Value => Environment.AddFunc(Owner.Value, RelativeValue);

            public void Move(T offset)
            {
                var args = new ValueChangedEventArgs();
                try
                {
                    OnRelativeValueChenged(Environment.AddFunc(_relativeValue, offset), args);
                    args.Accept();
                }
                catch(Exception e)
                {
                    args.Reject();
                    throw;
                }
            }

            public void Set(T value)
            {
                var offset = Environment.SubstractFunc(Value, value);
                Move(offset);
            }

            public IRestrictionOwner Owner => (IRestrictionOwner)From;
            public IRestrictionSubject Subject => (IRestrictionSubject)To;

            public event ValueChangedEventHandler ValueChanged;
            public void Accept(T originalValue)
            {
                
            }

            public void Reject(T originalValue)
            {
                throw new NotImplementedException();
            }

            void IEventedValue.TryChangeValue(T newValue, ValueChangedEventArgs args)
            {
                OnRelativeValueChenged(newValue, args);
            }

            public T SubjectOffset => Offset(Subject.Value);

            public T Offset(T value)
            {
                var realValue = IncludeValue
                    ? Value
                    : Environment.AddFunc
                        (
                            Value,
                            Direction == Direction.Left
                                ? Environment.Epsilon
                                : Environment.NegateFunc(Environment.Epsilon)
                        );

                var offset = Direction == Direction.Left
                    ? Environment.SubstractFunc(value, realValue)
                    : Environment.SubstractFunc(realValue, value);

                return offset;
            }

            protected internal void OnRelativeValueChenged(T newValue, ValueChangedEventArgs args)
            {

                args.Add(this, RelativeValue);

                _relativeValue = newValue;

                if (SubjectOffset.CompareTo(Environment.Zerro) < 0)
                {
                    Subject.TryChangeValue(Value, args);
                }
            }

            public override string ToString()
            {
                return ToString("C", null);
            }

            public string ToString(string format, IFormatProvider formatProvider)
            {
                return ToString(Owner, Subject, Direction, RelativeValue, IncludeValue, format, formatProvider);
            }


            public static string ToString(IEventedValue owner, IEventedValue subject, Direction direction, T relativeValue, bool includeValue, string format = "C", IFormatProvider formatProvider = null)
            {
                var environment = (owner ?? subject).Environment;
                var comparationOperator = direction == Direction.Left ? ">" : "<";
                if (includeValue)
                    comparationOperator += "=";

                var comp = relativeValue.CompareTo(environment.Zerro);

                var ariphmethicOperator = comp == 0
                    ? ""
                    : comp > 0 ? "+" : "-";

                var s = subject?.ToString(format, null);
                var o = owner?.ToString(format, null);
                var r = (relativeValue as IFormattable)?.ToString(environment.ValueFormat, environment.ValueFormatProvider) ?? relativeValue.ToString();

                return comp == 0
                    ? $"{s} {comparationOperator} {o}"
                    : $"{s} {comparationOperator} {o} {ariphmethicOperator} {r}";
            }
        }

        //public class RestrictionEdge : EnvironmentEdge, IRestrictionsInterval
        //{
        //    public RestrictionEdge(Environment<T> environment, IGraphNode @from, AnchorNode to) : base(environment, @from, to)
        //    {
        //        Restrictions = new RestrictionsInterval(Environment);
        //        Restrictions.Restrict(default(T), Direction.Left);
        //        to.AddRestriction(Restrictions);

        //    }


        //    private AnchorNode Target => (AnchorNode) To;

        //    public override void AfterRemove()
        //    {
        //        ((AnchorNode)To).RemoveRestriction(Restrictions);
        //        base.AfterRemove();
        //    }

        //    public RestrictionType RestrictionType { get; set; }
        //    public IRestrictionsInterval Restrictions { get; }

        //    public override string ToString()
        //    {
        //        return $"{RestrictionType} {Restrictions}";
        //    }

        //    public event PropertyChangedEventHandler PropertyChanged
        //    {
        //        add { Restrictions.PropertyChanged += value; }
        //        remove { Restrictions.PropertyChanged -= value; }
        //    }
        //    Environment<T> IRestrictionsInterval.Environment => Environment;
        //    public IRestrictionsInterval Owner => Restrictions.Owner;
        //    public IRestriction Left => Restrictions.Left;
        //    public IRestriction Right => Restrictions.Right;
        //    public void Restrict(T value, Direction direction, bool includeValue = true)
        //    {
        //        throw new System.NotSupportedException();
        //    }

        //    public IRestrictionsInterval Restrict()
        //    {
        //        throw new System.NotSupportedException();
        //    }

        //    public void AddRestriction(IRestrictionsInterval restriction)
        //    {
        //        throw new System.NotSupportedException();

        //    }

        //    public void RemoveRestriction(IRestrictionsInterval restriction)
        //    {
        //        throw new System.NotSupportedException();
        //    }

        //    public void Clear(Direction? direction = null)
        //    {
        //        throw new System.NotSupportedException();
        //    }

        //    public void Offsets(T value, out T leftOffset, out T rightOffset)
        //    {
        //        Restrictions.Offsets(value, out leftOffset, out rightOffset);
        //    }

        //    public bool Contains(T value)
        //    {
        //        return Restrictions.Contains(value);
        //    }
        //}

    }
}