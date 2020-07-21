using CMS.BL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CMS.WebApi.Controllers
{
    public class DashBoardApiController : ApiController
    {
        private IDashBoardManager _iDashBoardManager;
        public DashBoardApiController()
        {

        }
        public DashBoardApiController(IDashBoardManager dashBoardManager)
        {
            _iDashBoardManager = dashBoardManager;
        }

        [HttpGet]
        public IHttpActionResult GetBasicDashBoardDetails()
        {
            var basicDetails = _iDashBoardManager.GetBasicDashBoardDetails();
            if (basicDetails == null)
            {
                return NotFound();
            }
            return Ok(basicDetails);
        }

        [HttpGet]
        public IHttpActionResult GetCampaignByMonthOrYear()
        {
            var campaignByMonthYear = _iDashBoardManager.GetCampaignByMonthOrYear();
            if (campaignByMonthYear == null)
            {
                return NotFound();
            }
            return Ok(campaignByMonthYear);
        }

        [HttpGet]
        public IHttpActionResult GetTopFiveBrandByBudget()
        {
            var topFiveBrands = _iDashBoardManager.getTopFiveBrandByBudget();
            if (topFiveBrands == null)
            {
                return NotFound();
            }
            return Ok(topFiveBrands);
        }

        [HttpGet]
        public IHttpActionResult GetCampaignStatusList()
        {
            var statusCountList = _iDashBoardManager.GetCampaignStatusList();
            if (statusCountList == null)
            {
                return NotFound();
            }
            return Ok(statusCountList);
        }

        [HttpGet]
        public IHttpActionResult GetTopThreeResponse()
        {
            var topThreeResponse = _iDashBoardManager.GetTopThreeResponse();
            if (topThreeResponse == null)
            {
                return NotFound();
            }
            return Ok(topThreeResponse);
        }

        [HttpGet]
        public IHttpActionResult GetMarketingStatergies()
        {
            var marketingStrategies = _iDashBoardManager.getMarketingStatergies();
            if (marketingStrategies == null)
            {
                return NotFound();
            }
            return Ok(marketingStrategies);
        }

    }
}
