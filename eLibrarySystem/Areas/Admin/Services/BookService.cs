using eLibrary.DAL.DataConnection;
using eLibrary.DAL.Entity;
using eLibrarySystem.Areas.Admin.Helpers;
using eLibrarySystem.Areas.Admin.Interfaces;
using eLibrarySystem.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLibrarySystem.Areas.Admin.Services
{
    public class BookService : IBookService
    {
        /* Instancation of the database context model
         * and injecting some buisness layer services
         */
        #region Instanciation
        readonly eLibraryDatabaseEntities _db;
        public BookService()
        {
            _db = new eLibraryDatabaseEntities();
        }
        public BookService(eLibraryDatabaseEntities db)
        {
            _db = db;
        }
        #endregion

        /* ********************************************************************************* */
        // Book

        // Search books
        public List<BookVM> SearchBook(BookVM vmodel)
        {
            byte[] emptybyte = { 4, 3 };
            var model = _db.Books.Where(x => (x.Name == vmodel.Name || x.Author == vmodel.Author || x.CategoryID == vmodel.CategoryID || x.ISBN == vmodel.ISBN) && (x.IsDeleted == false)).Select(b => new BookVM()
            {
                Id = b.Id,
                Name = b.Name,
                Author = b.Author,
                ISBN = b.ISBN,
                Categories = b.Categories.Description,
                Edition = b.Edition,
                Publisher = b.Publisher,
                YearPublished = b.YearPublished,
                BookFormat = b.BookFormat,
                ContentFormat = b.ContentFormat,
                Description = b.Description,
                IsFeatured = b.IsFeatured,
                BackCover = b.BackCover == null ? emptybyte : b.BackCover,
                FrontCover = b.FrontCover == null ? emptybyte : b.FrontCover
            }).ToList();
            return model;
        }

        // Adding books
        public bool AddBook(BookVM vmodel, HttpPostedFileBase FrontPage, HttpPostedFileBase BackPage)
        {
            bool hasSaved = false;
            Book model = new Book()
            {
                Name = vmodel.Name,
                ISBN = vmodel.ISBN,
                Description = vmodel.Description,
                CategoryID = vmodel.CategoryID,
                Author = vmodel.Author,
                YearPublished = vmodel.YearPublished,
                UniqueNumber = Guid.NewGuid(),
                BookFormat = vmodel.BookFormat,
                ContentFormat = vmodel.ContentFormat,
                Edition = vmodel.Edition,
                Publisher = vmodel.Publisher,
                DateCreated = DateTime.Now,
                IsFeatured =  vmodel.IsFeatured,
                IsDeleted = false,
                CreatedByID = Global.AuthenticatedUserID
            };
            if (FrontPage != null)
                model.FrontCover = CustomSerializer.Serialize(FrontPage);
            if (BackPage != null)
                model.BackCover = CustomSerializer.Serialize(BackPage);

            // Save
            _db.Books.Add(model);
            _db.SaveChanges();

            // Save Book Sub Categories
            if (vmodel.SubCategories.Count() > 0)
                AddBookSubCategories(model.Id, vmodel.SubCategories);

            // Add bonus
            var AfflilateBonus = _db.ApplicationSettings.FirstOrDefault().AfflilateBookBonus;
            var AfflilateBonusRowExists = _db.AfflilateBonusManagers.Count(x => x.AfflilateUserID == Global.AuthenticatedUserID && x.BonusType == BonusType.Book);
            if(AfflilateBonusRowExists > 0)
            {
                var afflilateBonusRow = _db.AfflilateBonusManagers.FirstOrDefault(x => x.AfflilateUserID == Global.AuthenticatedUserID && x.BonusType == BonusType.Book);
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
                    BonusType = BonusType.Book,
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
        
        // Getting books
        public BookVM GetBook(int id)
        {
            byte[] emptybyte = { 4, 3 };
            var model = _db.Books.Where(x => x.Id == id).Select(b => new BookVM()
            {
                Id = b.Id,
                Name = b.Name,
                Description = b.Description,
                ContentFormat = b.ContentFormat,
                BookFormat = b.BookFormat,
                Categories = b.Categories.Description,
                CategoryID = b.CategoryID,
                Author = b.Author,
                ISBN = b.ISBN,
                Edition = b.Edition,
                Publisher = b.Publisher,
                YearPublished = b.YearPublished,
                IsFeatured = b.IsFeatured,
                BackCover = b.BackCover == null ? emptybyte : b.BackCover,
                FrontCover = b.FrontCover == null ? emptybyte : b.FrontCover
            }).FirstOrDefault();
            return model;
        }

        // Editting and updating books
        public bool UpdateBook(BookVM vmodel, HttpPostedFileBase FrontPage, HttpPostedFileBase BackPage)
        {
            bool hasUpdated = false;

            Book model = _db.Books.FirstOrDefault(x => x.Id == vmodel.Id);
            model.Name = vmodel.Name;
            model.ISBN = vmodel.ISBN;
            model.Description = vmodel.Description;
            model.Edition = vmodel.Edition;
            model.Publisher = vmodel.Publisher;
            model.CategoryID = vmodel.CategoryID;
            model.ContentFormat = vmodel.ContentFormat;
            model.BookFormat = vmodel.BookFormat;
            model.Author = vmodel.Author;
            model.YearPublished = vmodel.YearPublished;
            model.IsFeatured = vmodel.IsFeatured;
            model.LastModified = DateTime.Now;
            if (FrontPage != null)
                model.FrontCover = CustomSerializer.Serialize(FrontPage);
            if (BackPage != null)
                model.BackCover = CustomSerializer.Serialize(BackPage);
            _db.Entry(model).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();

            // Remove all existing sub categories
            RemoveAllSubCategories(vmodel.Id,vmodel.SubCategories);

            // Add new selected sub category
            if(vmodel.SubCategories != null)
                AddBookSubCategories(model.Id, vmodel.SubCategories);

            hasUpdated = true;
            return hasUpdated;
        }
        
        // Deleting books
        public bool DeleteBook(int id)
        {
            var book = _db.Books.FirstOrDefault(x => x.Id == id);
            book.IsDeleted = true;
            _db.Entry(book).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
            return true;
        }

        // Creating book sub categories tagged
        public void AddBookSubCategories(int bookID, int[] SubCategoriesID)
        {
            foreach (var subcategoryID in SubCategoriesID)
            {
                if(_db.BookSubCategories.Count(x=>x.BookID ==bookID && x.SubCategoryID == subcategoryID && x.IsDeleted == false) == 0)
                {
                    var model = new BookSubCategories()
                    {
                        BookID = bookID,
                        SubCategoryID = subcategoryID,
                        DateCreated = DateTime.Now,
                        IsDeleted = false
                    };
                    _db.BookSubCategories.Add(model);
                }
            }
            _db.SaveChanges();
        }
        
        // Removing book sub cateogories tagged
        public void RemoveAllSubCategories(int bookID, int[] selectedSubCategories)
        {
            var subcategories = _db.BookSubCategories.Where(x => x.BookID == bookID && x.IsDeleted == false).ToList();
            if(selectedSubCategories == null)
            {
                foreach (var subcategory in subcategories)
                {
                    subcategory.IsDeleted = true;
                    _db.Entry(subcategory).State = System.Data.Entity.EntityState.Modified;
                    _db.SaveChanges();
                }
            }
            else
            {
                foreach (var subcategory in subcategories)
                {
                    if (!selectedSubCategories.Contains(subcategory.Id))
                    {
                        subcategory.IsDeleted = true;
                        _db.Entry(subcategory).State = System.Data.Entity.EntityState.Modified;
                        _db.SaveChanges();
                    }
                }
            }
           
        }
        
        // Fetching all tagged book sub categories
        public List<SelectListItem> GetSelectedSubCategories(int bookID)
        {
            var bookSubCategories = _db.BookSubCategories.Where(x => x.IsDeleted == false && x.BookID == bookID).Select(b => b.SubCategoryID).ToList();
            List<SelectListItem> items = _db.Categories.Where(x => x.IsDeleted == false && x.ParentID != null).Select(b => new SelectListItem()
            {
                Text = b.Description,
                Value = b.Id.ToString(),
                Selected = bookSubCategories.Contains(b.Id),
            }).ToList();
            return items;
        }

        // Getting searchs
        public List<BookVM> GetSearch(string query)
        {
            byte[] empty = { 4,3 };
            var books = _db.Books.Where(x => (x.Name.StartsWith(query) || x.Author.StartsWith(query)) && (x.IsDeleted == false))
                .Select(b => new BookVM()
                {
                    Id = b.Id,
                    Name = b.Name,
                    Author = b.Author,
                    Categories = b.Categories.Description,
                    FrontCover = b.FrontCover == null ? empty : b.FrontCover,
                    BackCover = b.BackCover == null ? empty : b.BackCover,
                }).ToList();

            return books;
        }

        // Getting sub category books
        public List<BookVM> GetSubCategoryBooks(int Id)
        {
            byte[] empty = { 4, 3 };
            var model = _db.BookSubCategories.Where(x => x.SubCategoryID == Id && x.IsDeleted == false).Select(b => new BookVM()
            {
                Id = b.Book.Id,
                Name = b.Book.Name,
                Author = b.Book.Author,
                Categories = b.Book.Categories.Description,
                FrontCover = b.Book.FrontCover == null ? empty : b.Book.FrontCover,
                BackCover = b.Book.BackCover == null ? empty : b.Book.BackCover,
            }).ToList();
            return model;
        }
    }
}