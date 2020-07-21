using CMS.BE.ViewModels;
using CMS.BL.Interface;
using CMS.Common;
using CMS.Filter;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CMS.Controllers
{
    [CMS_Exception]
    [HandleError(View = "Custom_Error")]
    public class TemplateController : Controller
    {
        string baseUrl = "https://localhost:44308/";
        private IRoleManager _iRoleManager;
        private IEmailMasterManager _iemailMasterManager;
        private Constant constant = new Constant();
        public TemplateController()
        {

        }
        public ActionResult Templates()
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (!getViewTemplateAccess())
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!(getAddTemplateAccess() || getDeleteTemplateAccess() || getEditTemplateAccess() || getViewTemplateAccess()))
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            IEnumerable<TemplateViewModel> TemplateList = null;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                string addressUri = "api/TemplateApi/GetAllTemplates";
                var responseTask = client.GetAsync(addressUri);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var Response = result.Content.ReadAsStringAsync().Result;
                    TemplateList = JsonConvert.DeserializeObject<List<TemplateViewModel>>(Response);
                    return View(TemplateList);
                }
                else
                {
                    //log response status here..
                    TemplateList = Enumerable.Empty<TemplateViewModel>();
                    ViewBag.Error = "";
                   ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View(TemplateList);
                }

            }
        }
        // GET: Template
        [HttpGet]
        public ActionResult AddTemplate()
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getAddTemplateAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            ViewBag.Error = "";
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> AddTemplate(TemplateViewModel templateViewModel)
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getAddTemplateAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(constant.apiAddress);
                    var response = await client.PostAsJsonAsync("api/TemplateApi/InsertTemplate", templateViewModel);
                    int res = (int)response.StatusCode;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Success!";
                        return Json(new { success = true });
                    }
                    else if (res.Equals(400))
                    {
                        var message = "Template with same name already exist";
                        return Json(new { success = false, error = message });
                    }
                    else
                    {
                        var message = response.ReasonPhrase;
                        return Json(new { success = false, error = message });
                    }
                }

            }
            else
            {
                var message = string.Join("|", ModelState.Values
                                            .SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage));
                return Json(new { success = false, error = message });
               
            }

        }

        [HttpGet]
        public ActionResult EditTemplate(int id)
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getEditTemplateAccess())
            {
                
                return RedirectToAction("NotAuthorized", "Response");
            }
            TemplateViewModel data = null;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/TemplateApi/GetTemplate?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var Response = result.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<TemplateViewModel>(Response);
                    ViewBag.Error = "";
                    return View(data);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View("BrandList");
                }

            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> EditTemplate(TemplateViewModel templateViewModel)
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getEditTemplateAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(constant.apiAddress);
                    var response = await client.PutAsJsonAsync("api/TemplateApi/UpdateTemplate", templateViewModel);
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Template updated";
                        return Json(new { success = true });
                    }
                    else
                    {
                        var message = response.ReasonPhrase;
                        return Json(new { success = false, error = message });
                    }
                }
            }
            else
            {
                var message = string.Join("|", ModelState.Values
                                             .SelectMany(v => v.Errors)
                                             .Select(e => e.ErrorMessage));
                return Json(new { success = false, error = message });
            }
        }

        public ActionResult Details(int id)
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getViewTemplateAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            TemplateViewModel data = null;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/TemplateApi/GetTemplate?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var Response = result.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<TemplateViewModel>(Response);
                    ViewBag.Error = "";
                    return View(data);
                }
                else
                {
                    ViewBag.Error = result.ReasonPhrase;
                    return View("TemplatesList");
                }

            }
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {

            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getDeleteTemplateAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            TemplateViewModel data = null;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/TemplateApi/GetTemplate?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var Response = result.Content.ReadAsStringAsync().Result;
                    data = JsonConvert.DeserializeObject<TemplateViewModel>(Response);
                    return View(data);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View("TemplatesList");
                }

            }
        }

        [HttpPost]
        public ActionResult Delete(TemplateViewModel templateViewModel)
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getDeleteTemplateAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            if (ModelState.IsValidField("TemplateId"))
            {
                int id = templateViewModel.TemplateId;
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(constant.apiAddress);
                    var responseTask = client.DeleteAsync("api/TemplateApi/DeleteTemplate?id=" + id.ToString());
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                         return Json(new { success = true });
                    }
                    else
                    {
                        var message = result.ReasonPhrase;
                        return Json(new { success = false, error = message });
                    }

                }

            }
            else
            {
                var message = string.Join("|", ModelState.Values
                                              .SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage));
                return Json(new { success = false, error = message });
            }

        }

        private bool getAddTemplateAccess()
        {
            int UserId = getUId();
            if (UserId == 0)
                return false;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/RolesApi/GetAddTemplateAccess?id=" + UserId.ToString());
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
        private bool getViewTemplateAccess()
        {
            int UserId = getUId();
            if (UserId == 0)
                return false;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/RolesApi/GetViewTemplateAccess?id=" + UserId.ToString());
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
        private bool getEditTemplateAccess()
        {
            int UserId = getUId();
            if (UserId == 0)
                return false;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/RolesApi/GetEditTemplateAccess?id=" + UserId.ToString());
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
        private bool getDeleteTemplateAccess()
        {
            int UserId = getUId();
            if (UserId == 0)
                return false;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/RolesApi/GetDeleteTemplateAccess?id=" + UserId.ToString());
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