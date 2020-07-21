using CMS.Data.Database;
using CMS.DL.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.DL.Implementation
{
    public class Customer_QuickCampaignRepository : ICustomer_QuickCampaignRepository
    {
        private CMSContext cmsContext;
        private IResponse_QuickCampaignRepository  _quickResponseRepository;
        public Customer_QuickCampaignRepository()
        {

        }
        public Customer_QuickCampaignRepository(IResponse_QuickCampaignRepository response_QuickCampaignRepository)
        {
            _quickResponseRepository = response_QuickCampaignRepository;
            cmsContext = new CMSContext();
        }
        public bool AddToList(int quickcampaignId, List<int> customerIds, string Temp)
        {
            int c = 0;
            bool status = false;
            foreach (var item in customerIds)
            {
                Customer_QuickCampaign cc1 = new Customer_QuickCampaign();
                var cust = cmsContext.Customer_QuickCampaigns.Where(x => x.QuickCampaignId == quickcampaignId && x.CustomerID == item).FirstOrDefault();
                if (cust == null)
                {
                    cc1.CustomerID = item;
                    cc1.QuickCampaignId = quickcampaignId;
                    cc1.Response = "NoResponse";
                    cmsContext.Customer_QuickCampaigns.Add(cc1);
                    c = cmsContext.SaveChanges();
                }
                else
                {
                    continue;
                }
            }

            if (c > 0)
            {
                status = true;
            }
            if (!_quickResponseRepository.AddResponseDefault(quickcampaignId))
            {
                status = false;
            }
            return status;
        }

        public int AddUserResponse(Customer_QuickCampaign customerResponse)
        {
            var userResponse = cmsContext.Customer_QuickCampaigns.Where(x => x.QuickCampaignId == customerResponse.QuickCampaignId && x.CustomerID == customerResponse.CustomerID).FirstOrDefault();
            if ((userResponse.Response != "NoResponse" && userResponse.Response != ""))
            {
                return 2; //Already Response Provided
            }
            userResponse.Response = customerResponse.Response;
            try
            {
                var local = cmsContext.Set<Customer_QuickCampaign>()
                         .Local
                         .FirstOrDefault(f => f.Id == userResponse.Id);
                if (local != null)
                {
                    cmsContext.Entry(local).State = EntityState.Detached;
                }
                cmsContext.Entry(userResponse).State = EntityState.Modified;
                cmsContext.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public List<int> GetCustomerIdsListByQuickCampaignId(int id)
        {
            return cmsContext.Customer_QuickCampaigns.Where(x => x.QuickCampaignId == id).Select(x => x.CustomerID).ToList();
        }

        public List<Customer_QuickCampaign> GetCustomerListByQuickCampaignId(int id)
        {
            var cqList = cmsContext.Customer_QuickCampaigns.Where(x => x.QuickCampaignId == id).ToList();
            return cqList;
        }

        public bool ChangeEmailStatus(int qid, int customerId)
        {
            bool status = false;
            int c = 0;
            var cc = cmsContext.Customer_QuickCampaigns.Where(x => x.QuickCampaignId == qid && x.CustomerID == customerId).FirstOrDefault();
            if (cc != null)
            {
                cc.isEmailSent = true;
                c = cmsContext.SaveChanges();
            }
            if (c > 0)
            {
                status = true;
            }
            return status;
        }
    }
}
