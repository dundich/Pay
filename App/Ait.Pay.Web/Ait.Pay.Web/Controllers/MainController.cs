using Maybe2.Configuration;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Ait.Pay.Web.Controllers
{
    public class MainController : Controller
    {

        readonly IShellSettings cfg;
        public MainController(IShellSettings cfg)
        {
            this.cfg = cfg;
        }


        public IDictionary<string, string> GetSettings()
        {            
            return cfg.GetSettings();
        }


        /// <summary>
        /// This maps to the Main/Index.cshtml file.  This file is the main view for the application.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Settings = GetSettings();
            return View();
        }
    }
}