using CMS.BE.ViewModels;
using CMS.BL.Interface;
using CMS.Common;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CMS.WebApi.Controllers
{
    [EnableCors(origins: "https://localhost:44308", headers: "*", methods: "*")]
    public class ResponseApiController : ApiController
    {
        private IResponseManager _iResponseManager;
        private ICustomer_CampaignManager _iCustomerCampaign;
        private IResponse_QuickCampaignManager _iQuickResponseManager;
        private ICustomer_QuickCampaignManager _iCustomerQuickCampaign;
        public ResponseApiController()
        {

        }
        public ResponseApiController(IResponseManager responseManager, ICustomer_CampaignManager customer_CampaignManager,IResponse_QuickCampaignManager iQuickResponseManager,ICustomer_QuickCampaignManager iCustomerQuickCampaignManager)
        {
            _iResponseManager = responseManager;
            _iCustomerQuickCampaign = iCustomerQuickCampaignManager;
            _iCustomerCampaign = customer_CampaignManager;
            _iQuickResponseManager = iQuickResponseManager;
        }
        [HttpGet]
        public HttpResponseMessage QuickFeedback(string guid)
        {
            guid = Encrypt.DecryptString(guid);
            int pFrom = guid.IndexOf("QuickCampaignId=") + "QuickCampaignId=".Length;
            int pTo = guid.LastIndexOf("CustomerId=");

            int CampaignId = Convert.ToInt32(guid.Substring(pFrom, pTo - pFrom));

            pFrom = guid.IndexOf("CustomerId=") + "CustomerId=".Length;
            pTo = guid.LastIndexOf("CustomerEmail=");

            int CustomerId = Convert.ToInt32(guid.Substring(pFrom, pTo - pFrom));

            pFrom = guid.IndexOf("Response=") + "Response=".Length;
            pTo = guid.LastIndexOf("END");
            string Res = guid.Substring(pFrom, pTo - pFrom);

            Customer_QuickCampaignViewModel customerResponse = new Customer_QuickCampaignViewModel
            {
                CustomerID = CustomerId,
                QuickCampaignId = CampaignId,
                Response = Res
            };
            int responseStatus = _iCustomerQuickCampaign.AddCustomerResponse(customerResponse);
            if (responseStatus == 2)
            {
                var response = Request.CreateResponse(HttpStatusCode.Found);
                //response.Headers.Location = new Uri("https://localhost:53814");
                //return ("AlreadyGivenFeedback");
                response.Headers.Location = new Uri("http://localhost:53814/Response/AlreadyGivenFeedback");
                return response;
            }
            bool Status;
            Status = _iQuickResponseManager.UpdateGivenResponse(Res, CampaignId);
            if (Status)
            {
                var response = Request.CreateResponse(HttpStatusCode.Found);
                response.Headers.Location = new Uri("http://localhost:53814/Response/ResponseSuccess");
                // response.Headers.Location = new Uri(Url.Route("ResponseSuccess", "Response"));
                return response;
                //return View("ResponseSuccess");
            }
            else
            {
                var response = Request.CreateResponse(HttpStatusCode.Found);
                //response.Headers.Location = new Uri("http://localhost:53814/Response/ResponseFailed");
                ////response.Headers.Location = new Uri(Url.Route("ResponseFailed", "Response"));
                return response;
                //return View("ResponseFailed");
            }
        }

        [HttpGet]
        public HttpResponseMessage Feedback(string guid)
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
            int responseStatus = _iCustomerCampaign.AddCustomerResponse(customerResponse);
            if (responseStatus == 2)
            {
                var response = Request.CreateResponse(HttpStatusCode.Found);
                //response.Headers.Location = new Uri("https://localhost:53814");
                //return ("AlreadyGivenFeedback");
                response.Headers.Location = new Uri("http://localhost:53814/Response/AlreadyGivenFeedback");
                return response;
            }
            bool Status;
            Status = _iResponseManager.UpdateGivenResponse(Res, CampaignId);
            if (Status)
            {
                var response = Request.CreateResponse(HttpStatusCode.Found);
                response.Headers.Location = new Uri("http://localhost:53814/Response/ResponseSuccess");
                // response.Headers.Location = new Uri(Url.Route("ResponseSuccess", "Response"));
                return response;
                //return View("ResponseSuccess");
            }
            else
            {
                var response = Request.CreateResponse(HttpStatusCode.Found);
                //response.Headers.Location = new Uri("http://localhost:53814/Response/ResponseFailed");
                ////response.Headers.Location = new Uri(Url.Route("ResponseFailed", "Response"));
                return response;
                //return View("ResponseFailed");
            }
        }

        public IHttpActionResult GetResponseByCampaignId(int id)
        {
            var res = _iResponseManager.GetResponseById(id);
            if(res == null)
            {
                return NotFound();
            }
            return Ok(res);
        }
    }
}
