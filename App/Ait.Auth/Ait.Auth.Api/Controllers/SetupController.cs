using Maybe2.Configuration;
using System.Web.Http;

namespace Ait.Auth.Api.Controllers
{

    [RoutePrefix("api/Setup")]
    public class SetupController : ApiController
    {
        public IHttpActionResult Get(string id)
        {

            var sett = SettingsProvider.CreateProvider(id);

            var d = sett.LoadSettings();

            //sett.SaveSettings("dd".PairWith("!!!"), "dd".PairWith("@!!"));            

            return Json(d);
        }
    }
}