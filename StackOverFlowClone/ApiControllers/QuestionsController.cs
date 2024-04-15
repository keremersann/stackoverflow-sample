using StackOverFlowClone.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace StackOverFlowClone.ApiControllers
{
    public class QuestionsController : ApiController
    {
        IAnswersService asr;
        public QuestionsController(IAnswersService asr)
        {
            this.asr = asr;
        }
        public void Post(int AnswerID, int UserID, int value)
        {
            asr.UpdateAnswerVotesCount(AnswerID, UserID, value);
        }
    }
}