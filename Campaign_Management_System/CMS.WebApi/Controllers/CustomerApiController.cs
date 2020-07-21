using CMS.BL.Interface;
using System.Web;
using System.Web.Http;

namespace CMS.WebApi.Controllers
{
    
    public class CustomerApiController : ApiController
    {
        private IDataImportManager _idataImportManager;
        private ICustomer_CampaignManager _icustomer_CampaignManager;
        public CustomerApiController()
        {

        }
        public CustomerApiController(IDataImportManager dataImportManager,ICustomer_CampaignManager customer_CampaignManager)
        {
            _idataImportManager = dataImportManager;
            _icustomer_CampaignManager = customer_CampaignManager;
        }
        [HttpGet]
        public IHttpActionResult GetAllCustomers()
        {
            var customers = _idataImportManager.GetAllCustomers();
            if (customers == null)
            {
                return NotFound();
            }
            return Ok(customers);
        }

        [HttpGet]
        public IHttpActionResult GetCustomersByCampaignId(int campaignId)
        {
            var customers = _icustomer_CampaignManager.getCustomersListByCampaignId(campaignId);
            if (customers == null)
            {
                return NotFound();
            }
            return Ok(customers);
        }
        [HttpGet]
        public IHttpActionResult ReadAndSaveExcel(HttpPostedFileBase postedFile)
        {
            string status = _idataImportManager.ReadAndSaveExcel(postedFile);
            return Ok(status);
        }
    }
}
