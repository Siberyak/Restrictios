using Restrictions.Graph;

namespace Restrictions
{
    public partial class Environment<T>
    {
        public class EnvironmentEdge : GraphEdge
        {
            protected readonly Environment<T> Environment;

            public EnvironmentEdge(Environment<T> environment, IGraphNode @from, IGraphNode to) : base(environment._graph, @from, to)
            {
                Environment = environment;
            }
        }
    }
}