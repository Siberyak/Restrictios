using System.Collections.Generic;

namespace Restrictions.Graph
{
    public interface IGraphNode<T> : IGraphNode
    {
        #region Properties

        T Data { get; }

        #endregion
    }

    public interface IGraphNode
    {
        #region Properties

        IEnumerable<IGraphEdge> BackEdges { get; }
        IEnumerable<IGraphEdge> Edges { get; }
        IGraph Graph { get; }

        #endregion
    }
}