using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLibrary.DAL.Entity
{
    public class MemberWishList
    {
        public int Id { get; set; }
        public int? MemberID { get; set; }
        public int? BookID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateAdded { get; set; }

        [ForeignKey("MemberID")]
        public Member Member { get; set; }
        [ForeignKey("BookID")]
        public Book Book { get; set; }
    }
}
