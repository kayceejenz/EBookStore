using eLibrary.DAL.DataConnection;
using eLibrary.DAL.Entity;
using eLibrarySystem.Areas.Admin.Components;
using eLibrarySystem.Areas.Admin.Services;
using eLibrarySystem.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace eLibrarySystem.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        // Instantiation
        #region Instantiation
        private eLibraryDatabaseEntities db = new eLibraryDatabaseEntities();
        public readonly UserService _userService;
        public AccountController()
        {
            _userService = new UserService();
        }
        public AccountController(UserService userService)
        {
            _userService = userService;
        }
        #endregion
        // Action Methods
        #region Action Methods
        public ActionResult Login(bool? IsSuccess, string returnUrl)
        {
            Global.ReturnUrl = returnUrl;
            if (IsSuccess.Equals(true))
            {
                ViewBag.RegistrationSuccess = true;
            }
            return View();
        }
        [HttpPost]
        public ActionResult Login(UserVM userVM)
        {
            Session.Clear();
            var user = _userService.CheckCreditials(userVM);
            var member = _userService.CheckMemberCreditials(userVM);
            if (user.Count > 0)
            {
                LoginSession(user);
                if (Session["UserId"] != null)
                {
                    Global.AuthenticatedUserID = Convert.ToInt32(Session["UserId"].ToString());
                    var UserID = Convert.ToInt32(Session["UserId"]);
                    var RoleID = db.Users.Where(x => x.Id == UserID).FirstOrDefault().RoleID;
                    var PermissionList = db.RolePermissions.Where(x => x.RoleID == RoleID).ToList();
                    Nav.GetRolePermissionMenu(Nav.ApplicationMenu, PermissionList);
                }
                if(Global.ReturnUrl != null)
                {
                    return Redirect(Global.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Home", "Home", new { area = "Admin" });
                }
            }
            else if (member.Count > 0)
            {
                MemberLoginSession(member);
                if (Session["MemberId"] != null)
                {
                    Global.AuthenticatedMemberID = Convert.ToInt32(Session["MemberId"].ToString());

                }
                if (Global.ReturnUrl != null)
                {
                    return Redirect(Global.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Error = true;
            }
            return View(userVM);
        }
        [AllowAnonymous]
        public ActionResult Logout()
        {
            if (Session["UserId"] != null)
            {
                Session.Clear();
            }
            if (Session["MemberId"] != null)
            {
                Session.Clear();
            }
            return RedirectToAction("Login", "Account", new { area = "" });
        }
        public JsonResult CheckSessionExists()
        {
            bool IsExisting = false;
            if (Session["MemberId"] == null)
                IsExisting = false;
            else
                IsExisting = true;

            return Json(IsExisting, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(MemberVM vmodel)
        {
            bool state = false;
            if (ModelState.IsValid)
            {
                state = _userService.CreateMember(vmodel);
            }
            return RedirectToAction("Login", new { IsSuccess = state });
        }
        #endregion
        // Methods
        #region Methods
        public void LoginSession(List<User> user)
        {
            var Firstname = user.FirstOrDefault().Firstname;
            var Lastname = user.FirstOrDefault().Lastname;
            var DateCreated = user.FirstOrDefault().DateCreated;
            var Email = user.FirstOrDefault().Email;
            var Username = user.FirstOrDefault().Username;
            var ID = user.FirstOrDefault().Id;
            var Role = user.FirstOrDefault().Role.Description;

            var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, Firstname),
                    new Claim(ClaimTypes.Name, Lastname),
                    new Claim(ClaimTypes.DateOfBirth, DateCreated.ToString()),
                    new Claim(ClaimTypes.Email, Email),
                    new Claim(ClaimTypes.PrimarySid, ID.ToString()),
                    new Claim(ClaimTypes.Name, Username)
                },
                "ApplicationCookie");

            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignIn(identity);
            Session["UserId"] = ID.ToString();
            Session["Username"] = Username;
            Session["DateCreated"] = Convert.ToDateTime(DateCreated).ToLongDateString();
            Session["Name"] = Firstname + " " + Lastname;
            Session["Role"] = Role;
        }
        public void MemberLoginSession(List<Member> user)
        {
            var DateCreated = user.FirstOrDefault().DateCreated;
            var Email = user.FirstOrDefault().Email;
            var Name = user.FirstOrDefault().Name;
            var ID = user.FirstOrDefault().Id;

            var identity = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.DateOfBirth, DateCreated.ToString()),
                    new Claim(ClaimTypes.Email, Email),
                    new Claim(ClaimTypes.PrimarySid, ID.ToString()),
                    new Claim(ClaimTypes.Name, Name)
                },
                "ApplicationCookie");

            var ctx = Request.GetOwinContext();
            var authManager = ctx.Authentication;

            authManager.SignIn(identity);
            Session["MemberId"] = ID.ToString();
            Session["MemberName"] = Name;
            Session["MemberEmail"] = Email;
            Session["DateCreated"] = Convert.ToDateTime(DateCreated).ToLongDateString();
        }

        #endregion
    }
}