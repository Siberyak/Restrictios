using System;
using System.Collections.Generic;
using System.Linq;

namespace Restrictions.Graph
{
    public class GraphNode<T> : GraphNode, IGraphNode<T>
    {
        #region Constructors and Destructors

        public GraphNode(IGraph graph, T data)
            : base(graph)
        {
            Data = data;
        }

        #endregion

        #region Properties

        public T Data { get; }

        #endregion


    }

    public abstract class GraphNode : IGraphNode
    {
        #region Fields

        protected readonly IGraph _graph;

        #endregion

        #region Constructors and Destructors

        protected GraphNode(IGraph graph)
        {
            _graph = graph;
        }

        #endregion

        #region Properties

        public IEnumerable<IGraphEdge> BackEdges => _graph.Edges.Where(x => x.To == this);

        public IEnumerable<IGraphEdge> Edges => _graph.Edges.Where(x => x.From == this);

        IGraph IGraphNode.Graph
        {
            get { return _graph; }
        }

        #endregion


        protected IGraphNode GetNodeByBackEdge<TEdge>(Func<TEdge, bool> predicate = null)
           where TEdge : IGraphEdge
        {
            return BackEdges.OfType<TEdge>().FirstOrDefault(x => predicate?.Invoke(x) ?? true)?.From;
        }

        protected IGraphNode GetNodeByEdge<TEdge>(Func<TEdge, bool> predicate = null)
            where TEdge : IGraphEdge
        {
            return Edges.OfType<TEdge>().FirstOrDefault(x => predicate?.Invoke(x) ?? true)?.To;
        }

        protected IEnumerable<IGraphNode> GetNodesByBackEdge<TEdge>(Func<TEdge, bool> predicate = null)
            where TEdge : IGraphEdge
        {
            return BackEdges.OfType<TEdge>().Where(x => predicate?.Invoke(x) ?? true).Select(x => x.From);
        }

        protected IEnumerable<IGraphNode> GetNodesByEdge<TEdge>(Func<TEdge, bool> predicate = null)
            where TEdge : IGraphEdge
        {
            return Edges.OfType<TEdge>().Where(x => predicate?.Invoke(x) ?? true).Select(x => x.To);
        }

        protected TEdge SetBackEdgeByNode<TEdge>(IGraphNode node, Func<TEdge, bool> removeSameTypeLinks = null)
            where TEdge : IGraphEdge
        {
            if (removeSameTypeLinks != null)
                _graph.RemoveEdge(BackEdges.OfType<TEdge>().Where(removeSameTypeLinks).ToArray());

            return node != null ? _graph.Link<TEdge>(node, this) : default(TEdge);
        }

        protected TEdge SetBackEdgeByNode<TEdge>(IGraphNode node, bool removeSameTypeLinks = true)
            where TEdge : IGraphEdge
        {
            return removeSameTypeLinks 
                ? SetBackEdgeByNode<TEdge>(node, x => true) 
                : SetBackEdgeByNode<TEdge>(node, null);
        }

        protected TEdge SetEdgeByNode<TEdge>(IGraphNode node, Func<TEdge, bool> removeSameTypeLinks = null)
            where TEdge : IGraphEdge
        {

            if(removeSameTypeLinks != null)
                _graph.RemoveEdge(Edges.OfType<TEdge>().Where(removeSameTypeLinks).ToArray());

            return node != null ? _graph.Link<TEdge>(this, node) : default(TEdge);
        }

        protected TEdge SetEdgeByNode<TEdge>(IGraphNode node, bool removeSameTypeLinks = true)
            where TEdge : IGraphEdge
        {
            return removeSameTypeLinks
                ? SetEdgeByNode<TEdge>(node, x => true)
                : SetEdgeByNode<TEdge>(node, null);
        }
    }
}