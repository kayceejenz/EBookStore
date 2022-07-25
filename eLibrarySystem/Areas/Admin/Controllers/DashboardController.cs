using eLibrary.DAL.DataConnection;
using eLibrary.DAL.Entity;
using eLibrarySystem.Areas.Admin.Interfaces;
using eLibrarySystem.Areas.Admin.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLibrarySystem.Areas.Admin.Controllers
{
    public class DashboardController : Controller
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
        IApplicationSettingsService _settingsService;
        IUserService _userService;
        IArticleService _articleService;
        public DashboardController()
        {
            _bookService = new BookService(new eLibraryDatabaseEntities());
            _authorService = new AuthorService(new eLibraryDatabaseEntities());
            _articleService = new ArticleService(new eLibraryDatabaseEntities());
            _userService = new UserService(new eLibraryDatabaseEntities());
            _settingsService = new ApplicationSettingsService(new eLibraryDatabaseEntities());
        }
        public DashboardController(BookService bookService, AuthorService authorService, ArticleService articleService, UserService userService, ApplicationSettingsService applicationSettingsService)
        {
            _authorService = authorService;
            _bookService = bookService;
            _userService = userService;
            _articleService = articleService;
            _settingsService = applicationSettingsService;
        }
        eLibraryDatabaseEntities db = new eLibraryDatabaseEntities();
        #endregion
        public ActionResult Analytics()
        {
            ViewBag.Books = db.Books.Count(x => x.IsDeleted == false);
            ViewBag.Authors = db.Authors.Count(x => x.IsDeleted == false);
            ViewBag.Articles = db.Articles.Count(x => x.IsDeleted == false);
            ViewBag.Users = db.Users.Count(x => x.IsDeleted == false);
            ViewBag.ActiveUsers = db.Users.Count(x => x.IsDeleted == false && x.IsActive == true);
            ViewBag.InActiveUsers = db.Users.Count(x => x.IsDeleted == false && x.IsActive == false);
            ViewBag.Categories = db.Categories.Count(x => x.IsDeleted == false && x.ParentID == null);
            ViewBag.SubCategories = db.Categories.Count(x => x.IsDeleted == false && x.ParentID != null);


            // Chart
            List<int> repartition = new List<int>();
            int[] amList = { 500, 1000, 2000, 5000, 10000, 50000, 100000, 150000 };
            repartition.AddRange(amList);

            var amount = db.AfflilateBonusManagers.Where(x => x.IsDeleted == false).Select(x => new AfflilateBonusManager { Amount = x.Amount });

            ViewBag.Amount = amount;
            ViewBag.Rep = repartition;
            return View();
        }
    }
}