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
    [AllowAnonymous]
    public class HomeController : Controller
    {
        // Instanciation
        #region Instanciation
        IBookService _bookService;
        IAuthorService _authorService;
        IApplicationSettingsService _settingsService;
        IUserService _userService;
        IArticleService _articleService;
        HomeModel model = new HomeModel();
        public HomeController()
        {
            _bookService = new BookService(new eLibraryDatabaseEntities());
            _authorService = new AuthorService(new eLibraryDatabaseEntities());
            _articleService = new ArticleService(new eLibraryDatabaseEntities());
            _userService = new UserService(new eLibraryDatabaseEntities());
            _settingsService = new ApplicationSettingsService(new eLibraryDatabaseEntities());
        }
        public HomeController(BookService bookService, AuthorService authorService, ArticleService articleService, UserService userService, ApplicationSettingsService applicationSettingsService)
        {
            _authorService = authorService;
            _bookService = bookService;
            _userService = userService;
            _articleService = articleService;
            _settingsService = applicationSettingsService;
        }
        eLibraryDatabaseEntities db = new eLibraryDatabaseEntities();
        #endregion

        public ActionResult Index()
        {
            byte[] empty = { 4, 3 };
            var randomBooks = db.Books.Where(x => x.IsDeleted == false).Select(b => new BookVM()
            {
                Id = b.Id,
                Name = b.Name,
                Author = b.Author,
                Categories = b.Categories.Description,
                FrontCover = b.FrontCover == null ? empty : b.FrontCover,
                BackCover = b.BackCover == null ? empty : b.BackCover,
            }).OrderBy(o=> Guid.NewGuid()).ToList();
            model.NewRandowmBooks = randomBooks;

            var Featuredbooks = db.Books.Where(x => x.IsDeleted == false && x.IsFeatured == true).Select(b => new BookVM()
            {
                Id = b.Id,
                Name = b.Name,
                Author = b.Author,
                Categories = b.Categories.Description,
                FrontCover = b.FrontCover == null ? empty : b.FrontCover,
                BackCover = b.BackCover == null ? empty : b.BackCover,
            }).OrderBy(o => Guid.NewGuid()).ToList();
            model.FeaturedBooks = Featuredbooks;
            var articles = db.Articles.Where(x => x.IsDeleted == false && x.IsFeatured == true)
                       .Select(b => new ArticleVM()
                       {
                           Id = b.Id,
                           Description = b.Description,
                           Image = b.Image == null ? empty : b.Image,
                           SubCategory = b.SubCategory.Description
                       }).ToList();
            model.FeaturedArticles = articles;

            var categories = db.Categories.Where(x => x.IsDeleted == false).Select(b => new CategoriesVM()
            {
                Id = b.Id,
                ParentID = b.ParentID,
                Description = b.Description,
                ContentInformation = b.ContentInformation
            }).ToList();

            model.CategoryNav = categories;
            return View(model);

        }

        public ActionResult Search(string query)
        {
            ViewBag.Param = query;
            return View();
        }
        public ActionResult GetSearch(string query)
        {
            System.Threading.Thread.Sleep(5000);
            model.SearchedBooks = _bookService.GetSearch(query);
            model.SearchedArticles = _articleService.GetSearch(query);
            model.SearchCount = model.SearchedBooks.Count + model.SearchedArticles.Count;
            return PartialView("_Search", model);
        }

        public ActionResult SubCategory(int Id)
        {
            Global.SubCategoryID = Id;
            Global.CategoryID = (int)db.Categories.FirstOrDefault(x => x.Id == Id).ParentID;
            ViewBag.Category = db.Categories.FirstOrDefault(x => x.Id == Id).ParentDescription;
            ViewBag.SubCategory = db.Categories.FirstOrDefault(x => x.Id == Id).Description;

            return View();
        }
        public ActionResult GetBookUnderSubCategory(int Id)
        {
            model.Books = _bookService.GetSubCategoryBooks(Id);
            return PartialView("_CategorizedBooks", model);
        }

        public JsonResult AddToWishList(int bookID,string state)
        {
            string msg = "";
            var wishlist = new MemberWishListVM()
            {
                BookID = bookID,
                MemberID = Global.AuthenticatedMemberID
            };
            var check = _userService.CheckIfExistInWishList(wishlist);
            if (check)
            {
                var model = _userService.GetWishList(wishlist);
                switch (state)
                {
                    case "deactivate":
                        _userService.DeactivateFromWishList(model.Id);
                        msg = "Deactivated";
                        break;
                    case "activate":
                        _userService.ActivateFromWishList(model.Id);
                        msg = "Activated";
                        break;
                }
            }
            else
            {
                  _userService.AddToWishList(wishlist);
                msg = "Added";

            }
            return Json(msg, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddToCartList(int bookID)
        {
            var cartlist = new MemberCartVM()
            {
                BookID = bookID,
                MemberID = Global.AuthenticatedMemberID
            };
            var status = _userService.AddToCartList(cartlist);
            return Json(status, JsonRequestBehavior.AllowGet);
        }

    }
}