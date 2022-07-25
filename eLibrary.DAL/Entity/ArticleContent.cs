using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLibrary.DAL.Entity
{
    public class ArticleContent
    {
        public int Id { get; set; }
        public string Heading { get; set; }
        public string Body { get; set; }
        public int? ArticleID { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public int? CreatedUserID { get; set; }
        public int? EdittedUserID { get; set; }

        [ForeignKey("ArticleID")]
        public Article Article { get; set; }
    }
}
