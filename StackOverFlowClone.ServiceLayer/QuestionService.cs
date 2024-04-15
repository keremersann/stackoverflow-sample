using AutoMapper;
using StackOverFlowClone.DomainModels;
using StackOverFlowClone.Repositories;
using StackOverFlowClone.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlowClone.ServiceLayer
{

    public interface IQuestionService
    {
        void InsertQuestion(NewQuestionViewModel questionViewModel);
        void UpdateQuestionDetails(EditQuestionViewModel questionViewModel);
        void UpdateQuestionVotesCount(int questionID, int value);
        void UpdateQuestionAnswersCount(int questionID, int value);
        void UpdateQuestionViewsCount(int questionID, int value);
        void DeleteQuestion(int questionID);
        List<QuestionViewModel> GetQuestions();
        List<QuestionViewModel> GetQuestionsByContains(string searchKeyword);
        QuestionViewModel GetQuestionByQuestionId(int questionID, int userID);
    }
    public class QuestionService : IQuestionService
    {
        IQuestionsRepository qr;
        public QuestionService()
        {
            qr = new QuestionsRepository();
        }
        public void InsertQuestion(NewQuestionViewModel questionViewModel) 
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NewQuestionViewModel, Question>();
                cfg.IgnoreUnmapped();
            });
            IMapper mapper = config.CreateMapper();
            Question question = mapper.Map<NewQuestionViewModel, Question>(questionViewModel);
            qr.InsertQuestion(question);
        }
        public void UpdateQuestionDetails(EditQuestionViewModel questionViewModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EditQuestionViewModel, Question>();
                cfg.IgnoreUnmapped();
            });
            IMapper mapper = config.CreateMapper();
            Question question = mapper.Map<EditQuestionViewModel, Question>(questionViewModel);
            qr.UpdateQuestionDetails(question);
        }
        public void UpdateQuestionVotesCount(int questionID, int value)
        {
            qr.UpdateQuestionVotesCount(questionID, value);
        }
        public void UpdateQuestionAnswersCount(int questionID, int value)
        {
            qr.UpdateQuestionAnswersCount(questionID, value);
        }
        public void UpdateQuestionViewsCount(int questionID, int value)
        {
            qr.UpdateQuestionViewsCount(questionID, value);
        }
        public void DeleteQuestion(int questionID)
        {
            qr.DeleteQuestion(questionID);
        }
        public List<QuestionViewModel> GetQuestions()
        {
            List<Question> questions = qr.GetAllQuestions();
            List<QuestionViewModel> qvm = null;
            if(questions != null)
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<Question, QuestionViewModel>();
                    cfg.CreateMap<User, UserViewModel>();
                    cfg.CreateMap<Category, CategoryViewModel>();
                    cfg.CreateMap<Answer, AnswerViewModel>();
                    cfg.CreateMap<Vote, VoteViewModel>();
                    cfg.IgnoreUnmapped();
                });
                IMapper mapper = config.CreateMapper();
                qvm = mapper.Map<List<Question>, List<QuestionViewModel>>(questions);
            }
            return qvm;
        }
        public List<QuestionViewModel> GetQuestionsByContains(string searchKeyword = "")
        {
            List<Question> questions = searchKeyword == "" ? qr.GetAllQuestions() : qr.GetQuestionsBySearchKeyword(searchKeyword);
            List<QuestionViewModel> qvm = null;
            if (questions != null)
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<Question, QuestionViewModel>();
                    cfg.CreateMap<User, UserViewModel>();
                    cfg.CreateMap<Category, CategoryViewModel>();
                    cfg.CreateMap<Answer, AnswerViewModel>();
                    cfg.CreateMap<Vote, VoteViewModel>();
                    cfg.IgnoreUnmapped();
                });
                IMapper mapper = config.CreateMapper();
                qvm = mapper.Map<List<Question>, List<QuestionViewModel>>(questions);
            }
            return qvm;
        }
        public QuestionViewModel GetQuestionByQuestionId(int questionID, int userID = 0)
        {
            Question question = qr.GetQuestionsByQuestionId(questionID).FirstOrDefault();
            QuestionViewModel qvm = null;
            if (question != null)
            {
                var config = new MapperConfiguration(cfg => {
                    cfg.CreateMap<Question, QuestionViewModel>();
                    cfg.CreateMap<User, UserViewModel>();
                    cfg.CreateMap<Category, CategoryViewModel>();
                    cfg.CreateMap<Answer, AnswerViewModel>();
                    cfg.CreateMap<Vote, VoteViewModel>();
                    cfg.IgnoreUnmapped();
                });
                IMapper mapper = config.CreateMapper();
                qvm = mapper.Map<Question, QuestionViewModel>(question);

                foreach (var item in qvm.Answers)
                {
                    item.CurrentUserVoteType = 0;
                    VoteViewModel vvm = item.Votes.Where(vote => vote.UserID == userID).FirstOrDefault();
                    if(vvm != null)
                        item.CurrentUserVoteType = vvm.VoteValue;
                }
            }
            return qvm;
        }
        
    }
}
