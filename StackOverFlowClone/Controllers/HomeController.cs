using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using StackOverFlowClone.ServiceLayer;
using StackOverFlowClone.ViewModels;

namespace StackOverFlowClone.Controllers
{
    public class HomeController : Controller
    {
        IQuestionService qs;
        ICategoriesService cs;
        public HomeController(IQuestionService qs, ICategoriesService cs)
        {
            this.qs = qs;
            this.cs = cs;
        }

        public ActionResult Index()
        {
            List<QuestionViewModel> questionViewModels = qs.GetQuestions().Take(10).ToList();
            return View(questionViewModels);
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult Contact() 
        {
            return View();
        }
        public ActionResult Categories() 
        {
            var categoryViewModelList = cs.GetCategories();
            return View(categoryViewModelList);
        }
        public ActionResult Questions()
        {
            List<QuestionViewModel> questionViewModels = qs.GetQuestions();
            return View(questionViewModels);
        }
        public ActionResult Search(string searchKeyword)
        {
            List<QuestionViewModel> questionViewModels = qs.GetQuestionsByContains(searchKeyword);
            ViewBag.SearchKeyword = searchKeyword;
            return View(questionViewModels);
        }
    }
}