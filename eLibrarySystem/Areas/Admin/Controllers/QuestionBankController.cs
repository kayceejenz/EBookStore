using eLibrary.DAL.DataConnection;
using eLibrary.DAL.Entity;
using eLibrarySystem.Areas.Admin.Interfaces;
using eLibrarySystem.Areas.Admin.Services;
using eLibrarySystem.Areas.Admin.ViewModels;
using LumenWorks.Framework.IO.Csv;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace eLibrarySystem.Areas.Admin.Controllers
{
    public class QuestionBankController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            filterContext.ExceptionHandled = true;

            //Log the error!!
            log.Error("Error trying to do something", filterContext.Exception);
            Global.ErrorMessage = filterContext.Exception.ToString();

            //Redirect or return a view, but not both.
            filterContext.Result = RedirectToAction("Error", "Error", new { area = "" });

        }

        // Initializing dependency services

        #region Instantiation
        private eLibraryDatabaseEntities db = new eLibraryDatabaseEntities();
        private IQuestionBankService _questionBankService;
        private IArticleService _articleService;
        public QuestionBankController()
        {
            _questionBankService = new QuestionBankService(new eLibraryDatabaseEntities());
            _articleService = new ArticleService(new eLibraryDatabaseEntities());
        }
        public QuestionBankController(QuestionBankService questionBankService,ArticleService articleService)
        {
            _questionBankService = questionBankService;
            _articleService = articleService;
        }
        eLibraryDatabaseEntities _entities = new eLibraryDatabaseEntities();
        #endregion


        public ActionResult ManageQuestion(int ArticleID, HttpPostedFileBase file, bool? Saved)
        {
            if (Saved == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Question added successfully.";
            }
            ViewBag.Questions = _questionBankService.GetQuestions(ArticleID);
            ViewBag.Article = _articleService.GetArticle(ArticleID);
            ViewBag.ArticleID = ArticleID;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UploadQuestion(HttpPostedFileBase file, int ArticleID)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                var status = UploadQuestionCSVFile(file, ArticleID);
                if (status == true)
                    hasSaved = true;
            }
            return RedirectToAction("ManageQuestion", new { ArticleID = ArticleID, Saved = hasSaved });
        }

        public ActionResult AddQuestion(int ArticleID, bool? saved)
        {
            if (saved == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Question added successfully.";
            }
            ViewBag.Article = _articleService.GetArticle(ArticleID);

            var model = new QuestionBankVM()
            {
                QuestionCategoryID = ArticleID
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddQuestion(QuestionBankVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                var status = _questionBankService.SaveNewQuestion(vmodel);
                if (status.Equals(true))
                    hasSaved = true;
                return RedirectToAction("AddQuestion", new { ArticleID = vmodel.QuestionCategoryID, saved = hasSaved });
            }
            return View();
        }

        public ActionResult EditQuestion(int QuestionID, bool? Saved)
        {
            if (Saved == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Question updated successfully.";
            }
            var model = _questionBankService.GetQuestion(QuestionID);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditQuestion(QuestionBankVM vmodel)
        {
            bool hasSaved = false;
            if (ModelState.IsValid)
            {
                var status = _questionBankService.SaveEditedQuestion(vmodel);
                if (status.Equals(true))
                    hasSaved = true;
            }
            return RedirectToAction("EditQuestion", "QuestionBank", new { Saved = hasSaved, QuestionID = vmodel.Id });
        }

        public JsonResult DeleteQuestion(int id)
        {
            _questionBankService.DeleteQuestion(id);
            return Json(JsonRequestBehavior.AllowGet);
        }
        #region Functions

        public QuestionBankVM ReadBatchQuestionCSVFile(HttpPostedFileBase file)
        {
            var model = new QuestionBankVM();
            string data = "";
            if (file != null && file.ContentLength > 0)
            {
                string filename = file.FileName;
                if (file.FileName.EndsWith(".csv"))
                {
                    List<QuestionBankVM> vmodel = new List<QuestionBankVM>();
                    Stream stream = file.InputStream;
                    DataTable csvTable = new DataTable();
                    using (CsvReader csvReader = new CsvReader(new StreamReader(stream), true))
                    {
                        csvTable.Load(csvReader);
                    }
                    foreach (DataRow item in csvTable.Rows)
                    {
                        var questionVM = new QuestionBankVM()
                        {
                            Question = item.ItemArray[0].ToString(),
                            QuestionTypeUpload = item.ItemArray[1].ToString(),
                            OptionOne = item.ItemArray[2].ToString(),
                            OptionTwo = item.ItemArray[3].ToString(),
                            OptionThree = item.ItemArray[4].ToString(),
                            OptionFour = item.ItemArray[5].ToString(),
                            CorrectAnswer = item.ItemArray[6].ToString(),
                        };
                        vmodel.Add(questionVM);
                    }
                    model = new QuestionBankVM(vmodel);
                    return model;
                }
                else
                {
                    data = "File, This file format is not supported";
                    ViewBag.ErrorMessage = data;
                    return model;
                }
            }
            else
            {
                data = "File, Please Upload Your file";
                ViewBag.ErrorMessage = data;
                return model;
            }
        }

        public bool UploadQuestionCSVFile(HttpPostedFileBase file, int? QuestionCategoryID)
        {
            bool hasSaved = false;
            var items = ReadBatchQuestionCSVFile(file);
            foreach (var item in items.TableData)
            {
                item.QuestionCategoryID = QuestionCategoryID;
                _questionBankService.SaveNewQuestionUpload(item);
                hasSaved = true;
            }
            return hasSaved;
        }

        public ActionResult ManageArticleContentQuestions(int ArticleContentID, bool? Saved)
        {
            if (Saved == true)
            {
                ViewBag.ShowAlert = true;
                TempData["AlertType"] = "alert-success";
                TempData["AlertMessage"] = "Question mapped successfully.";
            }
            return View(_questionBankService.GetSelectedQuestions(ArticleContentID));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ManageArticleContentQuestions(ArticleContentQuestionMappingVM assessmentQuestionVM)
        {
            bool hasSaved = false;
            var model = new ArticleContentQuestionMappingVM();
            int ArticleContentID = 0;
            if (ModelState.IsValid)
            {
                var assessmentQuestionMapping = new ArticleContentQuestionMapping();
                foreach (var item in assessmentQuestionVM.TableDataSource)
                {
                    ArticleContentID = item.ArticleContentID;
                    if (_questionBankService.CheckQuestionExist(item.ArticleContentID, item.QuestionID) == 0)
                    {
                        assessmentQuestionMapping = _questionBankService.SaveQuestion(item);
                    }
                    else
                    {
                        _questionBankService.AssignQuestion(item);
                    }
                }
            }
            hasSaved = true;
            return ManageArticleContentQuestions(ArticleContentID, hasSaved);
        }
        #endregion
    }
}