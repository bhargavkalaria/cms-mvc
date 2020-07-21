using CMS.BL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CMS.WebApi.Controllers
{
    public class ReportApiController : ApiController
    {
        private IResponseManager _iResponseManager;
        private ICampaignManager _iCampaignManager;
        private ICustomer_CampaignManager _icustomer_CampaignManager;
        private ICustomerManager _iCustomerManager;
        private ICustomer_QuickCampaignManager _iCustomerQuick;
        private IResponse_QuickCampaignManager _iQuickResponse;

        public ReportApiController(IResponseManager iResponseManager, ICampaignManager iCampaignManager,
            ICustomer_CampaignManager customer_CampaignManager, ICustomerManager iCustomerManager,
            IResponse_QuickCampaignManager iQuickResponse, ICustomer_QuickCampaignManager iCustomerQuickCampaign)
        {
            _iCampaignManager = iCampaignManager;
            _iResponseManager = iResponseManager;
            _icustomer_CampaignManager = customer_CampaignManager;
            _iCustomerManager = iCustomerManager;
            _iCustomerQuick = iCustomerQuickCampaign;
            _iQuickResponse = iQuickResponse;
        }
        [HttpGet]
        public IHttpActionResult GetCampaignReportByDate(DateTime StartDate, DateTime EndDate)
        {
            var list = _iResponseManager.GetReportByDate(StartDate,EndDate);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        public IHttpActionResult GetCampaignReportById(int id)
        {
            var list = _iResponseManager.GetReportById(id);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        public IHttpActionResult GetCampaignReportByType()
        {
            var list = _iResponseManager.GetReportByType();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        [HttpGet]
        public IHttpActionResult GetQuickCampaignReportByDate(DateTime StartDate, DateTime EndDate)
        {
            var list = _iQuickResponse.GetReportByDate(StartDate, EndDate);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        public IHttpActionResult GetQuickCampaignReportById(int id)
        {
            var list = _iQuickResponse.GetReportById(id);
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        public IHttpActionResult GetQuickCampaignReportByType()
        {
            var list = _iQuickResponse.GetReportByType();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

    }
}
