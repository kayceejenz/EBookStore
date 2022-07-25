using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLibrary.DAL.Entity
{
    public class Categories
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public int? ParentID { get; set; }
        public string ParentDescription { get; set; }
        public string ContentInformation { get; set; }

        [ForeignKey("ParentID")]
        public Categories Parent { get;set; }
    }
}
