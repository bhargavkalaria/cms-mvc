using CMS.BE.ViewModels;
using CMS.BL.Interface;
using CMS.Common;
using NLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CMS.Filter;

namespace CMS.Controllers
{
    [CMS_Exception]
    [HandleError(View = "Custom_Error")]
    public class RoleController : Controller
    {

        Constant constant = new Constant();
        public RoleController()
        {

        }
        // GET: Role
        public ActionResult userList()
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getRoleAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            IEnumerable<UserViewModel> users;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/RolesApi/GetUsersList");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var Response = result.Content.ReadAsStringAsync().Result;
                    users = JsonConvert.DeserializeObject<List<UserViewModel>>(Response);
                    return View(users);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View();
                }
            }
        }
        [HttpGet]
        public ActionResult updateRole(int? id)
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getRoleAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            UserViewModel user = null;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/RolesApi/GetUser?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var Response = result.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<UserViewModel>(Response);
                    return View("updateRole",user);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View();
                }
            }
        }
        [HttpPost]
        public async Task<ActionResult> updateRole(UserViewModel userModel)
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getRoleAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(constant.apiAddress);
                var response =await  client.PostAsJsonAsync("api/RolesApi/UpdateRole", userModel);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "MainDashboard");
                }
                else
                {
                    return View(userModel);
                }
            }
        }
        private bool getRoleAccess()
        {
            int UserId = getUId();
            if (UserId == 0)
                return false;
            UserViewModel user = null;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/RolesApi/GetUser?id=" + UserId.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var Response = result.Content.ReadAsStringAsync().Result;
                    user = JsonConvert.DeserializeObject<UserViewModel>(Response);
                }
            }
            if (user.Role.Equals("SUPERADMIN"))
                return true;
            return false;
        }
        private int getUId()
        {
            var getCookie = Request.Cookies["UserId"];
            if (getCookie == null)
            {
                return 0;
            }
            string UId = getCookie.Value;
            string decUId = Encrypt.DecryptString(UId);

            int pFrom = decUId.IndexOf("UserId") + "UserId".Length;
            int pTo = decUId.LastIndexOf("END");

            int UserId = Convert.ToInt32(decUId.Substring(pFrom, pTo - pFrom));
            return UserId;
        }
    }
}