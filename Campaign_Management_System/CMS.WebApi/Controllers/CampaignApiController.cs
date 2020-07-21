using CMS.BE.ViewModels;
using CMS.BL.Interface;
using CMS.Common;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CMS.WebApi.Controllers
{
    public class CampaignApiController : ApiController
    {
        SendEmail se = new SendEmail();
        private ICampaignManager _icampaignManager;
        private ICustomer_CampaignManager _icustomer_CampaignManager;
        private IEmailMasterManager _iemailMasterManager;
        public CampaignApiController()
        {

        }
        public CampaignApiController(ICampaignManager icampaignManager,
            ICustomer_CampaignManager customer_CampaignManager,
            IEmailMasterManager emailMasterManager)
        {
            _icampaignManager = icampaignManager;
            _icustomer_CampaignManager = customer_CampaignManager;
            _iemailMasterManager = emailMasterManager;
        }

        [HttpGet]
        public IHttpActionResult GetAllCampaigns()
        {
            var campaigns = _icampaignManager.GetAllCampaignsForList();
            if (campaigns.Count == 0)
            {
                return NotFound();
            }
            return Ok(campaigns);
        }

        [HttpGet]
        public IHttpActionResult GetCampaign(int id)
        {
            var campaign = _icampaignManager.GetCampaignById(id);
            if (campaign == null)
            {
                return NotFound();
            }
            return Ok(campaign);
        }

        [HttpGet]
        public IHttpActionResult GetTemplate(int id)
        {
            var Template = _iemailMasterManager.GetTemplateById(id);
            if (Template == null)
            {
                return NotFound();
            }
            return Ok(Template);
        }


        [HttpGet]
        public IHttpActionResult GetAllCampaignStatuses(int id)
        {
            var campaignstatuses = _icampaignManager.GetAllCampaignStatus(id);
            if (campaignstatuses.Count == 0)
            {
                return NotFound();
            }
            return Ok(campaignstatuses);
        }

        [HttpGet]
        public IHttpActionResult GetAllStrategies()
        {
            var strategy = _icampaignManager.GetAllMarketingStrategy();
            if (strategy.Count == 0)
            {
                return NotFound();
            }
            return Ok(strategy);
        }

        [HttpGet]
        public IHttpActionResult GetAllMTypes()
        {
            var mtype = _icampaignManager.GetAllMarketingType();
            if (mtype.Count == 0)
            {
                return NotFound();
            }
            return Ok(mtype);
        }

        [HttpPost]
        public string EmailPreview([FromBody]CustomCampaignVM data)
        {
            string templatedata = _icampaignManager.EmailPreview(data.CampaignViewModel, data.Temp, data.CustomerID);
            return templatedata;
        }

        [HttpPost]
        public IHttpActionResult InsertCampaign([FromBody]CustomCampaignVM data)
        {
            CampaignViewModel campaignViewModel = data.CampaignViewModel;
            if (_icampaignManager.CheckSimilar(campaignViewModel))
            {
                return BadRequest();
            }
            else
            {
                var campAdded = _icampaignManager.AddCampaign(campaignViewModel);
                if (campAdded)
                {
                    int campaignId = _icampaignManager.GetLatestCampaignId();
                    var Temp = data.Temp;
                    List<int> customerIds = data.CustomerID.ToList();
                    _icustomer_CampaignManager.AddToList(campaignId, customerIds, Temp);
                    //se.SendCampaignMailAsync(campaignId, data.CampaignViewModel.TemplateId);
                    return Ok("Campaign "+campaignViewModel.CampaignName+" created successfully");
                }
                else
                {
                    return InternalServerError();
                }
            }
            
        }
        [HttpPut]
        public IHttpActionResult UpdateCampaign(CustomCampaignVM data)
        {
            CampaignViewModel campaignViewModel = data.CampaignViewModel;
            if (_icampaignManager.CheckSimilar(campaignViewModel))
            {
                return BadRequest();
            }
            else
            {
                var campAdded = _icampaignManager.EditCampaign(campaignViewModel);
                if (campAdded)
                {
                    int campaignId = campaignViewModel.CampaignId;
                    //var cs = data["CustomerID"].Cast<int>().ToList();
                    var Temp = data.Temp;
                    List<int> customerIds = data.CustomerID.ToList();
                    _icustomer_CampaignManager.AddToList(campaignId, customerIds, Temp);
                    return Ok("Campaign " + campaignViewModel.CampaignName + " updated successfully");
                }
                else
                {
                    return InternalServerError();
                }
            }
            
        }

        [HttpDelete]
        public IHttpActionResult DeleteCampaign(int id)
        {
            if (_icampaignManager.DeleteCampaign(id))
            {
                return Ok("Campaign deleted successfully");
            }
            else
            {
                return InternalServerError();
            }
        }

    }
}

