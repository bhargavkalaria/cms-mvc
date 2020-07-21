using CMS.BE.ViewModels;
using CMS.BL.Interface;
using CMS.Common;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CMS.WebApi.Controllers
{
    public class QuickCampaignApiController : ApiController
    {
        private IQuickCampaignManager _iQuickCampaignManager;
        private IDataImportManager _idataImportManager;
        SendEmail se = new SendEmail();
        public QuickCampaignApiController()
        {

        }
        public QuickCampaignApiController(IQuickCampaignManager quickCampaignManager,
            IDataImportManager dataImport)
        {
            _iQuickCampaignManager = quickCampaignManager;
            _idataImportManager = dataImport;

        }

        [HttpGet]
        public IHttpActionResult GetAllQuickCampaigns()
        {
            var list = _iQuickCampaignManager.GetAllQuickCampaigns();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        [HttpPost]
        public IHttpActionResult InsertQuickCampaign([FromBody]CustomQuickCampaignVM vm)
        {
            QuickCampaignViewModel quickcampaignViewModel = vm.QuickCampaignViewModel;
            if (_iQuickCampaignManager.CheckSimilar(quickcampaignViewModel))
            {
                return BadRequest("Quick campaign with same name already exist");
            }
            else
            {
                var campAdded = _iQuickCampaignManager.AddQuickCampaign(quickcampaignViewModel);
                if (campAdded)
                {
                    int quickcampaignId = _iQuickCampaignManager.GetLatestQuickCampaignId();
                    var Temp = vm.Temp;
                    List<int> customerIds = vm.CustomerID.ToList();
                    _iQuickCampaignManager.AddToList(quickcampaignId, customerIds);
                    _iQuickCampaignManager.sendMail(quickcampaignId, customerIds, quickcampaignViewModel.TemplateId, Temp);
                    return Ok("Quick campaign " + quickcampaignViewModel.QuickCampaignName + " created successfully");
                }
                else
                {
                    return InternalServerError();
                }
            }
        }

        [HttpPost]
        public string EmailPreview([FromBody]CustomQuickCampaignVM data)
        {
            string templatedata = _iQuickCampaignManager.EmailPreview(data.QuickCampaignViewModel, data.Temp, data.CustomerID);
            return templatedata;
        }

    }
}
