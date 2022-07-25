using eLibrary.DAL.DataConnection;
using eLibrary.DAL.Entity;
using eLibrarySystem.Areas.Admin.Components;
using eLibrarySystem.Areas.Admin.Helpers;
using eLibrarySystem.Areas.Admin.Interfaces;
using eLibrarySystem.Areas.Admin.Services;
using eLibrarySystem.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace eLibrarySystem.Areas.Admin.Controllers
{
    public class UserController : Controller
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
        eLibraryDatabaseEntities db = new eLibraryDatabaseEntities();
        IUserService _userService;
        public UserController()
        {
            _userService = new UserService(new eLibraryDatabaseEntities());
        }
        public UserController(UserService userService)
        {
            _userService = userService;
        }
        #endregion

        public ActionResult Manage(bool? Added,bool? Editted)
        {
            if (Added == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "User added successfully.";
            }
            if (Editted == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "User updated successfully.";
            }
            ViewBag.Roles = new SelectList(db.Roles.Where(x => x.IsDeleted == false), "Id", "Description");
            ViewBag.Users = _userService.GetUsers();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateUser(UserVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _userService.CreateUser(vmodel);
            }
            return RedirectToAction("Manage", new { Added = hasSaved });
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditUser(UserVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _userService.EditUser(vmodel);
            }
            return RedirectToAction("Manage", new { Editted = hasSaved });
        }
        public JsonResult GetUser(int id)
        {
            var model = _userService.GetUser(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteUser(int id)
        {
            var model = _userService.DeleteUser(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeactivateUser(int id)
        {
            var model = _userService.DeactivateUser(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ActivateUser(int id)
        {
            var model = _userService.ActivateUser(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        // Role
        public ActionResult ManageRoles(bool? Added,bool? Editted)
        {
            if (Added == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Role added successfully.";
            }
            if (Editted == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Role updated successfully.";
            }
            ViewBag.Roles = _userService.GetRoles();
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult CreateRole(RoleVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _userService.CreateRole(vmodel);
            }
            return RedirectToAction("ManageRoles", new { Added = hasSaved });
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult EditRole(RoleVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                hasSaved = _userService.EditRole(vmodel);
            }
            return RedirectToAction("ManageRoles", new { Editted = hasSaved });
        }
        public JsonResult GetRole(int id)
        {
            var model = _userService.GetRole(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteRole(int id)
        {
            var model = _userService.DeleteRole(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ManagePermissions(int RoleID, bool? Saved)
        {
            if (Saved == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertMessage"] = "Permissions updated successfully";
                TempData["AlertType"] = "alert-success";
            }
            Nav.SavePermissionForRole(RoleID);
            ViewBag.RoleName = db.Roles.FirstOrDefault(x => x.Id == RoleID).Description;
            ViewBag.ApplicationMenus = Nav.GetAssignedPermission(RoleID);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult ManagePermissions(string selectedItems, string role)
        {
            var roleID = db.Roles.FirstOrDefault(x => x.Description == role).Id;
            bool saved = false;
            if (ModelState.IsValid)
            {
                List<Menu> selectedPermissions = (new JavaScriptSerializer()).Deserialize<List<Menu>>(selectedItems);
                foreach (var each in selectedPermissions)
                {
                    each._stringText = HtmlConverter.RemoveHTMLTags(each._stringText);
                }
                var menus = Nav.AllMenus;
                foreach (var menu in menus)
                {
                    menu.isAssigned = selectedPermissions.Select(b => b._stringText).Contains(menu._stringText);
                    Nav.AssignPermission(menu._stringText, roleID, menu.isAssigned);
                }
            }
            saved = true;
            return ManagePermissions(roleID, saved);
        }

        public ActionResult ManageAfflilates()
        {
            ViewBag.BookBonusSummary = new List<BookVM>();
            ViewBag.AuthorBounsSummary = new List<AuthorVM>();
            ViewBag.ArticleBounsSummary = new List<ArticleVM>();
            var model = _userService.GetAfflilate_Bouns();
            return View(model);
        }

        [HttpPost]
        public ActionResult BookBonusSummary(int afflilateID)
        {
            System.Threading.Thread.Sleep(1500);
            ViewBag.BookBonusSummary = _userService.GetBooksAddedByAfflilate(afflilateID);
            return PartialView("_BookBonusSummary");
        }
        [HttpPost]
        public ActionResult ArticleBonusSummary(int afflilateID)
        {
            System.Threading.Thread.Sleep(1500);
            ViewBag.ArticleBounsSummary = _userService.GetArticleAddedByAfflilate(afflilateID);
            return PartialView("_ArticleBonusSummary");
        }
        [HttpPost]
        public ActionResult AuthorBonusSummary(int afflilateID)
        {
            System.Threading.Thread.Sleep(1500);
            ViewBag.AuthorBounsSummary = _userService.GetAuthorAddedByAfflilate(afflilateID);
            return PartialView("_AuthorBonusSummary");
        }

        public ActionResult ManageMembers()
        {
            return View(_userService.GetMembers());
        }
    }
}