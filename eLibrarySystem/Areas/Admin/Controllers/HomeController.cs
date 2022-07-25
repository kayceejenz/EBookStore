using eLibrary.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLibrarySystem.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            filterContext.ExceptionHandled = true;

            //Log the error!!
            log.Error("Error trying to do something", filterContext.Exception);
            Global.ErrorMessage = filterContext.Exception.ToString();

            //Redirect or return a view, but not both.
            filterContext.Result = RedirectToAction("Error", "Error", new { area = "" });

        }


        public ActionResult Home()
        {
            return View();
        }
    }
}