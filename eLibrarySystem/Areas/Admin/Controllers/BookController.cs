using eLibrary.DAL.DataConnection;
using eLibrarySystem.Areas.Admin.Interfaces;
using eLibrarySystem.Areas.Admin.Services;
using eLibrarySystem.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using eLibrary.DAL.Entity;

namespace eLibrarySystem.Areas.Admin.Controllers
{
    public class BookController : Controller
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
        public BookController()
        {
            _bookService = new BookService(new eLibraryDatabaseEntities());
            _authorService = new AuthorService(new eLibraryDatabaseEntities());
        }
        public BookController(BookService bookService, AuthorService authorService)
        {
            _authorService = authorService;
            _bookService = bookService;
        }
        eLibraryDatabaseEntities db = new eLibraryDatabaseEntities();
        #endregion

        public ActionResult Manage(bool? Added,bool? Editted, bool? Deleted)
        {
            if (Added == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Book added successfully.";
            }
            if (Editted == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Book updated successfully.";
            }
            if (Deleted == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Book deleted successfully.";
            }
            ViewBag.Books = new List<BookVM>();
            ViewBag.Category = new SelectList(db.Categories.Where(x => x.IsDeleted == false && x.ParentID == null), "Id", "Description");
            return View();
        }
        [HttpPost]
        public ActionResult SearchBook(BookVM vmodel)
        {
            System.Threading.Thread.Sleep(1500);
            var model = _bookService.SearchBook(vmodel);
            ViewBag.Books = model;
            return PartialView("_SearchBook");
        }
        public ActionResult AddBook()
        {
            ViewBag.Category = new SelectList(db.Categories.Where(x => x.IsDeleted == false && x.ParentID == null), "Id", "Description");
            ViewBag.SubCategory = new SelectList(db.Categories.Where(x => x.IsDeleted == false && x.ParentID != null), "Id", "Description", "ParentDescription", true);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddBook(BookVM vmodel, HttpPostedFileBase FrontPage, HttpPostedFileBase BackPage)
        {
            if (ModelState.IsValid)
            {
                if (_bookService.AddBook(vmodel, FrontPage, BackPage) == true)
                    return RedirectToAction("Manage", new { Added = true });

            }
            ViewBag.Category = new SelectList(db.Categories.Where(x => x.IsDeleted == false && x.ParentID == null), "Id", "Description");
            ViewBag.SubCategory = new SelectList(db.Categories.Where(x => x.IsDeleted == false && x.ParentID != null), "Id", "Description", "ParentDescription", true);
            return View(vmodel);
        }
        public ActionResult EditBook(int id)
        {
            var model = _bookService.GetBook(id);
            ViewBag.Category = new SelectList(db.Categories.Where(x => x.IsDeleted == false && x.ParentID == null), "Id", "Description");
            ViewBag.SubCategory = _bookService.GetSelectedSubCategories(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditBook(BookVM vmodel, HttpPostedFileBase FrontPage, HttpPostedFileBase BackPage)
        {
            if (ModelState.IsValid)
            {
                if (_bookService.UpdateBook(vmodel, FrontPage, BackPage) == true)
                {
                    return RedirectToAction("Manage", new { Editted = true });
                }
            }
            ViewBag.Category = new SelectList(db.Categories.Where(x => x.IsDeleted == false && x.ParentID == null), "Id", "Description");
            ViewBag.SubCategory = _bookService.GetSelectedSubCategories(vmodel.Id);
            return View(_bookService.GetBook(vmodel.Id));
        }
        public JsonResult GetBook(int id)
        {
            var model = _bookService.GetBook(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteBook(int id)
        {
            var status = _bookService.DeleteBook(id);
            return Json(status, JsonRequestBehavior.AllowGet);
        }
    }
}