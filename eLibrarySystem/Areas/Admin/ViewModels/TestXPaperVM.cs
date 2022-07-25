using eLibrary.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLibrarySystem.Areas.Admin.ViewModels
{
    public class TestXPaperVM
    {
        public int Id { get; set; }
        public int? ArticleID { get; set; }
        public int? CourseContentID { get; set; }
        public int? QuestionID { get; set; }
        public QuestionType QuestionType { get; set; }
        public string Question { get; set; }
        public string Anwser { get; set; }
        public string UserAnswer { get; set; }
        public bool IsCorrect { get; set; }
        public List<OptionBankVM> Options { get; set; }
        public TestXPaperVM() { }
    }
}