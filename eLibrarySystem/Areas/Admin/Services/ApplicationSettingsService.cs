using eLibrary.DAL.DataConnection;
using eLibrary.DAL.Entity;
using eLibrarySystem.Areas.Admin.Helpers;
using eLibrarySystem.Areas.Admin.Interfaces;
using eLibrarySystem.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLibrarySystem.Areas.Admin.Services
{
    public class ApplicationSettingsService : IApplicationSettingsService
    {
        /* Instancation of the database context model
         * and injecting some buisness layer services
         */
        #region Instanciation
        readonly eLibraryDatabaseEntities _db;
        public ApplicationSettingsService()
        {
            _db = new eLibraryDatabaseEntities();
        }
        public ApplicationSettingsService(eLibraryDatabaseEntities db)
        {
            _db = db;
        }
        #endregion

        /* ********************************************************************************************************** */
        // Application settings

        // Getting the basic system application setting data
        public ApplicationSettingsVM GetApplicationSettings()
        {
            byte[] emptyArr = { 4, 3 };
            var model = _db.ApplicationSettings.FirstOrDefault();
            var Vmodel = new ApplicationSettingsVM()
            {
                ID = model.Id,
                AppName = model.AppName,
                Logo = model.Logo == null ? emptyArr : model.Logo,
                Favicon = model.Favicon == null ? emptyArr : model.Favicon,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                LinkedInHandle = model.LinkedInHandle,
                FacebookHandle = model.FacebookHandle,
                InstagramHandle = model.InstagramHandle,
                TwitterHandle = model.TwitterHandle,
                MemberOvertimeDebtString = model.MemberOvertimeDebt.ToString(),
                AfflilateArticleBonusString = model.AfflilateArticleBonus.ToString(),
                AfflilateAuthorBonusString = model.AfflilateAuthorBonus.ToString(),
                AfflilateBookBonusString = model.AfflilateBookBonus.ToString()
            };
            return Vmodel;
        }

        // Editting and updating the system application setting data
        public bool UpdateApplicationSettings(ApplicationSettingsVM Vmodel, HttpPostedFileBase Logo, HttpPostedFileBase Favicon)
        {
            bool hasSucceed = false;
            var model = _db.ApplicationSettings.FirstOrDefault(x => x.Id == Vmodel.ID);
            model.AppName = Vmodel.AppName;
            if (Logo != null)
                model.Logo = CustomSerializer.Serialize(Logo);
            if (Favicon != null)
                model.Favicon = CustomSerializer.Serialize(Favicon);
            model.Address = Vmodel.Address;
            model.PhoneNumber = Vmodel.PhoneNumber;
            model.MemberOvertimeDebt = CustomSerializer.UnMaskString(Vmodel.MemberOvertimeDebtString);
            model.InstagramHandle = Vmodel.InstagramHandle;
            model.FacebookHandle = Vmodel.FacebookHandle;
            model.TwitterHandle = Vmodel.TwitterHandle;
            model.LinkedInHandle = Vmodel.LinkedInHandle;
            model.AfflilateBookBonus = CustomSerializer.UnMaskString(Vmodel.AfflilateBookBonusString);
            model.AfflilateAuthorBonus = CustomSerializer.UnMaskString(Vmodel.AfflilateAuthorBonusString);
            model.AfflilateArticleBonus = CustomSerializer.UnMaskString(Vmodel.AfflilateArticleBonusString);
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            var state = _db.SaveChanges();
            if (state > 0)
            {
                hasSucceed = true;
            }
            return hasSucceed;
        }

        /********************************************************************************************** */
        // Categories


        // Fetching content categories
        public List<CategoriesVM> GetCategories()
        {
            var model = _db.Categories.Where(x => x.IsDeleted == false && x.ParentID == null).Select(b => new CategoriesVM()
            {
                Id = b.Id,
                Description = b.Description,
                ContentInformation = b.ContentInformation,
            }).ToList();
            return model;
        }

        // Creating content category
        public bool CreateCategory(CategoriesVM Vmodel)
        {
            bool hasSaved = false;
            Categories model = new Categories()
            {
                Description = Vmodel.Description,
                DateCreated = DateTime.Now,
                IsDeleted = false,
                ContentInformation = Vmodel.ContentInformation,
            };
            _db.Categories.Add(model);
            _db.SaveChanges();
            hasSaved = true;
            return hasSaved;
        }

        // Getting content category
        public CategoriesVM GetCategory(int ID)
        {
            var model = _db.Categories.Where(x => x.Id == ID).Select(b => new CategoriesVM()
            {
                Id = b.Id,
                Description = b.Description,
                ContentInformation = b.ContentInformation,
            }).FirstOrDefault();
            return model;
        }

        // Editting and updating content category
        public bool EditCategory(CategoriesVM Vmodel)
        {
            bool hasSaved = false;
            Categories model = _db.Categories.FirstOrDefault(x => x.Id == Vmodel.Id);
            model.Description = Vmodel.Description;
            model.ContentInformation = Vmodel.ContentInformation;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            hasSaved = true;
            return hasSaved;
        }

        // Deleting content category
        public bool DeleteCategory(int ID)
        {
            bool hasSaved = false;
            var model = _db.Categories.FirstOrDefault(x => x.Id == ID);
            model.IsDeleted = true;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            hasSaved = true;
            return hasSaved;
        }

        /* ************************************************************************************************* */
        // Sub categories

        // Fetching the sub content categories
        public List<CategoriesVM> GetSubCategories()
        {
            var model = _db.Categories.Where(x => x.IsDeleted == false && x.ParentID != null).Select(b => new CategoriesVM()
            {
                Id = b.Id,
                Description = b.Description,
                ContentInformation = b.ContentInformation,
                Parent = b.Parent.Description,
                ParentID = b.ParentID
            }).ToList();
            return model;
        }

        // Creating sub content category
        public bool CreateSubCategory(CategoriesVM Vmodel)
        {
            bool hasSaved = false;
            Categories model = new Categories()
            {
                Description = Vmodel.Description,
                DateCreated = DateTime.Now,
                IsDeleted = false,
                ContentInformation = Vmodel.ContentInformation,
                ParentID = Vmodel.ParentID,
                ParentDescription = _db.Categories.FirstOrDefault(x=>x.Id == Vmodel.ParentID).Description
            };
            _db.Categories.Add(model);
            _db.SaveChanges();
            hasSaved = true;
            return hasSaved;
        }

        // Getting sub content category
        public CategoriesVM GetSubCategory(int ID)
        {
            var model = _db.Categories.Where(x => x.Id == ID).Select(b => new CategoriesVM()
            {
                Id = b.Id,
                Description = b.Description,
                ContentInformation = b.ContentInformation,
                ParentID = b.ParentID
            }).FirstOrDefault();
            return model;
        }

        // Editting and updating sub content category
        public bool EditSubCategory(CategoriesVM Vmodel)
        {
            bool hasSaved = false;
            Categories model = _db.Categories.FirstOrDefault(x => x.Id == Vmodel.Id);
            model.Description = Vmodel.Description;
            model.ContentInformation = Vmodel.ContentInformation;
            model.ParentID = Vmodel.ParentID;
            model.ParentDescription = _db.Categories.FirstOrDefault(x => x.Id == Vmodel.ParentID).Description;

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            hasSaved = true;
            return hasSaved;
        }

        // Deleting sub content category
        public bool DeleteSubCategory(int ID)
        {
            bool hasSaved = false;
            var model = _db.Categories.FirstOrDefault(x => x.Id == ID);
            model.IsDeleted = true;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            hasSaved = true;
            return hasSaved;
        }
    }
}