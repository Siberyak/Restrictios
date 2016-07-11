namespace KG.Planner.Data
{
    public interface IMutable
    {
        event ChangedEventHandler Changed;
    }

    public delegate void ChangedEventHandler(object sender, bool isRestored);

    public interface IMutable<T> : IMutable
    {
        new event ChangedEventHandler<T> Changed;
    }

    public delegate void ChangedEventHandler<in T>(T sender, bool isRestored);
}