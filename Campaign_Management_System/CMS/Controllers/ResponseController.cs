using CMS.BE.ViewModels;
using CMS.BL.Interface;
using CMS.Common;
using CMS.Filter;
using NLog;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace CMS.Controllers
{
    [CMS_Exception]
    [HandleError(View = "Custom_Error")]

    public class ResponseController : Controller
    {
        private Constant constant = new Constant();
        public ResponseController()
        {

        }
        [HttpGet]
        public ActionResult NotAuthorized()
        {
            return View();
        }
        [HttpGet]
        public ActionResult AlreadyGivenFeedback()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ResponseSuccess()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ResponseFailed()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Feedback(string guid)
        {
            guid = Encrypt.DecryptString(guid);
            int pFrom = guid.IndexOf("CampaignId=") + "CampaignId=".Length;
            int pTo = guid.LastIndexOf("CustomerId=");

            int CampaignId = Convert.ToInt32(guid.Substring(pFrom, pTo - pFrom));

            pFrom = guid.IndexOf("CustomerId=") + "CustomerId=".Length;
            pTo = guid.LastIndexOf("CustomerEmail=");

            int CustomerId = Convert.ToInt32(guid.Substring(pFrom, pTo - pFrom));

            pFrom = guid.IndexOf("Response=") + "Response=".Length;
            pTo = guid.LastIndexOf("END");
            string Res = guid.Substring(pFrom, pTo - pFrom);

            CampaignCustomerResponse customerResponse = new CampaignCustomerResponse
            {
                CustomerID = CustomerId,
                CampaignId = CampaignId,
                Response = Res
            };

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(constant.apiAddress);
                var responseTask = client.PostAsJsonAsync("api/ResponseApi/AddCustomerResponse", customerResponse);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return View("AlreadyGivenFeedback");
                }
                else
                {

                    var Task = client.GetAsync("api/ResponseApi/UpdateGivenResponse?Res=" + Res + "&CampaignId=" + CampaignId);
                    Task.Wait();

                    var resultData = responseTask.Result;
                    if (resultData.IsSuccessStatusCode)
                    {
                        return View("ResponseSuccess");
                    }
                    else
                    {
                        return View("ResponseFailed");
                    }
                }
            }

        }
    }

}