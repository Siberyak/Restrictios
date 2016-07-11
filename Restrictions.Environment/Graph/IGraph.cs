using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Restrictions.Graph
{
    public interface IGraph
    {
        #region Properties

        IEnumerable<IGraphEdge> Edges { get; }
        IEnumerable<IGraphNode> Nodes { get; }

        #endregion

        #region Public Methods and Operators

        IGraphNode<T> Add<T>(T data);

        TNode Add<TNode, T>(T data) where TNode : IGraphNode<T>;

        T Add<T>() where T : IGraphNode;

        Task<IGraphNode<T>> AddAsync<T>(T data, CancellationToken cancellationToken);

        Task<TNode> AddAsync<TNode, T>(T data, CancellationToken cancellationToken) where TNode : IGraphNode<T>;

        Task<T> AddAsync<T>(CancellationToken cancellationToken) where T : IGraphNode;

        void AddNode(IGraphNode node);

        IEnumerable<IGraphEdge> EdgesFrom(IGraphNode node);

        IEnumerable<T> EdgesFrom<T>(IGraphNode node) where T : IGraphEdge;

        Task<IEnumerable<IGraphEdge>> EdgesFromAsync(IGraphNode node, CancellationToken cancellationToken);

        Task<IEnumerable<T>> EdgesFromAsync<T>(IGraphNode node, CancellationToken cancellationToken) where T : IGraphEdge;

        IEnumerable<IGraphEdge> EdgesTo(IGraphNode node);

        IEnumerable<T> EdgesTo<T>(IGraphNode node) where T : IGraphEdge;

        Task<IEnumerable<IGraphEdge>> EdgesToAsync(IGraphNode node, CancellationToken cancellationToken);

        Task<IEnumerable<T>> EdgesToAsync<T>(IGraphNode node, CancellationToken cancellationToken) where T : IGraphEdge;

        T FindNode<T>(Func<T, bool> predicate);

        Task<T> FindNodeAsync<T>(Func<T, bool> predicate, CancellationToken cancellationToken);

        IGraphEdge<T> Link<T>(T data, IGraphNode from, IGraphNode to);

        T Link<T>(IGraphNode from, IGraphNode to) where T : IGraphEdge;

        Task<IGraphEdge<T>> LinkAsync<T>(T data, IGraphNode from, IGraphNode to, CancellationToken cancellationToken);

        Task<T> LinkAsync<T>(IGraphNode from, IGraphNode to, CancellationToken cancellationToken) where T : IGraphEdge;

        void RemoveEdge<T>(params T[] edges)
            where T : IGraphEdge;

        void RemoveNode<T>(params T[] nodes)
            where T : IGraphNode;

        #endregion
    }
}