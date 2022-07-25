using eLibrary.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLibrarySystem.Areas.Admin.ViewModels
{
    public class BookVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Edition { get; set; }
        public string Publisher { get; set; }
        public int[] SubCategories { get; set; }
        public int CategoryID { get; set; }
        public DateTime YearPublished { get; set; }
        public BookFormat BookFormat { get; set; }
        public ContentFormat ContentFormat { get; set; }
        public string Categories { get; set; }
        public byte[] FrontCover { get; set; }
        public byte[] BackCover { get; set; }
        public DateTime DateCreated { get; set; }
        public bool IsFeatured { get; set; }

    }
}