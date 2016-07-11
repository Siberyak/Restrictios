using Restrictions.Graph;

namespace Restrictions
{
    public partial class Environment<T>
    {
        public class Item
        {
            IGraph _graph;
            private readonly AnchorNode _start;
            private readonly AnchorNode _finish;
            private RestrictionEdge _durationRestrictions;
            private Environment<T> _environment;

            public AnchorNode Start => _start;
            public AnchorNode Finish => _finish;

            public Item(Environment<T> environment)
            {
                _environment = environment;
                _graph = environment._graph;

                _start = _graph.Add<AnchorNode>();
                _graph.Link<RestrictionEdge>(environment._root, _start);

                _finish = _graph.Add<AnchorNode>();
                _graph.Link<RestrictionEdge>(environment._root, _finish);

                _durationRestrictions = _graph.Link<RestrictionEdge>(_start, _finish);
                _durationRestrictions.RestrictionType = RestrictionType.Owner;
            }

            public void RestrictMinDuration(T value, bool included = true)
            {
                _durationRestrictions.Restrictions.Clear(Direction.Left);
                _durationRestrictions.Restrictions.Restrict(value, Direction.Left, included);
                RecalcFinish();
            }

            public void RestrictMaxDuration(T value, bool included = true)
            {
                _durationRestrictions.Restrictions.Clear(Direction.Right);
                _durationRestrictions.Restrictions.Restrict(value, Direction.Right, included);
                RecalcFinish();
            }

            public void RestrictDuration(T min, T max, bool minIncluded = true, bool maxIncleded = true)
            {
                _durationRestrictions.Restrictions.Clear();
                _durationRestrictions.Restrictions.Restrict(min, Direction.Left, minIncluded);
                _durationRestrictions.Restrictions.Restrict(max, Direction.Right, maxIncleded);

                RecalcFinish();
            }

            private void RecalcStart()
            { }

            private void RecalcFinish()
            {
                T leftOffset;
                T rightOffset;

                _durationRestrictions.Restrictions.Offsets(_finish.Value, out leftOffset, out rightOffset);
                if (leftOffset.CompareTo(Zerro) < 0)
                    _finish.Value = _environment.SubstractFunc(Zerro, leftOffset);
                else if (rightOffset.CompareTo(Zerro) < 0)
                    _finish.Value = _environment.SubstractFunc(Zerro, rightOffset);
            }

            public void OffsetStart(T offset)
            {
                _start.Value = _environment.AddFunc(_start.Value, offset);
            }

            public override string ToString()
            {
                return $"[{_start} - {_finish}] {_durationRestrictions}";
            }
        } 
    }
}