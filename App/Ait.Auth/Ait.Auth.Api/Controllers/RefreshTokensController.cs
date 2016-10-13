﻿using Ait.Auth.Api.Modules;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Ait.Auth.Api.Controllers
{
    [RoutePrefix("api/RefreshTokens")]
    public class RefreshTokensController : ApiController
    {

        private IAuthRepository _repo => Request.GetOwinContext().Get<IAuthRepository>(OwinConsts.AuthRepository);

        public RefreshTokensController()
        {
            //_repo = new AuthRepository();
        }

        [Authorize(Users = "Admin")]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(_repo.GetAllRefreshTokens());
        }

        //[Authorize(Users = "Admin")]
        [AllowAnonymous]
        [Route("")]
        public async Task<IHttpActionResult> Delete(string tokenId)
        {
            var result = await _repo.RemoveRefreshToken(tokenId);
            if (result)
            {
                return Ok();
            }
            return BadRequest("Token Id does not exist");
        }

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        _repo.Dispose();
        //    }

        //    base.Dispose(disposing);
        //}
    }
}
