using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Restrictions.Graph;

namespace Restrictions
{
    public partial class Environment<T>
        { 
        public class AnchorNode : EnvironmentNode
        {
            private readonly IRestrictionsInterval _restrictions;
            private T _value;

            public AnchorNode(Environment<T> environment) : base(environment)
            {
                _restrictions = new RestrictionsInterval(Environment);
                _restrictions.PropertyChanged += RestrictionsPropertyChanged;
            }

            private void RestrictionsPropertyChanged(object sender, PropertyChangedEventArgs e)
            {
                RecalcValue();
            }

            public void AddRestriction(IRestrictionsInterval restriction)
            {
                _restrictions.AddRestriction(restriction);
            }

            public void RemoveRestriction(IRestrictionsInterval restriction)
            {
                _restrictions.RemoveRestriction(restriction);
            }

            public IRestrictionsInterval Restrictions => _restrictions;

            public AnchorNode BaseAnchor
            {
                get { return GetNodesByBackEdge<RestrictionEdge>(x => x.RestrictionType != RestrictionType.None).OfType<AnchorNode>().FirstOrDefault() ?? this; }
            }

            public IEnumerable<AnchorNode> Items
            {
                get { return GetNodesByEdge<RestrictionEdge>().OfType<AnchorNode>(); }
            }

            public T Value
            {
                get { return _value; }
                set
                {
                    if (_value.CompareTo(value) == 0)
                        return;

                    //Console.WriteLine($"[{GetHashCode()}]: Value = [{_value} -> {value}]");
                    _value = value;

                    foreach (var item in Items)
                    {
                        item.RecalcValue();
                    }
                }
            }

            protected internal void RecalcValue()
            {
                T leftOffset;
                T rightOffset;

                Restrictions.Offsets(Value, out leftOffset, out rightOffset);
                if (!Restrictions.Left.IsEmpty && leftOffset.CompareTo(Zerro) < 0)
                    Value = Environment.AddFunc(Value, Environment.SubstractFunc(Zerro, leftOffset));
                else if (!Restrictions.Right.IsEmpty && rightOffset.CompareTo(Zerro) < 0)
                    Value = Environment.AddFunc(Value, Environment.SubstractFunc(Zerro, rightOffset));


                //Console.WriteLine($"[{GetHashCode()}]: RecalcValue");
            }

            public T AbsoluteValue => BaseAnchor == this ? Value : Environment.AddFunc(BaseAnchor.AbsoluteValue, Value);


            public override string ToString()
            {
                return $"{AbsoluteValue}";
            }

            
        }


    }
}
