using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Restrictions.Graph;

namespace Restrictions
{
    public partial class Environment<T>
    {
        public class AnchorNode : EnvironmentNode, IRestrictionOwner, IRestrictionSubject
        {
            Environment<T> IValue.Environment => Environment;


            public static RestrictionInfo operator +(AnchorNode owner, T value)
            {
                return new RestrictionInfo() {Owner = owner, RelativeValue = value};
            }
            public static RestrictionInfo operator -(AnchorNode owner, T value)
            {
                return owner + owner.Environment.NegateFunc(value);
            }

            public static RestrictionInfo operator <(AnchorNode subject, RestrictionInfo info)
            {
                var result = subject < (AnchorNode)info.Owner;
                result.RelativeValue = info.RelativeValue;
                return result;
            }
            public static RestrictionInfo operator >(AnchorNode subject, RestrictionInfo info)
            {
                var result = subject > (AnchorNode)info.Owner;
                result.RelativeValue = info.RelativeValue;
                return result;
            }

            public static RestrictionInfo operator <=(AnchorNode subject, RestrictionInfo info)
            {
                var result = subject <= (AnchorNode)info.Owner;
                result.RelativeValue = info.RelativeValue;
                return result;
            }

            public static RestrictionInfo operator >=(AnchorNode subject, RestrictionInfo info)
            {
                var result = subject >= (AnchorNode)info.Owner;
                result.RelativeValue = info.RelativeValue;
                return result;
            }

            public static RestrictionInfo operator <(AnchorNode subject, AnchorNode owner)
            {
                return new RestrictionInfo { Owner = owner, Subject = subject, Direction = Direction.Right };
            }
            public static RestrictionInfo operator >(AnchorNode subject, AnchorNode owner)
            {
                return new RestrictionInfo { Owner = owner, Subject = subject, Direction = Direction.Left };
            }

            public static RestrictionInfo operator <=(AnchorNode subject, AnchorNode owner)
            {
                var info = subject < owner;
                info.IncludeValue = true;
                return info;
            }

            public static RestrictionInfo operator >=(AnchorNode subject, AnchorNode owner)
            {
                var info = subject > owner;
                info.IncludeValue = true;
                return info;
            }

            internal string _caption;

            public AnchorNode(Environment<T> environment) : base(environment)
            {
            }

            public T Value { get; private set; }
            public void Move(T offset)
            {
                Set(Environment.AddFunc(Value, offset));
            }

            public void Set(T value)
            {
                var args = new ValueChangedEventArgs();
                try
                {
                    TryChangeValue(value, args);
                    args.Accept();
                }
                catch(Exception e)
                {
                    args.Reject();
                    throw;
                }
            }

            public IEnumerable<IRestriction> OwnedRestrictions => Edges.OfType<IRestriction>();
            public IEnumerable<IRestriction> SubjectForRestrictions => BackEdges.OfType<IRestriction>();


            public override string ToString()
            {
                return ToString(null, null);
            }

            public string ToString(string format, IFormatProvider formatProvider)
            {
                if (string.IsNullOrEmpty(format))
                    return $"{_caption}, v: {Value}";

                switch (format)
                {
                    case "C":
                        return _caption;
                    case "V":
                        return Value.ToString();
                }

                return ToString();
            }

            public event ValueChangedEventHandler ValueChanged;

            public void Accept(T originalValue)
            {
                
            }

            public void Reject(T originalValue)
            {
                throw new NotImplementedException();
            }

            public IRestriction Restrict(RestrictionInfo restrictionInfo)
            {
                return Restrict(restrictionInfo.Subject, restrictionInfo.RelativeValue, restrictionInfo.Direction, restrictionInfo.IncludeValue);
            }

            public IRestriction Restrict(IRestrictionSubject subject, T realtiveValue, Direction direction, bool included = true)
            {
                var edge = _graph.Link<RestrictionEdge>(this, (IGraphNode) subject);
                edge.RelativeValue = realtiveValue;
                edge.Direction = direction;
                edge.IncludeValue = included;


                ValueChangedEventArgs args = new ValueChangedEventArgs();
                try
                {
                    subject.TryChangeValue(subject.Value, args);
                    args.Accept();
                    return edge;
                }
                catch(Exception e)
                {
                    _graph.RemoveEdge(edge);
                    args.Reject();
                    throw;
                }
            }

            private bool _changingValue;

            void IEventedValue.TryChangeValue(T newValue, ValueChangedEventArgs args)
            {
                TryChangeValue(newValue, args);
            }

            protected internal void TryChangeValue(T value, ValueChangedEventArgs args)
            {

                if(value.CompareTo(Value) == 0)
                    return;

                if (_changingValue)
                    throw new Exception("value changing loop detected");

                args.Add(this, Value);

                Value = value;

                try
                {
                    _changingValue = true;

                    NormalizeSubjectRestrictions(args);

                    NoranlizeOwnedRestrictions(args);

                    CheckSubjectsOwnedRestrictionsIsValid();
                }
                finally
                {
                    _changingValue = false;
                }
            }

            private void NoranlizeOwnedRestrictions(ValueChangedEventArgs args)
            {
                var ownedRestrictions = OwnedRestrictions.ToArray();
                var enumerable = ownedRestrictions
                    .Select(x => new {Offset = x.Offset(x.Subject.Value), Edge = x}).ToArray();
                var ownedRestrictionsForChange = enumerable
                    .Where(x => x.Offset.CompareTo(Environment.Zerro) < 0)
                    .ToArray();

                foreach (var info in ownedRestrictionsForChange)
                {
                    info.Edge.Subject.TryChangeValue(info.Edge.Value, args);
                }
            }

            private void NormalizeSubjectRestrictions(ValueChangedEventArgs args)
            {
                var subjectRestrictionsForChange = SubjectForRestrictions
                    .Select(x => new {Offset = x.Offset(Value), Edge = x})
                    .Where(x => x.Offset.CompareTo(Environment.Zerro) < 0)
                    .ToArray();

                foreach (var info in subjectRestrictionsForChange)
                {
                    var newValue = Environment.SubstractFunc(info.Edge.Owner.Value, info.Offset);
                    info.Edge.Owner.TryChangeValue(newValue, args);
                }
            }

            private void CheckSubjectsOwnedRestrictionsIsValid()
            {
                
            }

            static Direction DirectionTo(T @from, T to)
            {
                var compareTo = @from.CompareTo(to);
                if(compareTo == 0)
                    throw new Exception("cant detect direction - values is equals");
                if(compareTo > 0)
                    return Direction.Left;

                return Direction.Right;
            }
        }

        public interface IValue : IFormattable
        {
            T Value { get; }
            Environment<T> Environment { get; }
            void Move(T offset);
            void Set(T value);
        }

        public interface IEventedValue : IValue
        {
            event ValueChangedEventHandler ValueChanged;

            void TryChangeValue(T newValue, ValueChangedEventArgs args);
            void Accept(T originalValue);
            void Reject(T originalValue);
        }

        public class ValueChangedEventArgs : EventArgs
        {
            private readonly List<KeyValuePair<IEventedValue, T>> _originalValues;

            public ValueChangedEventArgs()
            {
                _originalValues = new List<KeyValuePair<IEventedValue, T>>();
            }

            public void Add(IEventedValue key, T value)
            {
                if (key == null)
                    return;

                _originalValues.Add(new KeyValuePair<IEventedValue, T>(key, value));
            }

            public IEnumerable<T> this[IEventedValue key]
            {
                get { return _originalValues.Where(x => x.Key == key).Select(x => x.Value); }
            }

            public bool Success { get; set; } = true;

            public void Accept()
            {
                foreach (var pair in _originalValues)
                {
                    pair.Key.Accept(pair.Value);
                }
            }
            public void Reject()
            {
                var pairs = _originalValues.AsEnumerable().Reverse();
                foreach (var pair in pairs)
                {
                    pair.Key.Reject(pair.Value);
                }
            }
        }

        public delegate void ValueChangedEventHandler(IEventedValue sender, ValueChangedEventArgs args);

        #region old

        //public class AnchorNode : EnvironmentNode
        //{
        //    internal string _caption;


        //    private readonly IRestrictionsInterval _restrictions;
        //    private T _value;

        //    public AnchorNode(Environment<T> environment) : base(environment)
        //    {
        //        _restrictions = new RestrictionsInterval(Environment);
        //        _restrictions.PropertyChanged += RestrictionsPropertyChanged;
        //    }

        //    private void RestrictionsPropertyChanged(object sender, PropertyChangedEventArgs e)
        //    {
        //        RecalcValue();
        //    }

        //    public void AddRestriction(IRestrictionsInterval restriction)
        //    {
        //        _restrictions.AddRestriction(restriction);
        //    }

        //    public void RemoveRestriction(IRestrictionsInterval restriction)
        //    {
        //        _restrictions.RemoveRestriction(restriction);
        //    }

        //    public IRestrictionsInterval Restrictions => _restrictions;

        //    public AnchorNode BaseAnchor
        //    {
        //        get { return GetNodesByBackEdge<RestrictionEdge>(x => x.RestrictionType != RestrictionType.None).OfType<AnchorNode>().FirstOrDefault() ?? this; }
        //    }

        //    public IEnumerable<AnchorNode> Items
        //    {
        //        get { return GetNodesByEdge<RestrictionEdge>().OfType<AnchorNode>(); }
        //    }

        //    public T Value
        //    {
        //        get { return _value; }
        //        set
        //        {
        //            if (_value.CompareTo(value) == 0)
        //                return;

        //            //Console.WriteLine($"[{GetHashCode()}]: Value = [{_value} -> {value}]");
        //            _value = value;

        //            foreach (var item in Items)
        //            {
        //                item.RecalcValue();
        //            }
        //        }
        //    }

        //    protected internal void RecalcValue()
        //    {
        //        T leftOffset;
        //        T rightOffset;

        //        Restrictions.Offsets(Value, out leftOffset, out rightOffset);
        //        if (!Restrictions.Left.IsEmpty && leftOffset.CompareTo(Zerro) < 0)
        //            Value = Environment.AddFunc(Value, Environment.SubstractFunc(Zerro, leftOffset));
        //        else if (!Restrictions.Right.IsEmpty && rightOffset.CompareTo(Zerro) < 0)
        //            Value = Environment.AddFunc(Value, Environment.SubstractFunc(Zerro, rightOffset));


        //        //Console.WriteLine($"[{GetHashCode()}]: RecalcValue");
        //    }

        //    public T AbsoluteValue => BaseAnchor == this ? Value : Environment.AddFunc(BaseAnchor.AbsoluteValue, Value);


        //    public override string ToString()
        //    {
        //        return $"{_caption}, av: {AbsoluteValue}, v: {Value}";
        //    }


        //}

        #endregion

        //public class BaseRestrictionInfo
        //{
        //    public IRestrictionOwner Owner { get; set; }
        //    public IRestrictionSubject Subject { get; set; }
        //    protected bool _isSubst;

        //    public static BaseRestrictionInfo Add(IRestrictionOwner owner, IRestrictionSubject subject)
        //    {
        //        return new BaseRestrictionInfo {Owner = owner, Subject = subject};
        //    }

        //    public static BaseRestrictionInfo Substract(IRestrictionOwner owner, IRestrictionSubject subject)
        //    {
        //        return new BaseRestrictionInfo {Owner = owner, Subject = subject, _isSubst = true};
        //    }

        //    public static RestrictionInfo operator >(BaseRestrictionInfo info, T value)
        //    {
        //        Direction direction = info._isSubst ?  Direction.Left: Direction.Right;
        //        return new RestrictionInfo {Owner = info.Owner, Subject = info.Subject, Direction = direction, RelativeValue = value};
        //    }
        //    public static RestrictionInfo operator <(BaseRestrictionInfo info, T value)
        //    { }
        //}

        public class RestrictionInfo
        {
            private Environment<T> Environment => ((IEventedValue) Owner ?? Subject).Environment;


            
            public override string ToString()
            {
                return RestrictionEdge.ToString(Owner, Subject, Direction, RelativeValue, IncludeValue);
            }

            public RestrictionInfo()
            {
            }

            private RestrictionInfo(RestrictionInfo info)
            {
                Owner = info.Owner;
                Subject = info.Subject;
                Direction = info.Direction;
                RelativeValue = info.RelativeValue;
                IncludeValue = info.IncludeValue;
            }

            public IRestrictionOwner Owner { get; set; }
            public IRestrictionSubject Subject { get; set; }
            public bool IncludeValue { get; set; }
            public T RelativeValue { get; set; }
            public Direction Direction { get; set; }

            public IRestriction Apply()
            {
                return Owner.Restrict(Subject, RelativeValue, Direction, IncludeValue);
            }

            public static RestrictionInfo operator +(RestrictionInfo info, T value)
            {
                var result = info.Clone();
                result.RelativeValue = result.Owner.Environment.AddFunc(result.RelativeValue, value);
                return result;
            }
            public static RestrictionInfo operator -(RestrictionInfo info, T value)
            {
                var result = info.Clone();
                result.RelativeValue = result.Owner.Environment.SubstractFunc(result.RelativeValue, value);
                return result;
            }

            public RestrictionInfo Clone()
            {
                return new RestrictionInfo(this);
            }
        }
    }
}
