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
    public class AuthorService : IAuthorService
    {
        /* Instancation of the database context model
         * and injecting some buisness layer services
         */
        #region Instanciation
        readonly eLibraryDatabaseEntities _db;
        public AuthorService()
        {
            _db = new eLibraryDatabaseEntities();
        }
        public AuthorService(eLibraryDatabaseEntities db)
        {
            _db = db;
        }
        #endregion

        /* **************************************************************************** */
        // Fetching the authors
        public List<AuthorVM> GetAuthors()
        {
            var model = _db.Authors.Where(x => x.IsDeleted == false).Select(b => new AuthorVM()
            {
                Id = b.Id,
                Name = b.Name
            }).ToList();
            return model;
        }

        // Creating an author
        public bool AddAuthor(AuthorVM vmodel, HttpPostedFileBase image)
        {
            bool hasSaved = false;

            Author model = new Author()
            {
                Name = vmodel.Name,
                DateOfBirth = vmodel.DateOfBirth,
                Biography = vmodel.Biography,
                StateOfOrigin = vmodel.StateOfOrigin,
                ContactAddress = vmodel.ContactAddress,
                CountryOfOrigin = vmodel.CountryOfOrigin,
                IsDeleted = false,
                DateCreated = DateTime.Now,
                CreatedByID = Global.AuthenticatedUserID
            };
            if(image != null) 
                model.Photo = CustomSerializer.Serialize(image);

            _db.Authors.Add(model);
            _db.SaveChanges();

            // Add bonus
            var AfflilateBonus = _db.ApplicationSettings.FirstOrDefault().AfflilateAuthorBonus;
            var AfflilateBonusRowExists = _db.AfflilateBonusManagers.Count(x => x.AfflilateUserID == Global.AuthenticatedUserID && x.BonusType == BonusType.Author);
            if (AfflilateBonusRowExists > 0)
            {
                var afflilateBonusRow = _db.AfflilateBonusManagers.FirstOrDefault(x => x.AfflilateUserID == Global.AuthenticatedUserID && x.BonusType == BonusType.Author);
                afflilateBonusRow.Amount += AfflilateBonus;
                afflilateBonusRow.LastModified = DateTime.Now;
                _db.Entry(afflilateBonusRow).State = System.Data.Entity.EntityState.Modified;
                _db.SaveChanges();
            }
            else
            {
                var afflilateBonusRow = new AfflilateBonusManager()
                {
                    AfflilateUserID = Global.AuthenticatedUserID,
                    BonusType = BonusType.Author,
                    IsDeleted = false,
                    DateCreated = DateTime.Now,
                    Amount = AfflilateBonus
                };
                _db.AfflilateBonusManagers.Add(afflilateBonusRow);
                _db.SaveChanges();
            }

            hasSaved = true;
            return hasSaved;
        }

        // Getting an author
        public AuthorVM GetAuthor(int id)
        {
            byte[] empty = { 4, 3 };
            var model = _db.Authors.Where(x => x.Id == id).Select(b => new AuthorVM()
            {
                Id = b.Id,
                Name = b.Name,
                Biography = b.Biography,
                DateOfBirth = b.DateOfBirth,
                ContactAddress = b.ContactAddress,
                CountryOfOrigin = b.CountryOfOrigin,
                StateOfOrigin = b.StateOfOrigin,
                Photo = b.Photo == null ? empty: b.Photo
            }).FirstOrDefault();
            model.ImageString = Convert.ToBase64String(model.Photo);
            return model;
        }

        // Editting and updating an author
        public bool UpdateAuthor(AuthorVM vmodel, HttpPostedFileBase image)
        {
            bool hasUpdated = false;

            var model = _db.Authors.Where(x => x.Id == vmodel.Id).FirstOrDefault();
            model.Name = vmodel.Name;
            model.Biography = vmodel.Biography;
            model.ContactAddress = vmodel.ContactAddress;
            model.CountryOfOrigin = vmodel.CountryOfOrigin;
            model.DateOfBirth = vmodel.DateOfBirth;
            model.StateOfOrigin = vmodel.StateOfOrigin;
            model.LastModified = DateTime.Now;

            if (image != null)
                model.Photo = CustomSerializer.Serialize(image);

            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            hasUpdated = true;

            return hasUpdated;
        }

        // Deleting an author
        public bool DeleteAuthor(int id)
        {
            var author = _db.Authors.FirstOrDefault(x => x.Id == id);
            author.IsDeleted = true;
            _db.Entry(author).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            return true;
        }
    }
}