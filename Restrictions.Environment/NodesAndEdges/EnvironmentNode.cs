using Restrictions.Graph;

namespace Restrictions
{
    public partial class Environment<T>
    {
        public abstract class EnvironmentNode : GraphNode
        {
            protected readonly Environment<T> Environment;

            protected EnvironmentNode(Environment<T> environment) : base(environment._graph)
            {
                Environment = environment;
            }
        }
    }
}