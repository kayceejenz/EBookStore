using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLibrary.DAL.Entity
{
    public class Article
    {
        public int Id { get; set; }
        public string Description { get; set; }

        public string Overview { get; set; }
        public byte[] Image { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public int? CreatedUserID { get; set; }
        public int? EdittedUserID { get; set; }
        public int? SubCategoryID { get; set; }
        public int? CreatedByID { get; set; }
        public bool IsFeatured { get; set; }

        [ForeignKey("CreatedByID")]
        public User CreatedBy { get; set; }
        [ForeignKey("SubCategoryID")]
        public Categories SubCategory { get; set; }
        [ForeignKey("CreatedUserID")]
        public User CreatedUser { get; set; }
        [ForeignKey("EdittedUserID")]
        public User EdittedUser { get; set; }
    }
}
