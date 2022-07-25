using eLibrarySystem.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace eLibrarySystem.Areas.Admin.Interfaces
{
    interface IApplicationSettingsService
    {
        // Application settings
        ApplicationSettingsVM GetApplicationSettings();
        bool UpdateApplicationSettings(ApplicationSettingsVM Vmodel, HttpPostedFileBase Logo, HttpPostedFileBase Favicon);

        // Categories
        List<CategoriesVM> GetCategories();
        bool CreateCategory(CategoriesVM Vmodel);
        CategoriesVM GetCategory(int ID);
        bool EditCategory(CategoriesVM Vmodel);
        bool DeleteCategory(int ID);

        // Sub categories
        List<CategoriesVM> GetSubCategories();
        bool CreateSubCategory(CategoriesVM Vmodel);
        CategoriesVM GetSubCategory(int ID);
        bool EditSubCategory(CategoriesVM Vmodel);
        bool DeleteSubCategory(int ID);
    }
}
