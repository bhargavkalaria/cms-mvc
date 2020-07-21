using CMS.BE.ViewModels;
using CMS.BL.Interface;
using CMS.Common;
using CMS.Filter;
using Newtonsoft.Json;
using NLog;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace CMS.Controllers
{
    [CMS_Exception]
    [HandleError(View = "Custom_Error")]
    public class LoginController : Controller
    {
        string baseUrl = "https://localhost:44308/";
        private ILoginManager _iLoginManager;
        private IRoleManager _iRoleManager;
        private Constant constant = new Constant();
        private SendEmail sendEmail = new SendEmail();
        public LoginController()
        {

        }
        public LoginController(ILoginManager iLoginManager, IRoleManager iRoleManager)
        {
            _iLoginManager = iLoginManager;
            _iRoleManager = iRoleManager;
        }
        // GET: Login
        [HttpGet]
        public ActionResult Login()
        {
            if (Request.IsAuthenticated && Request.Cookies["UserId"] != null)
            {
                return RedirectToAction(constant.indexView, "MainDashboard");
            }
            UserViewModel userModel = new UserViewModel();
            userModel.RememberMe = false;
            ViewBag.Email = "";
            return View(userModel);
            
        }
        [HttpPost]
        public ActionResult Login(UserViewModel userModel)
        {
            
            UserViewModel data = null;
            if (Request.IsAuthenticated && Request.Cookies["UserId"] != null)
            {
                return RedirectToAction(constant.indexView, constant.dashboardController);
            }
            ViewBag.LoginError = "";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var responseTask = client.PostAsJsonAsync("api/LoginApi/Login", userModel);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var Response = result.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<UserViewModel>(Response);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View();
                   
                }
            }
            var userDetail = data;
            if (userDetail.Email != null)
            {
                int timeout = userModel.RememberMe ? 3600 : 200;
                var ticket = new FormsAuthenticationTicket(userModel.Email, userModel.RememberMe, timeout);
                string encrypted = FormsAuthentication.Encrypt(ticket);
                var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypted);
                cookie.Expires = DateTime.Now.AddMinutes(timeout);
                cookie.HttpOnly = true;
                Response.Cookies.Add(cookie);

                Session["UserName"] = userDetail.FName;

                string UId = "UserId" + userDetail.UId + "END";
                string encUId = Encrypt.EncryptString(UId);

                var userCookie = new HttpCookie("UserId", encUId);
                cookie.Expires = DateTime.Now.AddMinutes(timeout);
                userCookie.HttpOnly = true;
                Response.Cookies.Add(userCookie);
                
                return RedirectToAction(constant.indexView, "MainDashboard");
            }
            else
            {
                ViewBag.LoginError = constant.LoginError;
                return View(userModel);
            }
        }
        [HttpGet]
        public ActionResult AddUser()
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            //if (!getAddUserAccess())
            //{
            //    return RedirectToAction("NotAuthorized", "Response");
            //}
            ViewBag.Email = "";
            ViewBag.Fname = "";
            ViewBag.Lname = "";
            return View();
        }
        [HttpPost]
        public ActionResult AddUser(UserViewModel userModel)
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (!getAddUserAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            if (ModelState.IsValid)
            {
                ViewBag.EmailValidationError = "";
                ViewBag.PwdValidationError = "";
                ViewBag.NameValidationError = "";
                ViewBag.FormValidationError = "";
                ViewBag.Email = userModel.Email;
                ViewBag.Fname = userModel.FName;
                ViewBag.Lname = userModel.LName;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    var responseTask = client.GetAsync("api/LoginApi/CheckEmail?Email=" + userModel.Email);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var registerTask = client.PostAsJsonAsync("api/LoginApi/Register", userModel);
                        registerTask.Wait();

                        var registerResult = registerTask.Result;
                        if (registerResult.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Index","MainDashboard");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Something went wrong");
                            return View(userModel);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Email Already Exist");
                        return View(userModel);
                    }
                }
            }
            else
            {
                return View(userModel);
            }
        }
        [HttpPost]
        public ActionResult ResetPassword(string cur_pwd, string new_pwd)
        {
            string Email = Request.Cookies["Email"].Value;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                var responseTask = client.GetAsync("api/LoginApi/ResetPassword?email=" + Email + "&cur_pwd=" + cur_pwd + "&new_pwd=" + new_pwd);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return View("Login");
                }
                else
                {
                    return View();
                }
            }
        }
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> ForgotPassword(string Email)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    var responseTask = client.GetAsync("api/LoginApi/ForgotPassword?Email=" + Email);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var Response = result.Content.ReadAsStringAsync().Result;
                        string pwd  = JsonConvert.DeserializeObject<string>(Response);
                        var body = createEmailBody(Email, pwd);
                        bool status = await sendEmail.SendForgotMail(Email, body);
                        if (status)
                        {
                            return View("Login");
                        }
                    }
                    else
                    {
                        return View();
                    }
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e;
            }
            return View();
        }
        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            var getCookie = Request.Cookies["UserId"];
            if (getCookie == null)
                return View("Login");
            getCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(getCookie);
            return View("Login");
        }

        private string createEmailBody(string Email, string pwd)
        {
            string body = string.Empty;

            using (StreamReader reader = new StreamReader(Server.MapPath("~/Template/EmailTemplate.html")))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{Email}", Email);
            body = body.Replace("{ResetPassword}", pwd);

            return body;
        }
        
        private bool getAddUserAccess()
        {
            var getCookie = Request.Cookies["UserId"];
            if(getCookie == null)
            {
                return false;
            }
            string UId = getCookie.Value;
            string decUId = Encrypt.DecryptString(UId);

            int pFrom = decUId.IndexOf("UserId") + "UserId".Length;
            int pTo = decUId.LastIndexOf("END");

            int UserId = Convert.ToInt32(decUId.Substring(pFrom, pTo - pFrom));
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/RolesApi/GetAddUserAccess?id=" + UserId.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}