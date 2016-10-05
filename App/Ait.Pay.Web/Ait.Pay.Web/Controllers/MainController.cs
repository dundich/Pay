using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Ait.Pay.Web.Controllers
{
    public class MainController : Controller
    {

        readonly IPayConfig cfg;
        public MainController(IPayConfig cfg)
        {
            this.cfg = cfg;
        }


        public async Task<IDictionary<string, string>> GetSettings()
        {            
            return await cfg.GetSettings();
        }


        /// <summary>
        /// This maps to the Main/Index.cshtml file.  This file is the main view for the application.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> Index()
        {
            ViewBag.Settings = await GetSettings();
            return View();
        }
    }
}