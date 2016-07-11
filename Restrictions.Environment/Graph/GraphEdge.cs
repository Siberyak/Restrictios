namespace Restrictions.Graph
{
    public class GraphEdge<T> : GraphEdge, IGraphEdge<T>
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public GraphEdge(IGraph graph, IGraphNode @from, IGraphNode to, T data)
            : base(graph, @from, to)
        {
            Data = data;
        }

        #endregion

        #region Properties

        public T Data { get; }

        #endregion
    }

    public abstract class GraphEdge : IGraphEdge
    {
        #region Fields

        protected readonly IGraph _graph;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        protected GraphEdge(IGraph graph, IGraphNode @from, IGraphNode to)
        {
            _graph = graph;
            From = @from;
            To = to;
        }

        #endregion

        #region Properties

        public IGraphNode From { get; }

        IGraph IGraphEdge.Graph
        {
            get
            {
                return _graph;
            }
        }

        public IGraphNode To { get; }

        #endregion

        public virtual void BeforeRemove()
        { }
        public virtual void AfterRemove()
        { }
    }
}