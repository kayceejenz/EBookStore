using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLibrarySystem.Areas.Admin.ViewModels
{
    public class CategoriesVM
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public int? ParentID { get; set; }
        public string ContentInformation { get; set; }
        public string Parent { get; set; }
    }
}