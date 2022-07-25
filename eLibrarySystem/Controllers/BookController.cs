using eLibrary.DAL.DataConnection;
using eLibrary.DAL.Entity;
using eLibrarySystem.Areas.Admin.Interfaces;
using eLibrarySystem.Areas.Admin.Services;
using eLibrarySystem.Areas.Admin.ViewModels;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLibrarySystem.Controllers
{
    public class BookController : Controller
    {
        #region Instanciation
        IBookService _bookService;
        IAuthorService _authorService;
        IApplicationSettingsService _settingsService;
        IUserService _userService;
        IArticleService _articleService;
        HomeModel model = new HomeModel();
        public BookController()
        {
            _bookService = new BookService(new eLibraryDatabaseEntities());
            _authorService = new AuthorService(new eLibraryDatabaseEntities());
            _articleService = new ArticleService(new eLibraryDatabaseEntities());
            _userService = new UserService(new eLibraryDatabaseEntities());
            _settingsService = new ApplicationSettingsService(new eLibraryDatabaseEntities());
        }
        public BookController(BookService bookService, AuthorService authorService, ArticleService articleService, UserService userService, ApplicationSettingsService applicationSettingsService)
        {
            _authorService = authorService;
            _bookService = bookService;
            _userService = userService;
            _articleService = articleService;
            _settingsService = applicationSettingsService;
        }
        eLibraryDatabaseEntities db = new eLibraryDatabaseEntities();
        #endregion


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
        // GET: Book
        [AllowAnonymous]
        public ActionResult ViewBook(int id)
        {
            ViewBag.Param = id;
            return View(_bookService.GetBook(id));
        }
        [AllowAnonymous]
        public PartialViewResult GetBook(int id)
        {
            System.Threading.Thread.Sleep(1000);
            var model = _bookService.GetBook(id);
            return PartialView("_ViewBook", model);
        }
        [AllowAnonymous]
        public ActionResult BookStore(int? page, int? categoryID)
        {
            if (categoryID == null || categoryID == 0)
            {
                model.BookCollections = GetRandomBooks(page);
            }
            else
            {
                Global.CategoryID = (int)categoryID;
                if(categoryID > 0)
                {
                    model.BookCollections = GetCategoryBooks(page, categoryID);
                }
            }
            model.CategoryNav = GetCategories();
            return View(model);
        }


        #region Services
        public IPagedList<BookVM> GetRandomBooks(int? page)
        {
            byte[] empty = { 4, 3 };
            var books = db.Books.Where(x => x.IsDeleted == false).Select(b => new BookVM()
            {
                Id = b.Id,
                Name = b.Name,
                Author = b.Author,
                Categories = b.Categories.Description,
                FrontCover = b.FrontCover == null ? empty : b.FrontCover,
                BackCover = b.BackCover == null ? empty : b.BackCover,
            }).OrderByDescending(x => x.Id).ToPagedList(page ?? 1, 10);

            return books;
        }
        public IPagedList<BookVM> GetCategoryBooks(int? page, int? categoryID)
        {
            byte[] empty = { 4, 3 };
            var books = db.Books.Where(x => x.IsDeleted == false && x.CategoryID == categoryID).Select(b => new BookVM()
            {
                Id = b.Id,
                Name = b.Name,
                Author = b.Author,
                Categories = b.Categories.Description,
                FrontCover = b.FrontCover == null ? empty : b.FrontCover,
                BackCover = b.BackCover == null ? empty : b.BackCover,
            }).OrderByDescending(x => x.Id).ToPagedList(page ?? 1, 10);

            return books;
        }

        public List<CategoriesVM> GetCategories()
        {
            var categories = db.Categories.Where(x => x.IsDeleted == false).Select(b => new CategoriesVM()
            {
                Id = b.Id,
                ParentID = b.ParentID,
                Description = b.Description,
                ContentInformation = b.ContentInformation
            }).ToList();
            return categories;
        }
        #endregion
    }
}