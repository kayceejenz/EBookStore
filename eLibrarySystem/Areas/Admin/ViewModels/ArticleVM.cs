using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLibrarySystem.Areas.Admin.ViewModels
{
    public class ArticleVM
    {
        public int Id { get; set; }
        public string Description { get; set; }
        [AllowHtml]
        public string Overview { get; set; }

        public byte[] Image { get; set; }
        public string ImageString { get; set; }
        public int? SubCategoryID { get; set; }
        public string SubCategory { get; set; }
        public int CategoryID { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsFeatured { get; set; }
    }
}