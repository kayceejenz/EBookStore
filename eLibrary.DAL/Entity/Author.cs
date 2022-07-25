using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLibrary.DAL.Entity
{
    public class Author
    {
        public int Id { get; set; }
        public byte[] Photo { get; set; }
        public string Name { get; set; }
        public string Biography { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string CountryOfOrigin { get; set; }
        public string StateOfOrigin { get; set; }
        public string ContactAddress { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? LastModified { get; set; }
        public int? CreatedByID { get; set; }

        [ForeignKey("CreatedByID")]
        public User CreatedBy { get; set; }
    }
}
