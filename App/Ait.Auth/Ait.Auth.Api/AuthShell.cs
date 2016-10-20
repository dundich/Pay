using Maybe2.Classes;
using Maybe2.Configuration;

namespace Ait.Auth.Api
{
    public class AuthShell : Shell, IAuthShell
    {
        private readonly LazyCache<IAuthRepository> _rep = null;

        public AuthShell(string tenat = null)
            : base(tenat)
        {
            _rep = new LazyCache<IAuthRepository>(() => new AuthRepository(CreateAuthContext));
        }

        internal AuthContext CreateAuthContext()
        {
            return new AuthContext(ConnectionString);
        }

        public IAuthRepository AuthRepository => _rep.Value;


        public override IShell CreateChild(string tenat)
        {
            return new AuthShell(tenat);
        }

        public override void Reset()
        {
            base.Reset();
            _rep.Reset();
        }
    }
}