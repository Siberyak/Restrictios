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
            protected readonly Environment<T> Environment;

            public IEventedValue Start => _start;
            public IEventedValue Finish => _finish;

            public Item(Environment<T> environment, string caption)
            {
                Environment = environment;
                _graph = environment._graph;

                _start = _graph.Add<AnchorNode>();
                _start._caption = caption + ".start";

                _finish = _graph.Add<AnchorNode>();
                _finish._caption = caption + ".finish";

                var r = (_finish >= _start).Apply();
            }

            //public void RestrictMinDuration(T value, bool included = true)
            //{
            //    _durationRestrictions.Restrictions.Clear(Direction.Left);
            //    _durationRestrictions.Restrictions.Restrict(value, Direction.Left, included);
            //    _finish.RecalcValue();
            //}

            //public void RestrictMaxDuration(T value, bool included = true)
            //{
            //    _durationRestrictions.Restrictions.Clear(Direction.Right);
            //    _durationRestrictions.Restrictions.Restrict(value, Direction.Right, included);
            //    _finish.RecalcValue();
            //}

            //public void RestrictDuration(T min, T max, bool minIncluded = true, bool maxIncleded = true)
            //{
            //    _durationRestrictions.Restrictions.Clear();
            //    _durationRestrictions.Restrictions.Restrict(min, Direction.Left, minIncluded);
            //    _durationRestrictions.Restrictions.Restrict(max, Direction.Right, maxIncleded);

            //    _finish.RecalcValue();
            //}

            //public void OffsetStart(T offset)
            //{
            //    _start.Value = Environment.AddFunc(_start.Value, offset);
            //}


            public Item Add(string caption)
            {
                var item = new Item(Environment, caption);

                var r1 = (item._start >= _start).Apply();
                var r2 = (item._finish <= _finish).Apply();

                return item;
            }

            public override string ToString()
            {
                return $"[{_start} - {_finish}]";
            }
        } 
    }
}