using eLibrary.DAL.DataConnection;
using eLibrary.DAL.Entity;
using eLibrarySystem.Areas.Admin.Interfaces;
using eLibrarySystem.Areas.Admin.Services;
using eLibrarySystem.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLibrarySystem.Areas.Admin.Controllers
{
    public class AuthorController : Controller
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


        // Initializing dependency services
        #region Instanciation
        IBookService _bookService;
        IAuthorService _authorService;
        public AuthorController()
        {
            _bookService = new BookService(new eLibraryDatabaseEntities());
            _authorService = new AuthorService(new eLibraryDatabaseEntities());
        }
        public AuthorController(BookService bookService, AuthorService authorService)
        {
            _authorService = authorService;
            _bookService = bookService;
        }
        eLibraryDatabaseEntities db = new eLibraryDatabaseEntities();
        #endregion

        public ActionResult Manage(bool? Added, bool? Editted, bool? Deleted)
        {
            if (Added == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Author added successfully.";
            }
            if (Editted == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Author updated successfully.";
            }
            if (Deleted == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Author deleted successfully.";
            }
            ViewBag.Authors = _authorService.GetAuthors();
            return View();
        }
        public ActionResult AddAuthor()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAuthor(AuthorVM vmodel, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (_authorService.AddAuthor(vmodel, image) == true)
                    return RedirectToAction("Manage", new { Added = true });

            }
            return View(vmodel);
        }
        public ActionResult EditAuthor(int id)
        {
            var model = _authorService.GetAuthor(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAuthor(AuthorVM vmodel, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (_authorService.UpdateAuthor(vmodel, image) == true)
                {
                    return RedirectToAction("Manage", new { Editted = true });
                }
            }
            return View(_authorService.GetAuthor(vmodel.Id));
        }
        public JsonResult GetAuthor(int id)
        {
            var model = _authorService.GetAuthor(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteAuthor(int id)
        {
            var status = _authorService.DeleteAuthor(id);
            return Json(status, JsonRequestBehavior.AllowGet);
        }
    }
}