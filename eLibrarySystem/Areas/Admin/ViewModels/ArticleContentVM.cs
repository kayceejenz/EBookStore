using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLibrarySystem.Areas.Admin.ViewModels
{
    public class ArticleContentVM
    {
        public int Id { get; set; }
        public string Heading { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public int? ArticleID { get; set; }
        public string Article { get; set; }
    }
}