using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLibrary.DAL.Entity
{
   public class Book
    {
        public int Id { get; set; }
        public byte[] FrontCover { get; set; }
        public byte[] BackCover { get; set; }
        public string Name { get; set; }
        public Guid UniqueNumber { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Edition { get; set; }
        public string Publisher { get; set; }
        public int CategoryID { get; set; }
        public DateTime YearPublished { get; set; }
        public BookFormat BookFormat { get; set; }
        public ContentFormat ContentFormat { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? LastModified { get; set; }
        public int? CreatedByID { get; set; }
        public bool IsFeatured { get; set; }


        [ForeignKey("CreatedByID")]
        public User CreatedBy { get; set; }
        [ForeignKey("CategoryID")]
        public Categories Categories { get; set; }
      
    }
}
