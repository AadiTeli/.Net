﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ProductManagementWebAPI;
namespace ProductManagementWebAPI.Controllers
{
    public class UserLoginController : ApiController
    {
        [HttpPost]
        public User employeeLogin(User _user)
        {
            //db db = new db();
            //var userRecord = db.Users.Where(x => x.Email.Equals(_user.Email) && x.Password.Equals(_user.Password)).FirstOrDefault();
            //if /*(/*userRecord*/)*/
            //{
            //    return null;
            //}
            //else
            //{
            //    return /*userRecord;*/
            //}
            return null;
        }
    }
}
