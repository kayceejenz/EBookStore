using eLibrarySystem.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace eLibrarySystem.Areas.Admin.Interfaces
{
    interface IAuthorService
    {
        // Authors
        List<AuthorVM> GetAuthors();
        bool AddAuthor(AuthorVM vmodel, HttpPostedFileBase image);
        AuthorVM GetAuthor(int id);
        bool UpdateAuthor(AuthorVM vmodel, HttpPostedFileBase image);
        bool DeleteAuthor(int id);
    }
}
