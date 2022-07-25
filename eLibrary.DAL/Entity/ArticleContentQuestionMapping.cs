using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLibrary.DAL.Entity
{
    public class ArticleContentQuestionMapping
    {
        public int Id { get; set; }
        public int? ArticleContentID { get; set; }
        public int? QuestionID { get; set; }
        public int? ArticleID { get; set; }
        public float Mark { get; set; }
        public bool IsSelected { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime DateCreated { get; set; }
        public int? AssignedUserID { get; set; }
        public int? EditedUserID { get; set; }

        [ForeignKey("ArticleContentID")]
        public ArticleContent ArticleContent { get; set; }
        [ForeignKey("QuestionID")]
        public QuestionBank Question { get; set; }
    }
}
