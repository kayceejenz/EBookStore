using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLibrary.DAL.Entity
{
    public class AfflilateBonusManager
    {
        public int Id { get; set; }
        public int? AfflilateUserID { get; set; }
        public BonusType BonusType { get; set; }
        public decimal Amount { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? LastModified { get; set; }

        [ForeignKey("AfflilateUserID")]
        public User AfflicateUser { get; set; }
    }
}
