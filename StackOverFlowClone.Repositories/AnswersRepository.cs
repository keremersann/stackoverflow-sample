using StackOverFlowClone.DomainModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlowClone.Repositories
{
    public interface IAnswersRepository
    {
        void InsertAnswer(Answer answer);
        void UpdateAnswer(Answer answer);
        void UpdateAnswerVotesCount(int answerId, int userId, int value);
        void DeleteAnswer(int answerId);
        List<Answer> GetAnswersByQuestionId(int questionId);
        List<Answer> GetAnswersByAnswerId(int questionId);

    }
    public class AnswersRepository : IAnswersRepository
    {

        public StackOverflowDatabaseDBContext db;
        public IQuestionsRepository qr;
        public IVotesRepository vr;

        public AnswersRepository()
        {
            db = new StackOverflowDatabaseDBContext();
            qr = new QuestionsRepository();
            vr = new VotesRepository();
        }
        public void InsertAnswer(Answer answer) 
        {
            db.Answers.Add(answer);
            db.SaveChanges();
            qr.UpdateQuestionAnswersCount(answer.QuestionID, 1);
        }
        public void UpdateAnswer(Answer answer)
        {
            Answer answerToBeUpdated = db.Answers.Where(a => a.AnswerID == answer.AnswerID).FirstOrDefault();
            if (answerToBeUpdated != null) 
            {
                answerToBeUpdated.AnswerText = answer.AnswerText;
                db.SaveChanges();
            }
        }
        public void UpdateAnswerVotesCount(int answerId, int userId, int value)
        {
            Answer answerToBeUpdated = db.Answers.Where(a => a.AnswerID == answerId).FirstOrDefault();
            if (answerToBeUpdated != null)
            {
                answerToBeUpdated.VotesCount += value;
                db.SaveChanges();
                qr.UpdateQuestionVotesCount(answerToBeUpdated.QuestionID, value);
                vr.UpdateVote(answerId, userId, value); 
            }
        }
        public void DeleteAnswer(int answerId)
        {
            Answer answerToBeDeleted = db.Answers.Where(a => a.AnswerID == answerId).FirstOrDefault();
            if (answerToBeDeleted != null)
            {
                db.Answers.Remove(answerToBeDeleted);
                db.SaveChanges();
                qr.UpdateQuestionAnswersCount(answerToBeDeleted.QuestionID, -1);
            }
        }
        public List<Answer> GetAnswersByQuestionId(int questionId)
        {
            return db.Answers.Where(a => a.QuestionID == questionId).ToList();
        }
        public List<Answer> GetAnswersByAnswerId(int answerId)
        {
            return db.Answers.Where(a => a.AnswerID == answerId).ToList();
        }

    }
}
