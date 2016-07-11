using System;

namespace Restrictions.Graph
{
    public class Factory
    {
        #region Fields

        internal readonly DataByKey<Type> EdgesByDataFuncs = new DataByKey<Type>();

        internal readonly DataByKey<Type> EdgesByTypeFuncs = new DataByKey<Type>();

        internal readonly DataByKey<Type> NodesByDataFuncs = new DataByKey<Type>();

        internal readonly DataByKey<Type> NodesByTypeFuncs = new DataByKey<Type>();

        #endregion

        #region Public Methods and Operators

        public void RegisterCreateEdge<TData, TEdge>(Func<IGraph, IGraphNode, IGraphNode, TData, TEdge> createInstance)
            where TEdge : IGraphEdge<TData>
        {
            EdgesByDataFuncs.Add(typeof(TData), createInstance);
        }

        public void RegisterCreateEdge<TEdge>(Func<IGraph, IGraphNode, IGraphNode, TEdge> createInstance)
            where TEdge : IGraphEdge
        {
            EdgesByTypeFuncs.Add(typeof(TEdge), createInstance);
        }

        public void RegisterCreateNode<TData, TNode>(Func<IGraph, TData, TNode> createInstance)
            where TNode : IGraphNode<TData>
        {
            NodesByDataFuncs.Add(typeof(TData), createInstance);
        }

        public void RegisterCreateNode<TNode>(Func<IGraph, TNode> createInstance)
            where TNode : IGraphNode
        {
            NodesByTypeFuncs.Add(typeof(TNode), createInstance);
        }

        #endregion
    }
}