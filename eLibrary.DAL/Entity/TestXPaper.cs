using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLibrary.DAL.Entity
{
    public class TestXPaper
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? MemberID { get; set; }
        public int? ArticleContentID { get; set; }
        public int? QuestionID { get; set; }
        public string AnswerText { get; set; }
        public bool IsCorrect { get; set; }
        public float MarkGotten { get; set; }

        [ForeignKey("MemberID")]
        public Member Member { get; set; }
        [ForeignKey("ArticleContentID")]
        public ArticleContent ArticleContent { get; set; }
        [ForeignKey("QuestionID")]
        public QuestionBank Question { get; set; }
    }
}
