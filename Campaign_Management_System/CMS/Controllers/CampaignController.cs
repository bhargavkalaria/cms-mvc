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
    public class CampaignController : Controller
    {
        SendEmail se = new SendEmail();
        Constant constant = new Constant();
        string baseUrl = "https://localhost:44308/";
        public CampaignController()
        {

        }
        #region Get All Campaigns 
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!(getAddCampaignAccess() || getDeleteCampaignAccess() || getEditCampaignAccess() || getViewCampaignAccess()))
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            IEnumerable<CampaignViewModel> campaigns;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                string addressUri = "api/CampaignApi/GetAllCampaigns";
                var responseTask = client.GetAsync(addressUri);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var Response = result.Content.ReadAsStringAsync().Result;
                    campaigns = JsonConvert.DeserializeObject<List<CampaignViewModel>>(Response);
                    return View(campaigns);
                }
                else
                {
                    campaigns = Enumerable.Empty<CampaignViewModel>();
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View(campaigns);
                }
            }
        }
        #endregion

        #region Add campaign Get Method
        [HttpGet]
        public ActionResult Addcampaign()
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getAddCampaignAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            return View();
        }
        #endregion

        #region Add Campaign Post method 
        [HttpPost]
        public async Task<ActionResult> Addcampaign(CustomCampaignVM jdata)
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getAddCampaignAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            if (ModelState.IsValid)
            {
                jdata.CampaignViewModel.CreatedBy = getUId();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(constant.apiAddress);
                    var response = await client.PostAsJsonAsync("api/CampaignApi/InsertCampaign", jdata);
                    int res = (int)response.StatusCode;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "Success!";
                        return Json(new { success = true });
                    }
                    else if (res.Equals(400))
                    {
                        var message = "Campaign with same name already exist";
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
                var message = string.Join("|",ModelState.Values
                                            .SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage));
                return Json(new { success = false,error =message });
            }

        }


        #endregion

        #region Edit Camapign Get Method
        [HttpGet]

        public ActionResult Edit(int id)
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getEditCampaignAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            CampaignViewModel campaign = null;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/CampaignApi/GetCampaign?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var Response = result.Content.ReadAsStringAsync().Result;
                    campaign = JsonConvert.DeserializeObject<CampaignViewModel>(Response);
                    return View(campaign);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View(campaign);
                }
            }

        }
        #endregion

        #region Edit Campaign Post method
        [HttpPost]
        public async Task<ActionResult> Edit(CustomCampaignVM jdata)
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getEditCampaignAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            if (ModelState.IsValid)
            {
                jdata.CampaignViewModel.CreatedBy = getUId();
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(constant.apiAddress);
                    var response = await client.PutAsJsonAsync("api/CampaignApi/UpdateCampaign", jdata);
                    int res = (int)response.StatusCode;
                    if (response.IsSuccessStatusCode)
                    {
                        TempData["Message"] = "SuccessEdit!";
                        return Json(new { success = true });
                    }
                    else if (res.Equals(400))
                    {
                        var message = "Campaign with same name already exist";
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
        #endregion

        #region Get Campaign Detail by Id
        [HttpGet]
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
            if (!getViewCampaignAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            CampaignViewModel campaign = null;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/CampaignApi/GetCampaign?id=" + id.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var Response = result.Content.ReadAsStringAsync().Result;
                    campaign = JsonConvert.DeserializeObject<CampaignViewModel>(Response);
                    //var rd = _irensposeManager.GetResponseById(id);
                    //List<string> data = new List<string>();
                    //data.Add(rd.Positive.ToString());
                    //data.Add(rd.Neutral.ToString());
                    //data.Add(rd.Negative.ToString());
                    //data.TrimExcess();
                    //ViewBag.rdata = data;
                    return View(campaign);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                    return View(campaign);
                }
            }
          

        }
        #endregion

        #region Delete Camapaign Get Method 
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
            if (!getDeleteCampaignAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            return View();
        }
        #endregion

        #region Delete campaign Post method
        [HttpPost]
        public ActionResult Delete(CampaignViewModel campaignViewModel)
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getDeleteCampaignAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            if (ModelState.IsValidField("CampaignId"))
            {
                int id = campaignViewModel.CampaignId;
                using (var client = new HttpClient())
                {

                    client.BaseAddress = new Uri(constant.apiAddress);
                    var responseTask = client.DeleteAsync("api/CampaignApi/DeleteCampaign?id=" + id.ToString());
                    responseTask.Wait();

                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {

                        return Json(true, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(false, JsonRequestBehavior.AllowGet);
                    }
                }

                // return Json(_icampaignManager.DeleteCampaign(campaignViewModel.CampaignId), JsonRequestBehavior.AllowGet);
            }
            else
            {
                return View(campaignViewModel.CampaignId);
            }
        }
        #endregion



        private bool getAddCampaignAccess()
        {
            int UserId = getUId();
            if (UserId == 0)
                return false;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/RolesApi/GetAddCampaignAccess?id=" + UserId.ToString());
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
        private bool getViewCampaignAccess()
        {
            int UserId = getUId();
            if (UserId == 0)
                return false;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/RolesApi/GetViewCampaignAccess?id=" + UserId.ToString());
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
        private bool getEditCampaignAccess()
        {
            int UserId = getUId();
            if (UserId == 0)
                return false;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/RolesApi/GetEditCampaignAccess?id=" + UserId.ToString());
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
        private bool getDeleteCampaignAccess()
        {
            int UserId = getUId();
            if (UserId == 0)
                return false;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/RolesApi/GetDeleteCampaignAccess?id=" + UserId.ToString());
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
 