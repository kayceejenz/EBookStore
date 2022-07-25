using eLibrarySystem.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace eLibrarySystem.Areas.Admin.Interfaces
{
    public interface IBookService
    {
        // Books
        List<BookVM> SearchBook(BookVM vmodel);
        bool AddBook(BookVM vmodel, HttpPostedFileBase FrontPage, HttpPostedFileBase BackPage);
        BookVM GetBook(int id);
        bool UpdateBook(BookVM vmodel, HttpPostedFileBase FrontPage, HttpPostedFileBase BackPage);
        bool DeleteBook(int id);
        List<SelectListItem> GetSelectedSubCategories(int bookID);
        List<BookVM> GetSearch(string query);
        List<BookVM> GetSubCategoryBooks(int Id);
    }
}
