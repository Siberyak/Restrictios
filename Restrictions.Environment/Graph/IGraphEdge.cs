namespace Restrictions.Graph
{
    public interface IGraphEdge<T> : IGraphEdge
    {
        #region Properties

        T Data { get; }

        #endregion
    }

    public interface IGraphEdge
    {
        #region Properties

        IGraphNode From { get; }
        IGraph Graph { get; }
        IGraphNode To { get; }

        void BeforeRemove();
        void AfterRemove();

        #endregion
    }
}