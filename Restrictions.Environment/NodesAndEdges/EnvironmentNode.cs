using Restrictions.Graph;

namespace Restrictions
{
    public partial class Environment<T>
    {
        public class EnvironmentNode : GraphNode
        {
            protected readonly Environment<T> Environment;

            public EnvironmentNode(Environment<T> environment) : base(environment._graph)
            {
                Environment = environment;
            }
        }
    }
}