using eLibrary.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLibrarySystem.Areas.Admin.ViewModels
{
    public class QuestionBankVM
    {
        public int Id { get; set; }
        public int? OptionID { get; set; }
        public int? QuestionCategoryID { get; set; }
        public QuestionType QuestionType { get; set; }
        public string QuestionTypeUpload { get; set; }
        [AllowHtml]
        public string Question { get; set; }
        public int? Point { get; set; }
        public IList<OptionBankVM> OptionBank { get; set; }
        public string OptionOne { get; set; }
        public string OptionTwo { get; set; }
        public string OptionThree { get; set; }
        public string OptionFour { get; set; }
        public string CorrectAnswer { get; set; }
        public AnswerVM Answer { get; set; }
        public float Mark { get; set; }
        public IList<dynamic> TableData { get; set; }
        public QuestionBankVM()
        {

        }
        public QuestionBankVM(IEnumerable<OptionBankVM> TData)
        {
            OptionBank = TData.ToArray();
        }
        public QuestionBankVM(IEnumerable<dynamic> Tdata)
        {
            TableData = Tdata.ToArray();
        }
    }
    public class OptionBankVM
    {
        public int Id { get; set; }
        public int? QuestionID { get; set; }
        [AllowHtml]
        public string OptionTxt { get; set; }
    }

    public class AnswerVM
    {
        public int Id { get; set; }
        public string AnswerText { get; set; }
    }
}
