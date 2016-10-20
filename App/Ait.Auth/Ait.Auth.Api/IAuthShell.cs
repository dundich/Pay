using Maybe2.Configuration;

namespace Ait.Auth.Api
{
    public interface IAuthShell: IShell
    {
        IAuthRepository AuthRepository { get; }
    }
}