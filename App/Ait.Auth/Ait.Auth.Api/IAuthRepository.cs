using Ait.Auth.Api.Entities;
using Ait.Auth.Api.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ait.Auth.Api
{
    public interface IAuthRepository
    {
        Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login);
        Task<bool> AddRefreshToken(RefreshToken token);
        Task<IdentityResult> CreateAsync(IdentityUser user);
        Task<IdentityUser> FindAsync(UserLoginInfo loginInfo);
        Task<Client> FindClientAsync(string clientId);
        Client FindClient(string clientId);
        Task<RefreshToken> FindRefreshToken(string refreshTokenId);
        Task<IdentityUser> FindUser(string userName, string password);
        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        List<RefreshToken> GetAllRefreshTokens();
        Task<IdentityResult> RegisterUser(UserModel userModel);
        Task<bool> RemoveRefreshToken(string refreshTokenId);
        Task<bool> RemoveRefreshToken(RefreshToken refreshToken);
    }
}