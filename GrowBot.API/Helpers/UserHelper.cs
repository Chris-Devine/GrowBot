using System;
using System.Web;
using GrowBot.API.Helpers.HelpersInterfaces;
using Microsoft.AspNet.Identity;

namespace GrowBot.API.Helpers
{
    public class UserHelper : IUserHelper
    {
        public Guid GetUserGuid()
        {
            return new Guid(HttpContext.Current.User.Identity.GetUserId());
        }
    }
}