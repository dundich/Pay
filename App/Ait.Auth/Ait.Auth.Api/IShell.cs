using Maybe2.Configuration;

namespace Ait.Auth.Api
{
    public interface IShell: IShellSettings
    {
        string Tenat { get; }

        string ConnectionString { get; }

        IAuthRepository AuthRepository { get; }

        ISettingsProvider Provider { get; }

        IShell CreateShell(string tenat);

        string this[string key] { get; }
    }
}