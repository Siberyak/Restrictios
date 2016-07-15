using System;
using System.Linq;
using Restrictions.Graph;

namespace Restrictions
{
    public partial class Environment<T>
    {

        public Item Add(string caption)
        {
            return new Item(this, caption);
        }

        public RestrictionEdge Res(AnchorNode node)
        {
            return _graph.Edges<RestrictionEdge>(true).FirstOrDefault(x => x.From == _root && x.To == node);
        }
    }


    public class Absolute : Environment<decimal>
    {

        public Absolute() : base()
        {
            AddFunc = (x, y) => x + y;
            SubstractFunc = (x, y) => x - y;
            Epsilon = 0.0001m;
        }

    }
}