using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using GrowBot.Website.Classes.Helpers;
using GrowBot.Website.Models.Membership;
using GrowBot.Website.ViewModels.Membership;
using Newtonsoft.Json;
using RestSharp;
using HttpCookie = System.Web.HttpCookie;

namespace GrowBot.Website.Controllers.Membership
{
    public class MembershipController : Controller
    {
        private readonly HttpHelper httpHelper = new HttpHelper();

        // GET: Membership
        public ActionResult SignIn(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SignIn(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                CustomPrincipalSerializeModel tokenContent = GetUserToken(model.Email, model.Password);
                if (tokenContent != null && tokenContent.Token != null)
                {
                    CustomPrincipalSerializeModel userInfo = GetUserInfo(tokenContent.Token);
                    var userDetails = new CustomPrincipalSerializeModel
                    {
                        Email = tokenContent.Email,
                        Token = tokenContent.Token,
                        TokenExpiry = tokenContent.TokenExpiry,
                        FirstName = userInfo.FirstName,
                        LastName = userInfo.LastName,
                        Roles = userInfo.Roles
                    };
                    SignIn(userDetails, model.RememberMe);

                    return RedirectToLocal(returnUrl);
                }
                ModelState.AddModelError("", "Email/Password combination is wrong");
                return View(model);
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Login Failed");
            return View(model);
        }

        private CustomPrincipalSerializeModel GetUserToken(string userName, string password)
        {
            var request = new RestRequest("token", Method.POST);
            request.AddParameter("grant_type", "password");
            request.AddParameter("UserName", userName);
            request.AddParameter("password", password);
            IRestResponse response = httpHelper.ExecuteRequest(request);

            return JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(response.Content);
        }

        private CustomPrincipalSerializeModel GetUserInfo(string token)
        {
            var request2 = new RestRequest("api/Account/UserInfo", Method.GET);
            request2.AddHeader("Authorization", string.Format("Bearer {0}", token));
            IRestResponse response2 = httpHelper.ExecuteRequest(request2);
            return JsonConvert.DeserializeObject<CustomPrincipalSerializeModel>(response2.Content);
        }

        private void SignIn(CustomPrincipalSerializeModel userDetails, bool rememberMe)
        {
            //Need to get other user details

            var serializer = new JavaScriptSerializer();
            string userData = serializer.Serialize(userDetails);

            var authTicket = new FormsAuthenticationTicket(
                1,
                userDetails.Email,
                DateTime.Now,
                userDetails.TokenExpiry,
                rememberMe,
                userData);

            string encTicket = FormsAuthentication.Encrypt(authTicket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            if (rememberMe)
            {
                faCookie.Expires = authTicket.Expiration;
            }
            Response.Cookies.Add(faCookie);
        }

        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }
    }
}