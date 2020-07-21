using CMS.Data.Database;
using CMS.DL.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CMS.DL.Implementation
{
    public class Customer_CampaignRepository : ICustomer_CampaignRepository
    {
        private CMSContext cmsContext = new CMSContext();
        private IResponseRepository _responseRepository;

        public Customer_CampaignRepository()
        {

        }
        public Customer_CampaignRepository(IResponseRepository responseRepository)
        {
            _responseRepository = responseRepository;
            cmsContext = new CMSContext();
        }
        public bool AddToList(int campaignId, List<int> customerIds, string Temp)
        {
            bool status = false;
            int c = 0;
            foreach (var item in customerIds)
            {
                Customer_Campaign cc1 = new Customer_Campaign();
                var cust = cmsContext.Customer_Campaigns.Where(x => x.CampaignId == campaignId && x.CustomerID == item).FirstOrDefault();
                if (cust == null)
                {
                    cc1.CustomerID = item;
                    cc1.CampaignId = campaignId;
                    if (Temp != null)
                    {
                        cc1.CustomTemplate = Temp;
                    }
                    cc1.Response = "NoResponse";
                    cmsContext.Customer_Campaigns.Add(cc1);
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
            if (!_responseRepository.AddResponseDefault(campaignId))
            {
                status = false;
            }
            return status;
        }

        public List<Customer_Campaign> GetCustomerListByCampaignId(int id)
        {
            var ccList = cmsContext.Customer_Campaigns.Where(x => x.CampaignId == id).ToList();
            return ccList;
        }
        public int AddUserResponse(Customer_Campaign customerResponse)
        {
            var userResponse = cmsContext.Customer_Campaigns.Where(x => x.CampaignId == customerResponse.CampaignId && x.CustomerID == customerResponse.CustomerID).FirstOrDefault();
            if ((userResponse.Response != "NoResponse" && userResponse.Response != ""))
            {
                return 2; //Already Response Provided
            }
            userResponse.Response = customerResponse.Response;
            try
            {
                var local = cmsContext.Set<Customer_Campaign>()
                         .Local
                         .FirstOrDefault(f => f.Id == userResponse.Id);
                if (local != null)
                {
                    cmsContext.Entry(local).State = EntityState.Detached;
                }
                cmsContext.Entry(userResponse).State = EntityState.Modified;


                //cmsContext.Entry(response).State = EntityState.Modified;
                cmsContext.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public List<int> GetCustomerIdsListByCampaignId(int id)
        {
            return cmsContext.Customer_Campaigns.Where(x => x.CampaignId == id).Select(x => x.CustomerID).ToList();
        }

        public bool ChangeEmailStatus(int id)
        {
            bool status = false;
            int c = 0;
            var cc = cmsContext.Customer_Campaigns.Where(x => x.Id == id).FirstOrDefault();
            if (cc != null )
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

        public Customer_Campaign getCustomerByCampaignIdAndByCustomerId(int CampaignId, int CustomerId)
        {
            var cc = cmsContext.Customer_Campaigns.Where(x => x.CampaignId == CampaignId && x.CustomerID == CustomerId).FirstOrDefault();
            return cc;
        }
    }
}

