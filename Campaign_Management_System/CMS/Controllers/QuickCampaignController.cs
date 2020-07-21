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

namespace QuickCampaignController.Controllers
{
    [CMS_Exception]
    [HandleError(View = "Custom_Error")]
    public class QuickCampaignController : Controller
    {
        string baseUrl = "https://localhost:44308/";
        Constant constant = new Constant();
        SendEmail se = new SendEmail();
        private IDataImportManager _idataImportManager;
        public QuickCampaignController()
        {

        }
        public QuickCampaignController(IDataImportManager dataImportManager)
        {
            _idataImportManager = dataImportManager;
        }
        [HttpGet]
        public ActionResult QuickCampaignsList()
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (! ( getAddQuickCampaignAccess() || getViewQuickCampaignAccess() ))
            {
                return RedirectToAction("NotAuthorized", "Response");
            }

            IEnumerable<QuickCampaignViewModel> quickcampaigns = null;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(baseUrl);
                string addressUri = "api/QuickCampaignApi/GetAllQuickCampaigns";
                try
                {
                    var responseTask = client.GetAsync(addressUri);
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var Response = result.Content.ReadAsStringAsync().Result;
                        quickcampaigns = JsonConvert.DeserializeObject<List<QuickCampaignViewModel>>(Response);
                    }
                    else
                    {
                        //log response status here..

                        quickcampaigns = Enumerable.Empty<QuickCampaignViewModel>();

                        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    }

                }
                catch (Exception e)
                {
                    
                    throw;
                }

            }
            return View(quickcampaigns);
        }
        [HttpGet]
        public ActionResult AddQuickCampaign()
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getAddQuickCampaignAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> AddQuickCampaign(CustomQuickCampaignVM quickModel)
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getAddQuickCampaignAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            if (ModelState.IsValid)
            {
                quickModel.QuickCampaignViewModel.CreatedBy = getUId();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(baseUrl);
                    var response = await client.PostAsJsonAsync("api/QuickCampaignApi/InsertQuickCampaign", quickModel);
                    int res = (int)response.StatusCode;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Success!";
                        return Json(new { success = true });
                    }
                    else if (res.Equals(400))
                    {
                        var message = "Quick Campaign with same name already exist";
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
        private bool getTemplateAccess()
        {
            int UserId = getUId();
            if (UserId == 0)
                return false;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/RolesApi/GetTemplateAccess?id=" + UserId.ToString());
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
        private bool getViewQuickCampaignAccess()
        {
            int UserId = getUId();
            if (UserId == 0)
                return false;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/RolesApi/GetViewQuickCampaignAccess?id=" + UserId.ToString());
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
        private bool getAddQuickCampaignAccess()
        {
            int UserId = getUId();
            if (UserId == 0)
                return false;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/RolesApi/GetAddQuickCampaignAccess?id=" + UserId.ToString());
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