using Maybe2.Configuration;

namespace Ait.Auth.Api
{
    public interface IShell
    {
        string Tenat { get; }

        string ConnectionString { get; }

        IAuthRepository AuthRepository { get; }

        IShellSettings Settings { get; }

        ISettingsProvider Provider { get; }

        Shell CreateShell(string tenat);

        void Reset();
    }
}