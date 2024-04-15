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
    public interface IAnswersService
    {
        void InsertAnswer(NewAnswerViewModel answer);
        void UpdateAnswer(EditAnswerViewModel answer);
        void UpdateAnswerVotesCount(int aid, int uid, int value);
        void DeleteAnswer(int aid);
        List<AnswerViewModel> GetAnswersByQuestionId(int qid);
        AnswerViewModel GetAnswerByAnswerId(int aid);
    }
    public class AnswersService : IAnswersService
    {
        public IAnswersRepository ar;
        public AnswersService()
        {
            ar = new AnswersRepository();
        }
        public void InsertAnswer(NewAnswerViewModel newAnswer)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<NewAnswerViewModel, Answer>();
                cfg.IgnoreUnmapped();
            });
            IMapper mapper = config.CreateMapper();
            Answer answer = mapper.Map<NewAnswerViewModel, Answer>(newAnswer);
            ar.InsertAnswer(answer);
        }
        public void UpdateAnswer(EditAnswerViewModel awm)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EditAnswerViewModel, Answer>();
                cfg.IgnoreUnmapped();
            });
            IMapper mapper = config.CreateMapper();
            Answer answer = mapper.Map<EditAnswerViewModel, Answer>(awm);
            ar.UpdateAnswer(answer);
        }
        public void UpdateAnswerVotesCount(int aid, int uid, int value)
        {
            ar.UpdateAnswerVotesCount(aid, uid, value);
        }
        public void DeleteAnswer(int aid)
        {
            ar.DeleteAnswer(aid);
        }
        public List<AnswerViewModel> GetAnswersByQuestionId(int qid)
        {
            List<Answer> answers = ar.GetAnswersByQuestionId(qid);
            List<AnswerViewModel> avm = null;
            if(answers != null)
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
                avm = mapper.Map<List<Answer>, List<AnswerViewModel>>(answers);
            }
            return avm;
        }
        public AnswerViewModel GetAnswerByAnswerId(int aid)
        {
            Answer answer = ar.GetAnswersByAnswerId(aid).FirstOrDefault();
            AnswerViewModel avm = null;
            if (answer != null)
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
                avm = mapper.Map<Answer, AnswerViewModel>(answer);
            }
            return avm;
        }
    }
}
