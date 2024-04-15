using StackOverFlowClone.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StackOverFlowClone.Repositories
{
    public interface IVotesRepository
    {
        void UpdateVote(int answerId, int userId, int value);
    }
    public class VotesRepository : IVotesRepository
    {
        StackOverflowDatabaseDBContext db;

        public VotesRepository()
        {
            db = new StackOverflowDatabaseDBContext();
        }

        public void UpdateVote(int answerId, int userId, int value)
        {
            int updateValue = 0;
            if(value > 0)
                updateValue = 1;
            else if(value < 0)
                updateValue = -1;

            Vote vote = db.Votes.Where(v => v.AnswerID == answerId && v.UserID == userId).FirstOrDefault();
            if(vote != null)
            {
                vote.VoteValue = updateValue;
            }
            else
            {
                Vote newVote = new Vote() { AnswerID = answerId, UserID = userId, VoteValue = value };
                db.Votes.Add(newVote);
            }
            db.SaveChanges();
        }
    }
}
