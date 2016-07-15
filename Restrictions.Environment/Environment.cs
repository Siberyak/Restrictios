using System;
using Restrictions.Graph;

namespace Restrictions
{
    public partial class Environment<T>
        where T : struct, IComparable<T>, IEquatable<T>
    {
        public string ValueFormat;
        public IFormatProvider ValueFormatProvider;

        public Func<T, T, T> AddFunc;
        public Func<T, T, T> SubstractFunc;
        public T Epsilon;
        public T Zerro = default(T);

        public Func<T, T> NegateFunc;

        protected internal readonly Graph.Graph _graph;
        private readonly EnvironmentNode _root;

        public Environment()
        {

            NegateFunc = x => SubstractFunc(Zerro, x);

            var fatory = new Factory();
            fatory.RegisterCreateNode(graph => new EnvironmentNode(this));
            fatory.RegisterCreateNode(graph => new AnchorNode(this));
            fatory.RegisterCreateEdge((graph, from, to) => new RestrictionEdge(this, (AnchorNode)from, (AnchorNode)to));

            _graph = new Graph.Graph(fatory);
            _root = _graph.Add<EnvironmentNode>();
        }

        //public IRestrictionsInterval Restrict()
        //{
        //    return new RestrictionsInterval(this);
        //}
    }
}