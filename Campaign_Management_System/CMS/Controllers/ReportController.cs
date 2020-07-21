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
using System.Web.Mvc;

namespace CMS.Controllers
{

    [CMS_Exception]

    [HandleError(View = "Custom_Error")]
    public class ReportController : Controller
    {
        string baseUrl = "https://localhost:44308/";
        private Constant constant = new Constant();
        public ReportController()
        {

        }
       
        [HttpGet]
        // GET: Report
        public ActionResult GenerateReport()
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getReportAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            return View();
        }

        [HttpPost]
        public ActionResult GenerateReport(SearchViewModel searchModel)
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {

                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getReportAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            IList<ResponseCampaignViewModel> responseModel = new List<ResponseCampaignViewModel>();
            DateTime checkDate = new DateTime(0001, 01, 01, 00, 00, 00);
            if (string.IsNullOrEmpty(searchModel.Type))
            {
                searchModel.Type = "NotSpecified";
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);

                if (searchModel.StartDate != null && searchModel.EndDate != null && searchModel.StartDate != checkDate && searchModel.EndDate != checkDate && searchModel.Type.Equals("campaign"))
                {
                    var responseTask = client.GetAsync("api/ReportApi/GetCampaignReportByDate?StartDate=" + searchModel.StartDate + "&EndDate=" + searchModel.EndDate);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var Response = result.Content.ReadAsStringAsync().Result;
                        responseModel = JsonConvert.DeserializeObject<IList<ResponseCampaignViewModel>>(Response);
                    }
                    else
                    {
                        return View(searchModel);
                    }
                    if (searchModel.CId != 0)
                    {
                        bool status = false;
                        foreach (var item in responseModel)
                        {
                            if (item.CampaignId == searchModel.CId)
                            {
                                status = true;
                            }
                        }
                        if (!status)
                        {
                            var responseTask1 = client.GetAsync("api/ReportApi/GetCampaignReportById?id=" + searchModel.CId);
                            responseTask1.Wait();
                            var result1 = responseTask1.Result;
                            if (result1.IsSuccessStatusCode)
                            {
                                var Response = result1.Content.ReadAsStringAsync().Result;
                                var res = JsonConvert.DeserializeObject<ResponseCampaignViewModel>(Response);
                                responseModel.Add(res);
                            }

                        }
                    }
                    double totalPercentage = 0;
                    double totalResponses = responseModel.Count();

                    foreach (var item in responseModel)
                    {
                        totalPercentage = totalPercentage + item.percentageFor;
                    }

                    totalPercentage = totalPercentage / totalResponses;
                    totalPercentage = Math.Round(totalPercentage, 2);

                    ViewBag.TotalPercentage = totalPercentage + "%";
                }
                else if (searchModel.StartDate != null && searchModel.EndDate != null && searchModel.StartDate != checkDate && searchModel.EndDate != checkDate && searchModel.Type.Equals("quick-campaign"))
                {
                    var responseTask2 = client.GetAsync("api/ReportApi/GetQuickCampaignReportByDate?StartDate=" + searchModel.StartDate + "&EndDate=" + searchModel.EndDate);
                    responseTask2.Wait();
                    var result2 = responseTask2.Result;
                    if (result2.IsSuccessStatusCode)
                    {
                        var Response = result2.Content.ReadAsStringAsync().Result;
                        responseModel = JsonConvert.DeserializeObject<IList<ResponseCampaignViewModel>>(Response);
                    }
                    else
                    {
                        return View(searchModel);
                    }
                    if (searchModel.CId != 0)
                    {
                        bool status = false;
                        foreach (var item in responseModel)
                        {
                            if (item.CampaignId == searchModel.CId)
                            {
                                status = true;
                            }
                        }
                        if (!status)
                        {
                            var responseTask3 = client.GetAsync("api/ReportApi/GetQuickCampaignReportById?id=" + searchModel.CId);
                            responseTask3.Wait();
                            var result3 = responseTask3.Result;
                            if (result3.IsSuccessStatusCode)
                            {
                                var Response = result3.Content.ReadAsStringAsync().Result;
                                var res = JsonConvert.DeserializeObject<ResponseCampaignViewModel>(Response);
                                responseModel.Add(res);
                            }
                        }
                    }
                    double totalPercentage = 0;
                    double totalResponses = responseModel.Count();

                    foreach (var item in responseModel)
                    {
                        totalPercentage = totalPercentage + item.percentageFor;
                    }

                    totalPercentage = totalPercentage / totalResponses;
                    totalPercentage = Math.Round(totalPercentage, 2);

                    ViewBag.TotalPercentage = totalPercentage + "%";

                }
                else if (searchModel.CId != 0 && searchModel.Type.Equals("campaign"))
                {
                    ResponseCampaignViewModel res;
                    var responseTask4 = client.GetAsync("api/ReportApi/GetCampaignReportById?id=" + searchModel.CId);
                    responseTask4.Wait();
                    var result4 = responseTask4.Result;
                    if (result4.IsSuccessStatusCode)
                    {
                        var Response = result4.Content.ReadAsStringAsync().Result;
                        res = JsonConvert.DeserializeObject<ResponseCampaignViewModel>(Response);
                        responseModel.Add(res);
                        ViewBag.TotalPercentage = res.percentageFor;
                    }
                }
                else if (searchModel.CId != 0 && searchModel.Type.Equals("quick-campaign"))
                {
                    ResponseCampaignViewModel res;
                    var responseTask5 = client.GetAsync("api/ReportApi/GetQuickCampaignReportById?id=" + searchModel.CId);
                    responseTask5.Wait();
                    var result5 = responseTask5.Result;
                    if (result5.IsSuccessStatusCode)
                    {
                        var Response = result5.Content.ReadAsStringAsync().Result;
                        res = JsonConvert.DeserializeObject<ResponseCampaignViewModel>(Response);
                        responseModel.Add(res);
                        ViewBag.TotalPercentage = res.percentageFor;
                    }
                }
                else if (searchModel.StartDate == null && searchModel.EndDate == null && searchModel.Type.Equals("campaign"))
                {
                    var responseTask6 = client.GetAsync("api/ReportApi/GetCampaignReportByType");
                    responseTask6.Wait();
                    var result6 = responseTask6.Result;
                    if (result6.IsSuccessStatusCode)
                    {
                        var Response = result6.Content.ReadAsStringAsync().Result;
                        responseModel = JsonConvert.DeserializeObject<IList<ResponseCampaignViewModel>>(Response);
                    }
                    double totalPercentage = 0;
                    double totalResponses = responseModel.Count();

                    foreach (var item in responseModel)
                    {
                        totalPercentage = totalPercentage + item.percentageFor;
                    }

                    totalPercentage = totalPercentage / totalResponses;
                    totalPercentage = Math.Round(totalPercentage, 2);

                    ViewBag.TotalPercentage = totalPercentage + "%";
                }
                else if (searchModel.StartDate == null && searchModel.EndDate == null && searchModel.Type.Equals("quick-campaign"))
                {
                    var responseTask7 = client.GetAsync("api/ReportApi/GetQuickCampaignReportByType");
                    responseTask7.Wait();
                    var result7 = responseTask7.Result;
                    if (result7.IsSuccessStatusCode)
                    {
                        var Response = result7.Content.ReadAsStringAsync().Result;
                        responseModel = JsonConvert.DeserializeObject<IList<ResponseCampaignViewModel>>(Response);
                    }
                    double totalPercentage = 0;
                    double totalResponses = responseModel.Count();

                    foreach (var item in responseModel)
                    {
                        totalPercentage = totalPercentage + item.percentageFor;
                    }

                    totalPercentage = totalPercentage / totalResponses;
                    totalPercentage = Math.Round(totalPercentage, 2);

                    ViewBag.TotalPercentage = totalPercentage + "%";
                }
            }

            return Json(responseModel);
        }
     
        [HttpGet]
        public ActionResult GenerateGraph()
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getReportAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            return View();
        }

        [HttpPost]
        public ActionResult GenerateGraph(SearchViewModel searchModel)
        {
            if (!Request.IsAuthenticated || Request.Cookies["UserId"] == null)
            {
                return RedirectToAction("Login", "Login");
            }
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "MainDashboard");
            }
            if (!getReportAccess())
            {
                return RedirectToAction("NotAuthorized", "Response");
            }
            IList<ResponseCampaignViewModel> responseModel = new List<ResponseCampaignViewModel>();
            IList<string> campaignNames = new List<string>();
            IList<int> positive = new List<int>();
            IList<int> negative = new List<int>();
            IList<int> neutral = new List<int>();
            IList<int> noResponse = new List<int>();
            IList<double> percentage = new List<double>();

            DateTime checkDate = new DateTime(0001, 01, 01, 00, 00, 00);
            if (string.IsNullOrEmpty(searchModel.Type))
            {
                searchModel.Type = "NotSpecified";
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                if (searchModel.StartDate != null && searchModel.EndDate != null && searchModel.StartDate != checkDate && searchModel.EndDate != checkDate && searchModel.Type.Equals("campaign"))
                {
                    client.BaseAddress = new Uri(baseUrl);
                    var responseTask = client.GetAsync("api/ReportApi/GetCampaignReportByDate?StartDate=" + searchModel.StartDate + "&EndDate=" + searchModel.EndDate);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var Response = result.Content.ReadAsStringAsync().Result;
                        responseModel = JsonConvert.DeserializeObject<IList<ResponseCampaignViewModel>>(Response);
                    }
                    if (searchModel.CId != 0)
                    {
                        bool status = false;
                        foreach (var item in responseModel)
                        {
                            if (item.CampaignId == searchModel.CId)
                            {
                                status = true;
                            }
                        }
                        if (!status)
                        {
                            var responseTask1 = client.GetAsync("api/ReportApi/GetCampaignReportById?id=" + searchModel.CId);
                            responseTask1.Wait();
                            var result1 = responseTask1.Result;
                            if (result1.IsSuccessStatusCode)
                            {
                                var Response = result1.Content.ReadAsStringAsync().Result;
                                var res = JsonConvert.DeserializeObject<ResponseCampaignViewModel>(Response);
                                responseModel.Add(res);
                            }
                        }
                    }
                }
                else if (searchModel.StartDate != null && searchModel.EndDate != null && searchModel.StartDate != checkDate && searchModel.EndDate != checkDate && searchModel.Type.Equals("quick-campaign"))
                {
                    var responseTask2 = client.GetAsync("api/ReportApi/GetQuickCampaignReportByDate?StartDate=" + searchModel.StartDate + "&EndDate=" + searchModel.EndDate);
                    responseTask2.Wait();
                    var result2 = responseTask2.Result;
                    if (result2.IsSuccessStatusCode)
                    {
                        var Response = result2.Content.ReadAsStringAsync().Result;
                        responseModel = JsonConvert.DeserializeObject<IList<ResponseCampaignViewModel>>(Response);
                    }
                    if (searchModel.CId != 0)
                    {
                        bool status = false;
                        foreach (var item in responseModel)
                        {
                            if (item.CampaignId == searchModel.CId)
                            {
                                status = true;
                            }
                        }
                        if (!status)
                        {
                            var responseTask3 = client.GetAsync("api/ReportApi/GetQuickCampaignReportById?id=" + searchModel.CId);
                            responseTask3.Wait();
                            var result3 = responseTask3.Result;
                            if (result3.IsSuccessStatusCode)
                            {
                                var Response = result3.Content.ReadAsStringAsync().Result;
                                var res = JsonConvert.DeserializeObject<ResponseCampaignViewModel>(Response);
                                responseModel.Add(res);
                            }
                        }
                    }
                }
                else if (searchModel.CId != 0 && searchModel.Type.Equals("campaign"))
                {
                    ResponseCampaignViewModel res;
                    var responseTask4 = client.GetAsync("api/ReportApi/GetCampaignReportById?id=" + searchModel.CId);
                    responseTask4.Wait();
                    var result4 = responseTask4.Result;
                    if (result4.IsSuccessStatusCode)
                    {
                        var Response = result4.Content.ReadAsStringAsync().Result;
                        res = JsonConvert.DeserializeObject<ResponseCampaignViewModel>(Response);
                        responseModel.Add(res);
                    }
                }
                else if (searchModel.CId != 0 && searchModel.Type.Equals("quick-campaign"))
                {
                    ResponseCampaignViewModel res;
                    var responseTask5 = client.GetAsync("api/ReportApi/GetQuickCampaignReportById?id=" + searchModel.CId);
                    responseTask5.Wait();
                    var result5 = responseTask5.Result;
                    if (result5.IsSuccessStatusCode)
                    {
                        var Response = result5.Content.ReadAsStringAsync().Result;
                        res = JsonConvert.DeserializeObject<ResponseCampaignViewModel>(Response);
                        responseModel.Add(res);
                    }
                }
                else if (searchModel.StartDate == null && searchModel.EndDate == null && searchModel.Type.Equals("campaign"))
                {
                    var responseTask6 = client.GetAsync("api/ReportApi/GetCampaignReportByType");
                    responseTask6.Wait();
                    var result6 = responseTask6.Result;
                    if (result6.IsSuccessStatusCode)
                    {
                        var Response = result6.Content.ReadAsStringAsync().Result;
                        responseModel = JsonConvert.DeserializeObject<IList<ResponseCampaignViewModel>>(Response);
                    }
                }
                else if (searchModel.StartDate == null && searchModel.EndDate == null && searchModel.Type.Equals("quick-campaign"))
                {
                    var responseTask7 = client.GetAsync("api/ReportApi/GetQuickCampaignReportByType");
                    responseTask7.Wait();
                    var result7 = responseTask7.Result;
                    if (result7.IsSuccessStatusCode)
                    {
                        var Response = result7.Content.ReadAsStringAsync().Result;
                        responseModel = JsonConvert.DeserializeObject<IList<ResponseCampaignViewModel>>(Response);
                    }
                }
            }
            foreach (var item in responseModel)
            {
                positive.Add(item.Positive);
                negative.Add(item.Negative);
                neutral.Add(item.Neutral);
                noResponse.Add(item.NoResponse);
                percentage.Add(item.percentageFor);
                campaignNames.Add(item.CampaignName);
            }

            //return positive ,negative,noResponse,percentage,campaignNames
            return Json(new { 
                positive = positive,
                negative = negative,
                neutral = neutral,
                noResponse = noResponse,
                percentage = percentage,
                cName = campaignNames
            },JsonRequestBehavior.AllowGet);
        }


        private bool getReportAccess()
        {
            int UserId = getUId();
            if (UserId == 0)
                return false;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.GetAsync("api/RolesApi/GetReportAccess?id=" + UserId.ToString());
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