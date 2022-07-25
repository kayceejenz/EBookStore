using eLibrary.DAL.Entity;
using eLibrarySystem.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLibrarySystem.Areas.Admin.Interfaces
{
    interface IQuestionBankService
    {
        // Questions
        List<QuestionBankVM> GetQuestions(int? QuestionCategoryID);
        List<OptionBankVM> GetOptions(int QuestionID);
        AnswerVM GetAnswer(int QuestionID);
        bool SaveNewQuestion(QuestionBankVM Vmodel);
        bool SaveNewQuestionUpload(QuestionBankVM Vmodel);
        QuestionBankVM GetQuestion(int questionId);
        bool SaveEditedQuestion(QuestionBankVM vmodel);
        void DeleteQuestion(int questionId);

        ArticleContentQuestionMappingVM GetSelectedQuestions(int ID);
        int CheckQuestionExist(int ArticleContentID, int QuestionID);
        ArticleContentQuestionMapping SaveQuestion(QuestionMappingList questionMapping);
        void AssignQuestion(QuestionMappingList questionMapping);
    }
}
