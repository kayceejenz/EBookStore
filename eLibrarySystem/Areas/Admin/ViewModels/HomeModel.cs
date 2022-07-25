using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLibrarySystem.Areas.Admin.ViewModels
{
    public class HomeModel
    {
        public List<BookVM> FeaturedBooks { get; set; }
        public List<BookVM> NewRandowmBooks { get; set; }
        public IPagedList<BookVM> BookCollections { get; set; }
        public List<ArticleVM> FeaturedArticles { get; set; }
        public List<BookVM> SearchedBooks { get; set; }
        public List<ArticleVM> SearchedArticles { get; set; }
        public List<CategoriesVM> CategoryNav { get; set; }
        public int SearchCount { get; set; }
        public List<BookVM> Books { get; set; }

    }
}