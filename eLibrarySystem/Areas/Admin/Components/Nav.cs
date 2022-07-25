using eLibrary.DAL.DataConnection;
using eLibrary.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLibrarySystem.Areas.Admin.Components
{
    public static class Nav
    {
        private static eLibraryDatabaseEntities db = new eLibraryDatabaseEntities();
        private static string defaultIcon = "";
        public static List<Menu> AllMenus = new List<Menu>();
        public static List<Menu> ApplicationMenu = new List<Menu>()
        {
            // Dashboard
            new Menu(url: "/Admin/Dashboard/Analytics",stringText:"Dashboard", icon: "pe-7s-rocket",isMenu: false,childMenus: new List<Menu>()),
            
             // Articles
            new Menu(url: "#",stringText:"Article Management", icon: "pe-7s-news-paper", isMenu: true, childMenus: new List<Menu>(){
                new Menu(url: "/Admin/Article/ManageArticles",stringText:"Manage Articles", icon: null ?? defaultIcon, isMenu: true, childMenus: null)
            }),

              // Books
            new Menu(url: "#",stringText:"Books Management", icon: "pe-7s-notebook", isMenu: true, childMenus: new List<Menu>(){
                new Menu(url: "/Admin/Book/Manage",stringText:"Manage Books", icon: null ?? defaultIcon, isMenu: true, childMenus: null)
            }),

               // Authors
            new Menu(url: "#",stringText:"Authors Management", icon: "pe-7s-id", isMenu: true, childMenus: new List<Menu>(){
                new Menu(url: "/Admin/Author/Manage",stringText:"Manage Authors", icon: null ?? defaultIcon, isMenu: true, childMenus: null)
            }),

              // Authors
            new Menu(url: "#",stringText:"User Management", icon: "pe-7s-users", isMenu: true, childMenus: new List<Menu>(){
                new Menu(url: "/Admin/User/ManageMembers",stringText:"Manage Members", icon: null ?? defaultIcon, isMenu: true, childMenus: null),
                new Menu(url: "/Admin/User/Manage",stringText:"Manage Users", icon: null ?? defaultIcon, isMenu: true, childMenus: null),
                new Menu(url: "/Admin/User/ManageRoles",stringText:"Manage Roles", icon: null ?? defaultIcon, isMenu: true, childMenus: null),
                new Menu(url: "/Admin/User/ManageAfflilates",stringText:"Manage Afflilates", icon: null ?? defaultIcon, isMenu: true, childMenus: null),
            }),

            // Settings
            new Menu(url: "#",stringText:"Settings Managment", icon: "pe-7s-tools", isMenu: true, childMenus: new List<Menu>(){
                new Menu(url: "/Admin/ApplicationSettings/Manage",stringText:"Manage Settings", icon: null ?? defaultIcon, isMenu: true, childMenus: null),
                new Menu(url: "/Admin/ApplicationSettings/ManageCategories",stringText:"Manage Categories", icon: null ?? defaultIcon, isMenu: true, childMenus: null)
            }),
        };
        public static List<Menu> AppMenu = new List<Menu>()
        {
            // Dashboard
            new Menu(url: "/Admin/Dashboard/Analytics",stringText:"Dashboard", icon: "pe-7s-rocket",isMenu: false,childMenus: new List<Menu>()),
            
             // Articles
            new Menu(url: "#",stringText:"Article Management", icon: "pe-7s-news-paper", isMenu: true, childMenus: new List<Menu>(){
                new Menu(url: "/Admin/Article/ManageArticles",stringText:"Manage Articles", icon: null ?? defaultIcon, isMenu: true, childMenus: null)
            }),

              // Books
            new Menu(url: "#",stringText:"Books Management", icon: "pe-7s-notebook", isMenu: true, childMenus: new List<Menu>(){
                new Menu(url: "/Admin/Book/Manage",stringText:"Manage Books", icon: null ?? defaultIcon, isMenu: true, childMenus: null)
            }),

               // Authors
            new Menu(url: "#",stringText:"Authors Management", icon: "pe-7s-id", isMenu: true, childMenus: new List<Menu>(){
                new Menu(url: "/Admin/Author/Manage",stringText:"Manage Authors", icon: null ?? defaultIcon, isMenu: true, childMenus: null)
            }),

              // Authors
            new Menu(url: "#",stringText:"User Management", icon: "pe-7s-users", isMenu: true, childMenus: new List<Menu>(){
                new Menu(url: "/Admin/User/ManageMembers",stringText:"Manage Members", icon: null ?? defaultIcon, isMenu: true, childMenus: null),
                new Menu(url: "/Admin/User/Manage",stringText:"Manage Users", icon: null ?? defaultIcon, isMenu: true, childMenus: null),
                new Menu(url: "/Admin/User/ManageRoles",stringText:"Manage Roles", icon: null ?? defaultIcon, isMenu: true, childMenus: null),
                new Menu(url: "/Admin/User/ManageAfflilates",stringText:"Manage Afflilates", icon: null ?? defaultIcon, isMenu: true, childMenus: null),
            }),

            // Settings
            new Menu(url: "#",stringText:"Settings Managment", icon: "pe-7s-tools", isMenu: true, childMenus: new List<Menu>(){
                new Menu(url: "/Admin/ApplicationSettings/Manage",stringText:"Manage Settings", icon: null ?? defaultIcon, isMenu: true, childMenus: null),
                new Menu(url: "/Admin/ApplicationSettings/ManageCategories",stringText:"Manage Categories", icon: null ?? defaultIcon, isMenu: true, childMenus: null)
            }),
        };

        private static void GetAllMenu(List<Menu> menus, List<Menu> applicationMenus)
        {
            foreach (var menu in applicationMenus)
            {
                menus.Add(menu);
                if (menu._childMenus != null)
                {
                    GetAllMenu(AllMenus, menu._childMenus);
                }
            }
        }
        public static void UpdateAllMenus()
        {
            GetAllMenu(AllMenus, AppMenu);
        }
        public static void StorePermissions(List<Menu> menuViewModels)
        {
            List<Permission> model = new List<Permission>();

            foreach (var menu in menuViewModels)
            {
                var permission = new Permission()
                {
                    Description = menu._stringText,
                };
                model.Add(permission);
                if (menu._childMenus.Count > 0)
                {
                    foreach (var submenu in menu._childMenus)
                    {
                        var childpermission = new Permission()
                        {
                            Description = submenu._stringText,
                        };
                        model.Add(childpermission);
                    }
                }
            }
            db.Permissions.AddRange(model);
            db.SaveChanges();
        }

        public static void GetRolePermissionMenu(List<Menu> applicationMenus, List<RolePermission> permissionMenus)
        {
            foreach (var appmenu in applicationMenus)
            {
                var permissionRow = permissionMenus.FirstOrDefault(x => x.Permission == appmenu._stringText);

                appmenu.isAssigned = permissionRow == null ? false : permissionRow.IsAssigned;
                if (appmenu._childMenus.Count > 0)
                {
                    foreach (var subappmenu in appmenu._childMenus)
                    {
                        var permission = permissionMenus.FirstOrDefault(x => x.Permission == subappmenu._stringText);
                        if (permission != null)
                            subappmenu.isAssigned = permission.IsAssigned;
                    }
                    appmenu.isAssigned = appmenu._childMenus.Select(x => x.isAssigned).Contains(true);
                }
            }
        }
        public static List<Menu> GetAssignedPermission(int ID)
        {
            var CheckPermissionExists = db.RolePermissions.Where(x => x.RoleID == ID).Count();
            List<Menu> appmenus = new List<Menu>();
            appmenus = Nav.AppMenu;
            foreach (var menu in appmenus)
            {
                menu.isAssigned = db.RolePermissions.FirstOrDefault(x => x.Permission == menu._stringText && x.RoleID == ID).IsAssigned;
                if (menu._childMenus.Count > 0)
                {
                    foreach (var submenu in menu._childMenus)
                    {
                        submenu.isAssigned = db.RolePermissions.FirstOrDefault(x => x.Permission == submenu._stringText && x.RoleID == ID).IsAssigned;
                    }
                }
            }
            return appmenus;
        }

        public static void SavePermissionForRole(int roleID)
        {
            UpdateAllMenus();
            foreach (var menu in AllMenus)
            {
                if (CheckPermissionExist(roleID, menu._stringText) == 0)
                {
                    var rolePermission = new RolePermission();
                    rolePermission.PermissionID = db.Permissions.FirstOrDefault(x => x.Description == menu._stringText).Id;
                    rolePermission.Permission = menu._stringText;
                    rolePermission.RoleID = roleID;
                    rolePermission.IsAssigned = false;
                    rolePermission.IsDeleted = false;
                    db.RolePermissions.Add(rolePermission);
                }
                db.SaveChanges();
            }
        }
        public static int CheckPermissionExist(int RoleID, string Permission)
        {
            int result;
            result = db.RolePermissions.Where(x => x.RoleID == RoleID && x.Permission == Permission).Count();
            return result;
        }

        public static void AssignPermission(string permission, int roleID, bool isAssigned)
        {
            var RolePermission = db.RolePermissions.Where(p => p.RoleID == roleID && p.PermissionID == db.Permissions.FirstOrDefault(x => x.Description == permission).Id).FirstOrDefault();
            RolePermission.IsAssigned = isAssigned;
            db.Entry(RolePermission).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
        }

    }
}