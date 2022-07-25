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
    public class ArticleService : IArticleService
    {

        /* Instancation of the database context model
         * and injecting some buisness layer services
         */
        
        #region Instanciation
        readonly eLibraryDatabaseEntities _db;
        public ArticleService()
        {
            _db = new eLibraryDatabaseEntities();
        }
        public ArticleService(eLibraryDatabaseEntities db)
        {
            _db = db;
        }
        #endregion
        /* ******************************************************************************** */
        /* Article */
        // Fetching a article
        public List<ArticleVM> GetArticles()
        {
            var model = _db.Articles.Where(x => x.IsDeleted == false).Select(b => new ArticleVM()
            {
                Id = b.Id,
                Description = b.Description,
                Overview = b.Overview,
                Image = b.Image,
                SubCategoryID = b.SubCategoryID,
                SubCategory = b.SubCategory.Description,
                CategoryID = (int)b.SubCategory.ParentID,
                IsFeatured = b.IsFeatured
            }).ToList();
            return model;
        }

        // Creating an article
        public bool CreateArticle(ArticleVM Vmodel, HttpPostedFileBase ImageData)
        {
            bool hasSaved = false;
            Article model = new Article()
            {
                Description = Vmodel.Description,
                Overview = Vmodel.Overview,
                Image = CustomSerializer.Serialize(ImageData),
                SubCategoryID = Vmodel.SubCategoryID,
                DateCreated = DateTime.Now,
                IsDeleted = false,
                CreatedByID = Global.AuthenticatedUserID,
                IsFeatured = Vmodel.IsFeatured
            };
            _db.Articles.Add(model);
            _db.SaveChanges();

            // Add bonus
            var AfflilateBonus = _db.ApplicationSettings.FirstOrDefault().AfflilateArticleBonus;
            var AfflilateBonusRowExists = _db.AfflilateBonusManagers.Count(x => x.AfflilateUserID == Global.AuthenticatedUserID && x.BonusType == BonusType.Article);
            if (AfflilateBonusRowExists > 0)
            {
                var afflilateBonusRow = _db.AfflilateBonusManagers.FirstOrDefault(x => x.AfflilateUserID == Global.AuthenticatedUserID && x.BonusType == BonusType.Article);
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
                    BonusType = BonusType.Article,
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
        
        // Getting an article
        public ArticleVM GetArticle(int ID)
        {
            var model = _db.Articles.Where(x => x.Id == ID).Select(b => new ArticleVM()
            {
                Id = b.Id,
                Description = b.Description,
                Overview = b.Overview,
                Image = b.Image,
                SubCategoryID = b.SubCategoryID,
                IsFeatured = b.IsFeatured
            }).FirstOrDefault();
            model.ImageString = Convert.ToBase64String(model.Image);
            return model;
        }


        // Search articles
        public List<ArticleVM> SearchArticle(ArticleVM vmodel)
        {
            byte[] emptybyte = { 4, 3 };
            var model = _db.Articles.Where(x => (x.Description.StartsWith(vmodel.Description) || x.SubCategoryID == vmodel.SubCategoryID) && (x.IsDeleted == false)).Select(b => new ArticleVM()
            {
                Id = b.Id,
                Description = b.Description,
                Overview = b.Overview,
                Image = b.Image,
                SubCategoryID = b.SubCategoryID,
                SubCategory = b.SubCategory.Description,
                CategoryID = (int)b.SubCategory.ParentID,
                IsFeatured = b.IsFeatured
            }).ToList();
            return model;
        }

        // Books related to an article / course
        public List<BookVM> RelatedBooksForCourse(int CourseID)
        {
            byte[] emptybyte = { 4, 3 };
            var courseSubCategories = _db.Articles.FirstOrDefault(x => x.Id == CourseID).SubCategoryID;
            var model = _db.BookSubCategories.Where(x => x.SubCategoryID == courseSubCategories && x.IsDeleted == false).Select(b => new BookVM()
            {
                Id = b.Book.Id,
                Name = b.Book.Name,
                Author = b.Book.Author,
                ISBN = b.Book.ISBN,
                Categories = b.Book.Categories.Description,
                Edition = b.Book.Edition,
                Publisher = b.Book.Publisher,
                YearPublished = b.Book.YearPublished,
                BookFormat = b.Book.BookFormat,
                ContentFormat = b.Book.ContentFormat,
                Description = b.Book.Description,
                IsFeatured = b.Book.IsFeatured,
                BackCover = b.Book.BackCover == null ? emptybyte : b.Book.BackCover,
                FrontCover = b.Book.FrontCover == null ? emptybyte : b.Book.FrontCover
            }).ToList();
            return model;
        }

        // Editting and updating an article
        public bool EditArticle(ArticleVM Vmodel, HttpPostedFileBase ImageData)
        {
            bool hasSaved = false;
            Article model = _db.Articles.FirstOrDefault(x => x.Id == Vmodel.Id);
            model.Description = Vmodel.Description;
            if(ImageData != null)
                model.Image = CustomSerializer.Serialize(ImageData);
            model.Overview = Vmodel.Overview;
            model.SubCategoryID = Vmodel.SubCategoryID;
            model.IsFeatured = Vmodel.IsFeatured;
            model.EdittedUserID = Global.AuthenticatedUserID;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            hasSaved = true;
            return hasSaved;
        }

        // Deleting an article
        public bool DeleteArticle(int ID)
        {
            bool hasSaved = false;
            var model = _db.Articles.FirstOrDefault(x => x.Id == ID);
            model.IsDeleted = true;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            hasSaved = true;
            return hasSaved;
        }

        /* ************************************************************************************ */
        /* Article Content*/

        // Fetching the list of content for articles
        public List<ArticleContentVM> GetArticleContents(int ArticleID)
        {
            var model = _db.ArticleContents.Where(x => x.IsDeleted == false && x.ArticleID == ArticleID).Select(b => new ArticleContentVM()
            {
                Id = b.Id,
                Heading = b.Heading,
                Body = b.Body,
                ArticleID = b.ArticleID
            }).ToList();
            return model;
        }

        // Create content for articles
        public bool CreateArticleContent(ArticleContentVM Vmodel)
        {
            bool hasSaved = false;
            ArticleContent model = new ArticleContent()
            {
                Heading = Vmodel.Heading,
                Body = Vmodel.Body,
                ArticleID = Vmodel.ArticleID,
                DateCreated = DateTime.Now,
                IsDeleted = false,
                CreatedUserID = Global.AuthenticatedUserID,
            };
            _db.ArticleContents.Add(model);
            _db.SaveChanges();
            hasSaved = true;
            return hasSaved;
        }
        
        // Getting content for articles
        public ArticleContentVM GetArticleContent(int ID)
        {
            var model = _db.ArticleContents.Where(x => x.Id == ID).Select(b => new ArticleContentVM()
            {
                Id = b.Id,
                Heading = b.Heading,
                Body = b.Body,
                ArticleID = b.ArticleID
            }).FirstOrDefault();
            return model;
        }

        // Editting and updating content for an article
        public bool EditArticleContent(ArticleContentVM Vmodel)
        {
            bool hasSaved = false;
            ArticleContent model = _db.ArticleContents.FirstOrDefault(x => x.Id == Vmodel.Id);
            model.Heading = Vmodel.Heading;
            model.Body = Vmodel.Body;
            model.ArticleID = Vmodel.ArticleID;
            model.EdittedUserID = Global.AuthenticatedUserID;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            hasSaved = true;
            return hasSaved;
        }

        // Delete content of an article
        public bool DeleteArticleContent(int ID)
        {
            bool hasSaved = false;
            var model = _db.ArticleContents.FirstOrDefault(x => x.Id == ID);
            model.IsDeleted = true;
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            hasSaved = true;
            return hasSaved;
        }

        // Search for an article
        public List<ArticleVM> GetSearch(string query)
        {
            byte[] empty = { 4, 3 };
            var articles = _db.Articles.Where(x => x.Description.StartsWith(query) && x.IsDeleted == false)
                .Select(b => new ArticleVM()
                {
                    Id = b.Id,
                    Description = b.Description,
                    Image = b.Image == null ? empty : b.Image,
                    SubCategory = b.SubCategory.Description
                }).ToList();

            return articles;
        }

    }
}