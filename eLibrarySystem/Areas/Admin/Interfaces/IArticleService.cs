using eLibrarySystem.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace eLibrarySystem.Areas.Admin.Interfaces
{
    interface IArticleService
    {
        // Article
        List<ArticleVM> GetArticles();

        List<ArticleVM> SearchArticle(ArticleVM vmodel);
        bool CreateArticle(ArticleVM Vmodel, HttpPostedFileBase ImageData);
        ArticleVM GetArticle(int ID);
        bool EditArticle(ArticleVM Vmodel, HttpPostedFileBase ImageData);
        bool DeleteArticle(int ID);

        List<BookVM> RelatedBooksForCourse(int CourseID);

        //Article Contents
        List<ArticleContentVM> GetArticleContents(int ArticleID);
        bool CreateArticleContent(ArticleContentVM Vmodel);
        ArticleContentVM GetArticleContent(int ID);
        bool EditArticleContent(ArticleContentVM Vmodel);
        bool DeleteArticleContent(int ID);
        List<ArticleVM> GetSearch(string query);
    }
}
