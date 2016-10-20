namespace Maybe2.Configuration
{
    public interface IShell : IShellSettings
    {
        string Tenat { get; }

        string ConnectionString { get; }        

        ISettingsProvider Provider { get; }

        IShell CreateChild(string tenat);

        DynamicDictionary<string> Config { get; }
    }
}
