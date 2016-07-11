using System.ComponentModel;
using System.Runtime.InteropServices;
using Restrictions.Graph;

namespace Restrictions
{
    public partial class Environment<T>
    {
        public class RestrictionEdge : EnvironmentEdge, IRestrictionsInterval
        {
            public RestrictionEdge(Environment<T> environment, IGraphNode @from, AnchorNode to) : base(environment, @from, to)
            {
                Restrictions = new RestrictionsInterval(Environment);
                Restrictions.Restrict(default(T), Direction.Left);
                to.AddRestriction(Restrictions);

            }


            private AnchorNode Target => (AnchorNode) To;

            public override void AfterRemove()
            {
                ((AnchorNode)To).RemoveRestriction(Restrictions);
                base.AfterRemove();
            }

            public RestrictionType RestrictionType { get; set; }
            public IRestrictionsInterval Restrictions { get; }

            public override string ToString()
            {
                return $"{RestrictionType} {Restrictions}";
            }

            public event PropertyChangedEventHandler PropertyChanged
            {
                add { Restrictions.PropertyChanged += value; }
                remove { Restrictions.PropertyChanged -= value; }
            }
            Environment<T> IRestrictionsInterval.Environment => Environment;
            public IRestrictionsInterval Owner => Restrictions.Owner;
            public IRestriction Left => Restrictions.Left;
            public IRestriction Right => Restrictions.Right;
            public void Restrict(T value, Direction direction, bool includeValue = true)
            {
                throw new System.NotSupportedException();
            }

            public IRestrictionsInterval Restrict()
            {
                throw new System.NotSupportedException();
            }

            public void AddRestriction(IRestrictionsInterval restriction)
            {
                throw new System.NotSupportedException();

            }

            public void RemoveRestriction(IRestrictionsInterval restriction)
            {
                throw new System.NotSupportedException();
            }

            public void Clear(Direction? direction = null)
            {
                throw new System.NotSupportedException();
            }

            public void Offsets(T value, out T leftOffset, out T rightOffset)
            {
                Restrictions.Offsets(value, out leftOffset, out rightOffset);
            }

            public bool Contains(T value)
            {
                return Restrictions.Contains(value);
            }
        }

    }
}