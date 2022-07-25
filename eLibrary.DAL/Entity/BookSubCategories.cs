using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLibrary.DAL.Entity
{
    public class BookSubCategories
    {
        public int Id { get; set; }
        public int? BookID { get; set; }
        public int? SubCategoryID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }

        public Book Book { get; set; }
        public Categories SubCategory { get; set; }
    }
}
