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
    public class ArticleController : Controller
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
        IApplicationSettingsService _settingService;
        IArticleService _articleService;
        public ArticleController()
        {
            _settingService = new ApplicationSettingsService(new eLibraryDatabaseEntities());
            _articleService = new ArticleService(new eLibraryDatabaseEntities());
        }
        public ArticleController(ApplicationSettingsService settingsService, ArticleService articleService)
        {
            _settingService = settingsService;
            _articleService = articleService;
        }
        eLibraryDatabaseEntities db = new eLibraryDatabaseEntities();
        #endregion

        // Article
        public ActionResult ManageArticles(bool? Added,bool? Editted)
        {
            if (Added == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Article added successfully.";
            }
            if (Editted == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Article updated successfully.";
            }
            ViewBag.SubCategories = new SelectList(db.Categories.Where(x => x.IsDeleted == false && x.ParentID != null), "Id", "Description", "ParentDescription", true);
            ViewBag.Articles = _articleService.GetArticles();
            return View();
        }

        [HttpPost]
        public ActionResult SearchArticle(ArticleVM vmodel)
        {
            System.Threading.Thread.Sleep(1500);
            var model = _articleService.SearchArticle(vmodel);
            ViewBag.Articles = model;
            return PartialView("_Search");
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateArticle(ArticleVM vmodel, HttpPostedFileBase ImageData)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _articleService.CreateArticle(vmodel,ImageData);
            }
            return RedirectToAction("ManageArticles", new { Added = hasSaved });
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditArticle(ArticleVM vmodel, HttpPostedFileBase ImageData)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _articleService.EditArticle(vmodel,ImageData);
            }
            return RedirectToAction("ManageArticles", new { Editted = hasSaved });
        }
        public JsonResult GetArticle(int id)
        {
            var model = _articleService.GetArticle(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteArticle(int id)
        {
            var model = _articleService.DeleteArticle(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // Categories
        public ActionResult ManageArticleContents(bool? Saved, int ArticleID)
        {
            if (Saved == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Save successfully.";
            }
            ViewBag.Article = _articleService.GetArticle(ArticleID);
            ViewBag.ArticleContents = _articleService.GetArticleContents(ArticleID);
            return View();
        }
        public ActionResult CreateArticleContent(int ArticleID)
        {
            ViewBag.Article = _articleService.GetArticle(ArticleID);
            var model = new ArticleContentVM();
            model.ArticleID = ArticleID;
            return View(model);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateArticleContent(ArticleContentVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _articleService.CreateArticleContent(vmodel);
            }
            return RedirectToAction("ManageArticleContents", new { Saved = hasSaved, ArticleID = vmodel.ArticleID });
        }
        public ActionResult EditArticleContent(int Id, int ArticleID)
        {
            ViewBag.Article = _articleService.GetArticle(ArticleID);
            var model = _articleService.GetArticleContent(Id);
            return View(model);
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditArticleContent(ArticleContentVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _articleService.EditArticleContent(vmodel);
            }
            return RedirectToAction("ManageArticleContents", new { Saved = hasSaved, ArticleID = vmodel.ArticleID });
        }
        public JsonResult DeleteArticleContent(int id)
        {
            var model = _articleService.DeleteArticleContent(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}