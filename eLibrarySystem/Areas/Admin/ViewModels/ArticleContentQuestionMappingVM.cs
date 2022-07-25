using eLibrary.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eLibrarySystem.Areas.Admin.ViewModels
{
    public class ArticleContentQuestionMappingVM
    {
        public string ArticleContent { get; set; }

        public IList<dynamic> TableData { get; set; }
        public IList<QuestionMappingList> TableDataSource { get; set; }
        public ArticleContentQuestionMappingVM()
        {

        }
        public ArticleContentQuestionMappingVM(IEnumerable<dynamic> TData)
        {
            TableData = TData.ToArray();
        }
        public ArticleContentQuestionMappingVM(IEnumerable<QuestionMappingList> TData)
        {
            TableDataSource = TData.ToArray();
        }
    }
    public class QuestionMappingList
    {
        public int Id { get; set; }
        public bool IsSelected { get; set; }
        public int ArticleContentID { get; set; }
        public int QuestionID { get; set; }
        public QuestionType QuestionType { get; set; }
        public int? ArticleID { get; set; }
        public float Mark { get; set; }
        public string ArticleContentHeading { get; set; }
        public string Question { get; set; }
        public List<OptionBankVM> Options { get; set; }
        public AnswerVM Answer { get; set; }
    }
}
