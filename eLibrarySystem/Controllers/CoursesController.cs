using eLibrary.DAL.DataConnection;
using eLibrary.DAL.Entity;
using eLibrarySystem.Areas.Admin.Interfaces;
using eLibrarySystem.Areas.Admin.Services;
using eLibrarySystem.Areas.Admin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLibrarySystem.Controllers
{
    [AllowAnonymous]
    public class CoursesController : Controller
    {
        // Instanciation
        #region Instanciation
        IBookService _bookService;
        IAuthorService _authorService;
        IApplicationSettingsService _settingsService;
        IUserService _userService;
        IArticleService _articleService;
        IQuestionBankService _questionBankService;
        HomeModel model = new HomeModel();
        public CoursesController()
        {
            _bookService = new BookService(new eLibraryDatabaseEntities());
            _authorService = new AuthorService(new eLibraryDatabaseEntities());
            _articleService = new ArticleService(new eLibraryDatabaseEntities());
            _userService = new UserService(new eLibraryDatabaseEntities());
            _questionBankService = new QuestionBankService(new eLibraryDatabaseEntities());
            _settingsService = new ApplicationSettingsService(new eLibraryDatabaseEntities());
        }
        public CoursesController(BookService bookService, AuthorService authorService, ArticleService articleService, UserService userService, ApplicationSettingsService applicationSettingsService, QuestionBankService questionBankService)
        {
            _authorService = authorService;
            _bookService = bookService;
            _userService = userService;
            _articleService = articleService;
            _settingsService = applicationSettingsService;
            _questionBankService = questionBankService;
        }
        eLibraryDatabaseEntities db = new eLibraryDatabaseEntities();
        #endregion


        public ActionResult Courses()
        {
            ViewBag.Courses = _settingsService.GetCategories();
            ViewBag.ArticleCourses = _articleService.GetArticles();
            return View();
        }
        public ActionResult CourseContentHeader(int CourseID)
        {
            ViewBag.CourseContentHeader = _articleService.GetArticleContents(CourseID);
            ViewBag.Course = _articleService.GetArticle(CourseID);
            ViewBag.RelateBooks = _articleService.RelatedBooksForCourse(CourseID);
            return View();
        }

        public ActionResult Read(int courseContentID)
        {
            var articleID = db.ArticleContents.FirstOrDefault(x => x.Id == courseContentID).ArticleID;
            ViewBag.CourseContent = _articleService.GetArticleContent(courseContentID);
            IQueryable<TestXPaperVM> model = null;
            List<TestXPaperVM> tempModel = new List<TestXPaperVM>();
            ViewBag.Course = _articleService.GetArticle((int)articleID);

            tempModel = db.ArticleContentQuestionMappings.Where(x => x.ArticleContentID == courseContentID && x.ArticleID == articleID && x.IsDeleted == false && x.IsSelected == true).Select(o => new TestXPaperVM()
            {
                Id = o.Id,
                CourseContentID = o.ArticleContentID,
                QuestionID = o.Question.Id,
                Question = o.Question.Question,
                QuestionType = o.Question.QuestionType,
                ArticleID = (int)o.Question.ArticleID,
            }).OrderBy(o => Guid.NewGuid()).ToList();
            foreach (var item in tempModel)
            {
                item.Options = _questionBankService.GetOptions((int)item.QuestionID).OrderBy(o => Guid.NewGuid()).ToList();
            }
            model = tempModel.AsQueryable();
            Session["TestID"] = courseContentID;
            Session["CourseID"] = articleID;
            return View(model);
        }


        [HttpPost]
        public ActionResult PostAnswers(List<TestAnswers> Vmodel)
        {
            List<TestAnswers> finalResultQuiz = new List<TestAnswers>();
            //Analysing the test
            foreach (TestAnswers answer in Vmodel)
            {
                TestAnswers result = db.AnswerBanks.Where(a => a.QuestionID == answer.QuestionID).Select(a => new TestAnswers
                {
                    QuestionID = a.QuestionID.Value,
                    AnswerQ = answer.AnswerQ,
                    isCorrect = (answer.AnswerQ.ToLower().Equals(a.AnswerText.ToLower())),
                    CorrectAnswer = a.AnswerText
                }).FirstOrDefault();

                finalResultQuiz.Add(result);
            }

            int testID = Convert.ToInt32(Session["TestID"].ToString());
            int studentID = Global.AuthenticatedMemberID;
            var assessment = db.ArticleContents.FirstOrDefault(x => x.Id == testID);

            float TotalScore = 0;
            // Saving test to database
            foreach (var quiz in finalResultQuiz)
            {
                var finalResultQuizModel = new TestXPaper()
                {
                    ArticleContentID = testID,
                    MemberID = studentID,
                    QuestionID = quiz.QuestionID,
                    AnswerText = quiz.AnswerQ,
                    IsCorrect = quiz.isCorrect,
                    MarkGotten = quiz.isCorrect.Equals(true) ? 5 : 0
                };
                TotalScore += finalResultQuizModel.MarkGotten;
                db.TestXPapers.Add(finalResultQuizModel);
                db.SaveChanges();
            };

            return Json(new
            {
                result = finalResultQuiz
            }, JsonRequestBehavior.AllowGet);
        }

    }
}