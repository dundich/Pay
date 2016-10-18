using Ait.Auth.Api.Entities;
using Ait.Auth.Api.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ait.Auth.Api
{

    public class AuthRepository : IAuthRepository
    {
        private readonly Func<AuthContext> getAuthCtx;
        private readonly Func<UserManager<IdentityUser>> getUserCtx;

        public AuthRepository(Func<AuthContext> ctx)
        {
            getAuthCtx = ctx;
            getUserCtx = () => new UserManager<IdentityUser>(new UserStore<IdentityUser>(ctx()));
        }

        public async Task<IdentityResult> RegisterUser(UserModel userModel)
        {
            IdentityUser user = new IdentityUser
            {
                UserName = userModel.UserName
            };

            using (var um = getUserCtx())
            {
                var result = await um.CreateAsync(user, userModel.Password);
                return result;
            }
        }

        public async Task<IdentityUser> FindUser(string userName, string password)
        {
            using (var um = getUserCtx())
            {
                IdentityUser user = await um.FindAsync(userName, password);
                return user;
            }
        }

        public async Task<Client> FindClientAsync(string clientId)
        {
            using (var ctx = getAuthCtx())
            {
                var client = await ctx.Clients.FindAsync(clientId);
                return client;
            }
        }

        public Client FindClient(string clientId)
        {
            using (var ctx = getAuthCtx())
            {
                var client = ctx.Clients.Find(clientId);
                return client;
            }
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {
            using (var ctx = getAuthCtx())
            {
                var existingToken = ctx.RefreshTokens.Where(r => r.Subject == token.Subject && r.ClientId == token.ClientId).SingleOrDefault();

                if (existingToken != null)
                {
                    ctx.RefreshTokens.Remove(existingToken);
                    var result = await ctx.SaveChangesAsync() > 0;

                    //var result = await RemoveRefreshToken(existingToken);
                }

                ctx.RefreshTokens.Add(token);

                return await ctx.SaveChangesAsync() > 0;
            }
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            using (var ctx = getAuthCtx())
            {

                var refreshToken = await ctx.RefreshTokens.FindAsync(refreshTokenId);

                if (refreshToken != null)
                {
                    ctx.RefreshTokens.Remove(refreshToken);
                    return await ctx.SaveChangesAsync() > 0;
                }

                return false;
            }
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken refreshToken)
        {
            using (var ctx = getAuthCtx())
            {
                ctx.RefreshTokens.Remove(refreshToken);
                return await ctx.SaveChangesAsync() > 0;
            }
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            using (var ctx = getAuthCtx())
            {
                var refreshToken = await ctx.RefreshTokens.FindAsync(refreshTokenId);
                return refreshToken;
            }
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            using (var ctx = getAuthCtx())
            {
                return ctx.RefreshTokens.ToList();
            }
        }

        public async Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
        {
            using (var ctx = getUserCtx())
            {
                IdentityUser user = await ctx.FindAsync(loginInfo);
                return user;
            }
        }

        public async Task<IdentityResult> CreateAsync(IdentityUser user)
        {
            using (var ctx = getUserCtx())
            {
                var result = await ctx.CreateAsync(user);
                return result;
            }
        }

        public async Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login)
        {
            using (var ctx = getUserCtx())
            {
                var result = await ctx.AddLoginAsync(userId, login);
                return result;
            }
        }

        public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            using (var ctx = getUserCtx())
            {
                var result = await ctx.ChangePasswordAsync(userId, currentPassword, newPassword);
                return result;
            }
        }
    }
}