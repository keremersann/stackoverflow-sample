using StackOverFlowClone.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Http;
using System.Web.Http.Results;

namespace StackOverFlowClone.ApiControllers
{
    public class AccountController : ApiController
    {
        public IUsersService us;
        public AccountController(IUsersService us)
        {
            this.us = us;
        }

        [HttpGet]
        [Route("api/account/emailExists")]
        public string CheckEmailIsExists(string email)
        {
            var user = us.GetUsersByEmail(email);
            if(user == null) 
            {
                return "Not Found";
            }
            else
            {
                return "Found";
            }
        }
        [HttpGet]
        [Route("api/account/getPassword")]
        public string GetPasswordByUserId(int userId)
        {
            var user = us.GetUsersById(userId);
            if(user == null) 
            {
                return "Not Found";
            }
            else
            {
                return user.Password;
            }
        }
    }
}