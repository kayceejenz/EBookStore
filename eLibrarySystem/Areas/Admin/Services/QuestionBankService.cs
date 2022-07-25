using eLibrary.DAL.DataConnection;
using eLibrary.DAL.Entity;
using eLibrarySystem.Areas.Admin.Interfaces;
using eLibrarySystem.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace eLibrarySystem.Areas.Admin.Services
{
    public class QuestionBankService : IQuestionBankService
    {
        // Instanciation Process
        #region Instanciation
        readonly eLibraryDatabaseEntities _db;
        public QuestionBankService()
        {
            _db = new eLibraryDatabaseEntities();
        }
        public QuestionBankService(eLibraryDatabaseEntities db)
        {
            _db = db;
        }
        #endregion

        // Fetching all questions for a category
        public List<QuestionBankVM> GetQuestions(int? QuestionCategoryID)
        {
            var model = _db.QuestionBanks.Where(x => x.ArticleID == QuestionCategoryID && x.IsDeleted == false).Select(b => new QuestionBankVM()
            {
                Id = b.Id,
                Question = b.Question,
                QuestionType = b.QuestionType,
            }).ToList();
            foreach (var question in model)
            {
                question.OptionBank = GetOptions(question.Id);
                question.Answer = GetAnswer(question.Id);
            }
            return model;
        }

        // Fetching the options for a question
        public List<OptionBankVM> GetOptions(int QuestionID)
        {
            var options = _db.OptionBanks.Where(x => x.QuestionID == QuestionID).Select(b => new OptionBankVM()
            {
                Id = b.Id,
                OptionTxt = b.OptionText,
            }).ToList();
            return options;
        }

        // Fetching the answer of a question
        public AnswerVM GetAnswer(int QuestionID)
        {
            var answer = _db.AnswerBanks.Where(x => x.QuestionID == QuestionID)
                .Select(b => new AnswerVM() { Id = b.Id, AnswerText = b.AnswerText }).FirstOrDefault();
            return answer;
        }

        // Saving question created using a single upload method
        public bool SaveNewQuestion(QuestionBankVM Vmodel)
        {
            bool HasSaved = false;
            var QuestionModel = new QuestionBank()
            {
                Question = Vmodel.Question,
                ArticleID = Vmodel.QuestionCategoryID,
                QuestionType = Vmodel.QuestionType,
                IsDeleted = false,
            };
            _db.QuestionBanks.Add(QuestionModel);
            _db.SaveChanges();

            // Saving the options
            if (Vmodel.OptionBank != null)
            {
                foreach (var option in Vmodel.OptionBank)
                {
                    var OptionModel = new OptionBank()
                    {
                        QuestionID = QuestionModel.Id,
                        OptionText = option.OptionTxt,
                        IsDeleted = false
                    };
                    _db.OptionBanks.Add(OptionModel);
                }
                _db.SaveChanges();
            }

            // Saving the answer
            var answer = new AnswerBank()
            {
                QuestionID = QuestionModel.Id,
                AnswerText = Vmodel.Answer.AnswerText
            };
            _db.AnswerBanks.Add(answer);
            _db.SaveChanges();

            HasSaved = true;
            return HasSaved;
        }

        // Saving the uploaded question into the question bank
        public bool SaveNewQuestionUpload(QuestionBankVM Vmodel)
        {
            bool HasSaved = false;
            var QuestionModel = new QuestionBank()
            {
                Question = Vmodel.Question,
                ArticleID = Vmodel.QuestionCategoryID,
                QuestionType = (QuestionType)Enum.Parse(typeof(QuestionType), Vmodel.QuestionTypeUpload),
                IsDeleted = false,
            };
            _db.QuestionBanks.Add(QuestionModel);
            _db.SaveChanges();

            // Saving the options
            var OptionOne = new OptionBank()
            {
                QuestionID = QuestionModel.Id,
                OptionText = Vmodel.OptionOne,
                IsDeleted = false
            };
            _db.OptionBanks.Add(OptionOne);

            var OptionTwo = new OptionBank()
            {
                QuestionID = QuestionModel.Id,
                OptionText = Vmodel.OptionTwo,
                IsDeleted = false
            };
            _db.OptionBanks.Add(OptionTwo);

            var OptionThree = new OptionBank()
            {
                QuestionID = QuestionModel.Id,
                OptionText = Vmodel.OptionThree,
                IsDeleted = false
            };
            _db.OptionBanks.Add(OptionThree);

            var OptionFour = new OptionBank()
            {
                QuestionID = QuestionModel.Id,
                OptionText = Vmodel.OptionFour,
                IsDeleted = false
            };
            _db.OptionBanks.Add(OptionFour);

            var answer = new AnswerBank()
            {
                QuestionID = QuestionModel.Id,
                AnswerText = Vmodel.CorrectAnswer
            };
            _db.AnswerBanks.Add(answer);

            _db.SaveChanges();
            HasSaved = true;
            return HasSaved;
        }

        // Fetching Question according the question category
        public QuestionBankVM GetQuestion(int questionId)
        {
            var model = _db.QuestionBanks.Where(x => x.Id == questionId).Select(b => new QuestionBankVM()
            {
                Id = b.Id,
                Question = b.Question,
                QuestionCategoryID = b.ArticleID,
                QuestionType = b.QuestionType,
            }).FirstOrDefault();
            model.OptionBank = GetOptions(questionId);
            model.Answer = GetAnswer(questionId);
            return model;
        }

        // Saving a edited question to the question bank
        public bool SaveEditedQuestion(QuestionBankVM vmodel)
        {
            bool hasSaved = false;

            // Saving Question
            var questionModel = _db.QuestionBanks.FirstOrDefault(x => x.Id == vmodel.Id);
            questionModel.Question = vmodel.Question;
            questionModel.ArticleID = vmodel.QuestionCategoryID;
            _db.Entry(questionModel).State = EntityState.Modified;
            _db.SaveChanges();

            //Saving Options
            if (vmodel.OptionBank != null)
            {
                foreach (var option in vmodel.OptionBank)
                {
                    var model = _db.OptionBanks.FirstOrDefault(x => x.Id == option.Id);
                    model.OptionText = option.OptionTxt;
                    _db.Entry(model).State = EntityState.Modified;
                }
                _db.SaveChanges();
            }

            var answer = _db.AnswerBanks.FirstOrDefault(x => x.Id == vmodel.Answer.Id);
            answer.AnswerText = vmodel.Answer.AnswerText;
            _db.Entry(answer).State = EntityState.Modified;
            _db.SaveChanges();
            hasSaved = true;

            return hasSaved;
        }

        // Deleting a question from the question bank
        public void DeleteQuestion(int questionId)
        {
            var model = _db.QuestionBanks.FirstOrDefault(x => x.Id == questionId);
            model.IsDeleted = true;

            _db.Entry(model).State = EntityState.Modified;
            _db.SaveChanges();
        }

        // Fetching all question for a test in the already selected one
        public ArticleContentQuestionMappingVM GetSelectedQuestions(int ID)
        {
            var tableData = new List<QuestionMappingList>();
            var articleContent = _db.ArticleContents.Where(x => x.Id == ID).FirstOrDefault();

            var CheckQuestionExists = _db.ArticleContentQuestionMappings.Where(x => x.ArticleContentID == ID && x.IsDeleted == false).Count();
            if (CheckQuestionExists == 0)
            {
                tableData = _db.QuestionBanks.Where(x => x.IsDeleted == false && x.ArticleID == articleContent.ArticleID).Select(o => new QuestionMappingList()
                {
                    Id = o.Id,
                    ArticleContentID = ID,
                    ArticleContentHeading = articleContent.Heading,
                    QuestionID = o.Id,
                    Question = o.Question,
                    QuestionType = o.QuestionType,
                    ArticleID = (int)o.ArticleID,
                    IsSelected = false
                }).ToList();
                foreach (var item in tableData)
                {
                    item.Options = GetOptions(item.QuestionID);
                    item.Answer = GetAnswer(item.QuestionID);
                }
            }
            else
            {
                var Questions = _db.QuestionBanks.Where(x => x.IsDeleted == false && x.ArticleID == articleContent.ArticleID);
                var ArticleContentQuestionMapping = _db.ArticleContentQuestionMappings.Where(x => x.ArticleContentID == ID && x.IsDeleted == false).Select(o => o.Question);

                var unavailableQuestions = Questions.Except(ArticleContentQuestionMapping);
                foreach (var item in unavailableQuestions)
                {
                    var NewQuestion = new ArticleContentQuestionMapping();
                    NewQuestion.ArticleContentID = ID;
                    NewQuestion.QuestionID = item.Id;
                    NewQuestion.ArticleID = item.ArticleID;
                    NewQuestion.IsDeleted = false;
                    NewQuestion.DateCreated = DateTime.Now;
                    NewQuestion.IsSelected = false;
                    _db.ArticleContentQuestionMappings.Add(NewQuestion);
                }
                _db.SaveChanges();
                tableData = _db.ArticleContentQuestionMappings.Where(x => x.ArticleContentID == ID && x.IsDeleted == false).Select(o => new QuestionMappingList()
                {
                    Id = o.Id,
                    ArticleContentID = ID,
                    ArticleContentHeading = o.ArticleContent.Heading,
                    QuestionID = o.Question.Id,
                    Question = o.Question.Question,
                    QuestionType = o.Question.QuestionType,
                    ArticleID = (int)o.Question.ArticleID,
                    Mark = o.Mark,
                    IsSelected = o.IsSelected == true ? true : false,
                }).ToList();
                foreach (var item in tableData)
                {
                    item.Options = GetOptions(item.QuestionID);
                    item.Answer = GetAnswer(item.QuestionID);
                }
            }
            var model = new ArticleContentQuestionMappingVM(tableData);
            model.ArticleContent = articleContent.Heading;
            return model;
        }

        // Checking if a question already exists in a question bank
        public int CheckQuestionExist(int ArticleContentID, int QuestionID)
        {
            int result;
            result = _db.ArticleContentQuestionMappings.Where(x => x.ArticleContentID == ArticleContentID && x.QuestionID == QuestionID).Count();
            return result;
        }

        // Saving the question in the question bank in respect to assessment tests
        public ArticleContentQuestionMapping SaveQuestion(QuestionMappingList questionMapping)
        {
            var assessmentQuestionMapping = new ArticleContentQuestionMapping();
            assessmentQuestionMapping.QuestionID = questionMapping.QuestionID;
            assessmentQuestionMapping.ArticleContentID = questionMapping.ArticleContentID;
            assessmentQuestionMapping.ArticleID = questionMapping.ArticleID;
            assessmentQuestionMapping.IsSelected = questionMapping.IsSelected;
            assessmentQuestionMapping.Mark = questionMapping.Mark;
            assessmentQuestionMapping.IsDeleted = false;
            assessmentQuestionMapping.DateCreated = DateTime.Now;
            assessmentQuestionMapping.AssignedUserID = Global.AuthenticatedUserID; ;
            _db.ArticleContentQuestionMappings.Add(assessmentQuestionMapping);
            _db.SaveChanges();
            return assessmentQuestionMapping;
        }

        // Updating the assignment of question for assessment test
        public void AssignQuestion(QuestionMappingList questionMapping)
        {
            var assessmentQuestionMapping = _db.ArticleContentQuestionMappings.Where(p => p.ArticleContentID == questionMapping.ArticleContentID && p.QuestionID == questionMapping.QuestionID).FirstOrDefault();
            assessmentQuestionMapping.IsSelected = questionMapping.IsSelected;
            assessmentQuestionMapping.Mark = questionMapping.Mark;
            assessmentQuestionMapping.EditedUserID = Global.AuthenticatedUserID;
            _db.Entry(assessmentQuestionMapping).State = System.Data.Entity.EntityState.Modified;
            _db.SaveChanges();
        }
    }
}