using CMS.BE.ViewModels;
using CMS.BL.Interface;
using CMS.Common;
using CMS.Filter;
using NLog;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Mvc;

namespace CMS.Controllers
{
    [CMS_Exception]
    [HandleError(View = "Custom_Error")]
    public class MainDashboardController : Controller
    {
        Constant constant = new Constant();
        public MainDashboardController()
        {

        }
       
        // GET: MainDashboard
        public ActionResult Index()
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            setAccessSession();
            return View();
        }

        public ActionResult GetDashboardData()
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }

            DashboardGraphViewModel basicDetails = new DashboardGraphViewModel();
            List<CampaignMonthYearViewModel> campaignByMonthYear = new List<CampaignMonthYearViewModel>();
            List<DashboardBrandViewModel> topFiveBrands = new List<DashboardBrandViewModel>();
            List<CampaignCountByStatusViewModel> statusCountList = new List<CampaignCountByStatusViewModel>();
            List<ResponseCampaignViewModel> topThreeResponse = new List<ResponseCampaignViewModel>();
            List<MarketingStrategyViewModel> marketingStrategies = new List<MarketingStrategyViewModel>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/DashBoardApi/GetBasicDashBoardDetails");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var Response = result.Content.ReadAsStringAsync().Result;
                    basicDetails = JsonConvert.DeserializeObject<DashboardGraphViewModel>(Response);
                }

                var responseTask1 = client.GetAsync("api/DashBoardApi/GetCampaignByMonthOrYear");
                responseTask1.Wait();

                var result1 = responseTask1.Result;
                if (result1.IsSuccessStatusCode)
                {
                    var Response1 = result1.Content.ReadAsStringAsync().Result;
                    campaignByMonthYear = JsonConvert.DeserializeObject<List<CampaignMonthYearViewModel>>(Response1);
                }

                var responseTask2 = client.GetAsync("api/DashBoardApi/GetTopFiveBrandByBudget");
                responseTask2.Wait();
                var result2 = responseTask2.Result;
                if (result2.IsSuccessStatusCode)
                {
                    var Response2 = result2.Content.ReadAsStringAsync().Result;
                    topFiveBrands = JsonConvert.DeserializeObject<List<DashboardBrandViewModel>>(Response2);
                }

                var responseTask3 = client.GetAsync("api/DashBoardApi/GetCampaignStatusList");
                responseTask3.Wait();
                var result3 = responseTask3.Result;
                if (result3.IsSuccessStatusCode)
                {
                    var Response3 = result3.Content.ReadAsStringAsync().Result;
                    statusCountList = JsonConvert.DeserializeObject<List<CampaignCountByStatusViewModel>>(Response3);
                }

                var responseTask4 = client.GetAsync("api/DashBoardApi/GetTopThreeResponse");
                responseTask4.Wait();
                var result4 = responseTask4.Result;
                if (result4.IsSuccessStatusCode)
                {
                    var Response4 = result4.Content.ReadAsStringAsync().Result;
                    topThreeResponse = JsonConvert.DeserializeObject<List<ResponseCampaignViewModel>>(Response4);
                }

                var responseTask5 = client.GetAsync("api/DashBoardApi/GetMarketingStatergies");
                responseTask5.Wait();
                var result5 = responseTask5.Result;
                if (result5.IsSuccessStatusCode)
                {
                    var Response5 = result5.Content.ReadAsStringAsync().Result;
                    marketingStrategies = JsonConvert.DeserializeObject<List<MarketingStrategyViewModel>>(Response5);
                }

            }

                return Json(new { basicDetails = basicDetails,
                campaignByMonthYear = campaignByMonthYear,
                topFiveBrands = topFiveBrands,
                statusCountList = statusCountList,
                topThreeResponse = topThreeResponse,
                marketingStrategies = marketingStrategies
            }, JsonRequestBehavior.AllowGet);
        }
        private bool setAccessSession()
        {
            int UserId = getUId();
            UserViewModel currentUser;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/RolesApi/GetUser?id=" + UserId.ToString());
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var Response = result.Content.ReadAsStringAsync().Result;
                    currentUser = JsonConvert.DeserializeObject<UserViewModel>(Response);
                    Session["UserName"] = currentUser.FName;
                    if (currentUser.addBrandAccess)
                    {
                        Session["addBrandAccess"] = "yes";
                    }
                    else if (!currentUser.addBrandAccess)
                    {
                        Session["addBrandAccess"] = null;
                    }
                    if (currentUser.addCampaignAccess)
                    {
                        Session["addCampaignAccess"] = "yes";
                    }
                    else if (!currentUser.addCampaignAccess)
                    {
                        Session["addCampaignAccess"] = null;
                    }
                    if (currentUser.addQuickCampaignAccess)
                    {
                        Session["addQuickCampaignAccess"] = "yes";
                    }
                    else if (!currentUser.addQuickCampaignAccess)
                    {
                        Session["addQuickCampaignAccess"] = null;
                    }
                    if (currentUser.addTemplateAccess)
                    {
                        Session["addTemplateAccess"] = "yes";
                    }
                    else if (!currentUser.addTemplateAccess)
                    {
                        Session["addTemplateAccess"] = null;
                    }
                    if (currentUser.addUserAccess)
                    {
                        Session["addUserAccess"] = "yes";
                    }
                    else if (!currentUser.addUserAccess)
                    {
                        Session["addUserAccess"] = null;
                    }
                    if (currentUser.deleteBrandAccess)
                    {
                        Session["deleteBrandAccess"] = "yes";
                    }
                    else if (currentUser.deleteBrandAccess)
                    {
                        Session["deleteBrandAccess"] = null;
                    }
                    if (currentUser.deleteCampainAccess)
                    {
                        Session["deleteCampainAccess"] = "yes";
                    }
                    else if (!currentUser.deleteCampainAccess)
                    {
                        Session["deleteCampainAccess"] = null;
                    }
                    if (currentUser.deleteTemplateAccess)
                    {
                        Session["deleteTemplateAccess"] = "yes";
                    }
                    else if (!currentUser.deleteTemplateAccess)
                    {
                        Session["deleteTemplateAccess"] = null;
                    }
                    if (currentUser.editBrandAccess)
                    {
                        Session["editBrandAccess"] = "yes";
                    }
                    else if (!currentUser.editBrandAccess)
                    {
                        Session["editBrandAccess"] = null;
                    }
                    if (currentUser.editCampaignAccess)
                    {
                        Session["editCampaignAccess"] = "yes";
                    }
                    else if (!currentUser.editCampaignAccess)
                    {
                        Session["editCampaignAccess"] = null;
                    }
                    if (currentUser.editTemplateAccess)
                    {
                        Session["editTemplateAccess"] = "yes";
                    }
                    else if (!currentUser.editTemplateAccess)
                    {
                        Session["editTemplateAccess"] = null;
                    }
                    if (currentUser.hasReportAccess)
                    {
                        Session["hasReportAccess"] = "yes";
                    }
                    else if (!currentUser.hasReportAccess)
                    {
                        Session["hasReportAccess"] = null;
                    }
                    if (currentUser.uploadCustomerAccess)
                    {
                        Session["uploadCustomerAccess"] = "yes";
                    }
                    else if (!currentUser.uploadCustomerAccess)
                    {
                        Session["uploadCustomerAccess"] = null;
                    }
                    if (currentUser.viewBrandAccess)
                    {
                        Session["viewBrandAccess"] = "yes";
                    }
                    else if (!currentUser.viewBrandAccess)
                    {
                        Session["viewBrandAccess"] = null;
                    }
                    if (currentUser.viewCampaignAccess)
                    {
                        Session["viewCampaignAccess"] = "yes";
                    }
                    else if (!currentUser.viewCampaignAccess)
                    {
                        Session["viewCampaignAccess"] = null;
                    }
                    if (currentUser.viewQuickCampaignAccess)
                    {
                        Session["viewQuickCampaignAccess"] = "yes";
                    }
                    else if (!currentUser.viewQuickCampaignAccess)
                    {
                        Session["viewQuickCampaignAccess"] = null;
                    }
                    if (currentUser.viewTemplateAccess)
                    {
                        Session["viewTemplateAccess"] = "yes";
                    }
                    else if (!currentUser.viewTemplateAccess)
                    {
                        Session["viewTemplateAccess"] = null;
                    }
                    if (currentUser.Role.Equals("SUPERADMIN"))
                    {
                        Session["accesUser"] = "yes";
                    }
                    else if (!currentUser.Role.Equals("SUPERADMIN"))
                    {
                        Session["accesUser"] = null;
                    }
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
         //   logger.Info("UserID");
            int UserId = Convert.ToInt32(decUId.Substring(pFrom, pTo - pFrom));
          
            return UserId;
        }
    }
}