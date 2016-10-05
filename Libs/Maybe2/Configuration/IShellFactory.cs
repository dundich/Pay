namespace Maybe2.Configuration
{
    public interface IShellFactory
    {
        T Get<T>();
    }
}