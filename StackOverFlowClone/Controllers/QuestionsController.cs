using StackOverFlowClone.CustomFilters;
using StackOverFlowClone.ServiceLayer;
using StackOverFlowClone.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StackOverFlowClone.Controllers
{
    public class QuestionsController : Controller
    {
        IQuestionService qs;
        ICategoriesService cs;
        IAnswersService ans;

        public QuestionsController(IQuestionService qs, ICategoriesService cs, IAnswersService ans)
        {
            this.qs = qs;
            this.cs = cs;
            this.ans = ans;
        }

        public ActionResult View(int id)
        {
            qs.UpdateQuestionViewsCount(id, 1);
            var userId = Convert.ToInt32(Session["CurrentUserId"]);
            QuestionViewModel qvm = qs.GetQuestionByQuestionId(id, userId);

            return View(qvm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserAuthorizationFilter]
        public ActionResult AddAnswer(NewAnswerViewModel navm) 
        {
            navm.UserID = Convert.ToInt32(Session["CurrentUserId"]);
            navm.AnswerDateAndTime = DateTime.Now;
            navm.VotesCount = 0;
            if(ModelState.IsValid) 
            {
                ans.InsertAnswer(navm);
                return RedirectToAction("View", "Questions", new { id = navm.QuestionID });
            }
            else
            {
                ModelState.AddModelError("x", "Invalid Answer Data");
                QuestionViewModel qvm = qs.GetQuestionByQuestionId(navm.QuestionID, navm.UserID);
                return View("View", qvm);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [UserAuthorizationFilter]
        public ActionResult EditAnswer(EditAnswerViewModel eavm)
        {
            if (ModelState.IsValid)
            {
                eavm.UserID = Convert.ToInt32(Session["CurrentUserId"]);
                ans.UpdateAnswer(eavm);
                return RedirectToAction("View", new {id = eavm.QuestionID});
            }
            else
            {
                ModelState.AddModelError("x", "Invalid Data");
                return RedirectToAction("View", new { id = eavm.QuestionID });
            }
        }
        public ActionResult Create()
        {
            List<CategoryViewModel> categories = cs.GetCategories();
            ViewBag.Categories = categories;
            return View();
        }
        [HttpPost]
        [UserAuthorizationFilter]
        [ValidateAntiForgeryToken]
        public ActionResult Create(NewQuestionViewModel nqvm)
        {
            if (ModelState.IsValid) 
            {
                nqvm.QuestionDateAndTime = DateTime.Now;
                nqvm.UserID = Convert.ToInt32(Session["CurrentUserId"]);
                nqvm.VotesCount = 0;
                nqvm.AnswersCount = 0;
                nqvm.ViewsCount = 0;
                qs.InsertQuestion(nqvm);
                return RedirectToAction("Questions", "Home");
            }
            else
            {
                ModelState.AddModelError("x", "Invalid input data!");
                return View(nqvm);
            }
        }
    }
}