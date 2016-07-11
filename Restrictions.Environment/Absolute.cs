using System;
using System.Linq;
using Restrictions.Graph;

namespace Restrictions
{
    public partial class Environment<T>
    {
        protected internal readonly Graph.Graph _graph;
        private AnchorNode _root;

        public Environment()
        {

            var fatory = new Factory();
            fatory.RegisterCreateNode(graph => new AnchorNode(this));
            fatory.RegisterCreateEdge((graph, from, to) => new RestrictionEdge(this, from, (AnchorNode)to));

            _graph = new Graph.Graph(fatory);
            _root = _graph.Add<AnchorNode>();
        }   

        public Item Add()
        {
            return new Item(this);
        }

        public RestrictionEdge Res(AnchorNode node)
        {
            return _graph.Edges<RestrictionEdge>(true).FirstOrDefault(x => x.From == _root && x.To == node);
        }
    }


    public class Absolute : Environment<decimal>
    {

        public Absolute()
        {
            AddFunc = (x, y) => x + y;
            SubstractFunc = (x, y) => x - y;
            Epsilon = 0.0001m;
        }

    }
}