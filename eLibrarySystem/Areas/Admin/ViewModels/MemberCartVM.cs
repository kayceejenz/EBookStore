using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLibrarySystem.Areas.Admin.ViewModels
{
    public class MemberCartVM
    {
        public int Id { get; set; }
        public int? MemberID { get; set; }
        public int? BookID { get; set; }
        public bool IsDeleted { get; set; }

        public string BookName { get; set; }
        public string Category { get; set; }
        public string Author { get; set; }
    }
}