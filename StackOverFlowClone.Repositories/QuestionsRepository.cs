using StackOverFlowClone.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlowClone.Repositories
{
    public interface IQuestionsRepository
    {
        void InsertQuestion(Question question);
        void UpdateQuestionDetails(Question question);
        void UpdateQuestionVotesCount(int questionId, int value);
        void UpdateQuestionAnswersCount(int questionId, int value);
        void UpdateQuestionViewsCount(int questionId, int value);
        void DeleteQuestion(int questionId);    
        List<Question> GetAllQuestions();
        List<Question> GetQuestionsBySearchKeyword(string searchKeyword);
        List<Question> GetQuestionsByQuestionId(int questionId);
    }
    public class QuestionsRepository : IQuestionsRepository
    {
        StackOverflowDatabaseDBContext db;
        public QuestionsRepository()
        {
            db = new StackOverflowDatabaseDBContext();
        }
        public void InsertQuestion(Question q) 
        {
            db.Questions.Add(q);
            db.SaveChanges();
        }
        public void UpdateQuestionDetails(Question question)
        {
            Question questionToBeUpdated = db.Questions.Where(q => q.QuestionID == question.QuestionID).FirstOrDefault();
            if (questionToBeUpdated != null) 
            {
                questionToBeUpdated.QuestionName = question.QuestionName;
                questionToBeUpdated.QuestionDateAndTime = question.QuestionDateAndTime;
                questionToBeUpdated.CategoryID = question.CategoryID;
                db.SaveChanges();
            }
        }
        public void UpdateQuestionVotesCount(int questionId, int value)
        {
            Question questionToBeUpdated = db.Questions.Where(q => q.QuestionID == questionId).FirstOrDefault();
            if (questionToBeUpdated != null)
            {
                questionToBeUpdated.VotesCount += value;
                db.SaveChanges();
            }
        }
        public void UpdateQuestionAnswersCount(int questionId, int value)
        {
            Question questionToBeUpdated = db.Questions.Where(q => q.QuestionID == questionId).FirstOrDefault();
            if (questionToBeUpdated != null)
            {
                questionToBeUpdated.AnswersCount += value;
                db.SaveChanges();
            }
        }
        public void UpdateQuestionViewsCount(int questionId, int value)
        {
            Question questionToBeUpdated = db.Questions.Where(q => q.QuestionID == questionId).FirstOrDefault();
            if (questionToBeUpdated != null)
            {
                questionToBeUpdated.ViewsCount += value;
                db.SaveChanges();
            }
        }
        public void DeleteQuestion(int questionId)
        {
            Question questionToBeDeleted = db.Questions.Where(q => q.QuestionID == questionId).FirstOrDefault();
            if (questionToBeDeleted != null)
            {
                db.Questions.Remove(questionToBeDeleted);
                db.SaveChanges();
            }
        }
        public List<Question> GetAllQuestions()
        {
            return db.Questions.OrderByDescending(q => q.QuestionDateAndTime).ToList();
        }
        public List<Question> GetQuestionsBySearchKeyword(string searchKeyword)
        {
            return db.Questions.Where(q => q.QuestionName.Contains(searchKeyword)).OrderByDescending(q => q.QuestionDateAndTime).ToList();
        }
        public List<Question> GetQuestionsByQuestionId(int questionId)
        {
            return db.Questions.Where(q => q.QuestionID == questionId).ToList();
        }
    }
}
