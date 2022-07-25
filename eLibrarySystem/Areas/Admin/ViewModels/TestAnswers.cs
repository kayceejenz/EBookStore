using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLibrarySystem.Areas.Admin.ViewModels
{
    public class TestAnswers
    {
        public int QuestionID { get; set; }
        public string QuestionText { get; set; }
        public string AnswerQ { get; set; }
        public bool isCorrect { get; set; }
        public string CorrectAnswer { get; set; }
    }

}