using System.Collections;
using System.Collections.Generic;
using System.Linq;
using KG.Planner.Data;

namespace Restrictions.Graph
{
    public static class GraphExtender
    {
        #region Public Methods and Operators

        public static IEnumerable<TEdge> AllByEdges<TEdge>(this IGraphNode node, bool strong = true)
            where TEdge : IGraphEdge
        {
            IEnumerable<TEdge> local = node.Edges<TEdge>(strong).ToArray();
            return local.Union(local.SelectMany(x => x.To.AllByEdges<TEdge>()));
        }

        public static IEnumerable<T> BackEdges<T>(this IGraphNode node, bool strongTypeCheck = false)
            where T : IGraphEdge
        {
            return node?.BackEdges.OfTypeWithCheck<T>(strongTypeCheck);
        }

        public static IEnumerable<T> Edges<T>(this IGraphNode node, bool strongTypeCheck = false)
            where T : IGraphEdge
        {
            return node?.Edges.OfTypeWithCheck<T>(strongTypeCheck);
        }

        public static IEnumerable<T> Edges<T>(this IGraph graph, bool strongTypeCheck = false)
            where T : IGraphEdge
        {
            return graph?.Edges.OfTypeWithCheck<T>(strongTypeCheck);
        }

        public static IEnumerable<T> Nodes<T>(this IGraph graph, bool strongTypeCheck = false)
            where T : IGraphNode
        {
            return graph?.Nodes.OfTypeWithCheck<T>(strongTypeCheck);
        }

        public static void SubscribeUpdates(this IGraphNode node, ChangedEventHandler<IGraphNode> handler)
        {
            IMutableNode mutableNode = node as IMutableNode;
            if (mutableNode != null)
            {
                mutableNode.Changed += handler;
            }
        }

        public static void UnsubscribeUpdates(this IGraphNode node, ChangedEventHandler<IGraphNode> handler)
        {
            IMutableNode mutableNode = node as IMutableNode;
            if (mutableNode != null)
            {
                mutableNode.Changed -= handler;
            }
        }

        #endregion

        #region Methods

        static IEnumerable<T> OfTypeWithCheck<T>(this IEnumerable enumerable, bool strongTypeCheck = false)
        {
            return
                (
                    strongTypeCheck
                        ? enumerable?.OfType<object>().Where(x => x != null && x.GetType() == typeof(T)).OfType<T>()
                        : enumerable.OfType<T>()
                )
                ?? Enumerable.Empty<T>();
        }

        #endregion
    }

    public interface IMutableNode : IGraphNode, IMutable<IGraphNode>
    {
    }
}