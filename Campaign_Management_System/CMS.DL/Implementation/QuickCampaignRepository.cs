using CMS.Data.Database;
using CMS.DL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;


namespace CMS.DL.Implementation
{
    public class QuickCampaignRepository : IQuickCampaignRepository
    {
        private CMSContext cmsContext;
        private IResponse_QuickCampaignRepository _iquickResponse;
        public QuickCampaignRepository(IResponse_QuickCampaignRepository iQuickResponse)
        {
            _iquickResponse = iQuickResponse;
            cmsContext = new CMSContext();
        }
        bool status = false;
        public bool AddQuickCampaign(QuickCampaign quickModel)
        {
            bool status = false;
            try
            {
               cmsContext.QuickCampaigns.Add(quickModel);
                int c = cmsContext.SaveChanges();
                if (c > 0)
                {
                    status = true;
                }
                return status;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public int GetLatestQuickCampaignId()
        {
            return cmsContext.QuickCampaigns.Max(x => x.QuickCampaignId);
        }



        public bool AddToList(int quickcampaignId, List<int> customerIds)
        {
            int c = 0;
            Customer_QuickCampaign cc = new Customer_QuickCampaign();
            foreach (var item in customerIds)
            {
                cc.CustomerID = item;
                cc.QuickCampaignId = quickcampaignId;
                cc.Response = "NoResponse";
                cmsContext.Customer_QuickCampaigns.Add(cc);
                c = cmsContext.SaveChanges();
            }

            if (c > 0)
            {
                status = true;
            }
            if (!_iquickResponse.AddResponseDefault(quickcampaignId))
            {
                status = false;
            }
            return status;
        }


        public List<int> GetCustomerIdsListByQuickCampaignId(int id)
        {
            return cmsContext.Customer_QuickCampaigns.Where(x => x.QuickCampaignId == id).Select(x => x.CustomerID).ToList();

        }
        

        public List<QuickCampaign> GetAllQuickCampaigns()
        {
            return cmsContext.QuickCampaigns.ToList();
        }

        public QuickCampaign GetQuickCampaignById(int id)
        {
            return cmsContext.QuickCampaigns.Find(id);
        }
        public IList<QuickCampaign> GetCampaignByDate(DateTime startDate, DateTime endDate)
        {
            List<QuickCampaign> campaigns = new List<QuickCampaign>();
            campaigns = cmsContext.QuickCampaigns.Where(a => a.Start_Date >= startDate && a.Start_Date <= endDate).ToList();
            return campaigns;
        }

        public bool CheckSimilar(QuickCampaign quickCampaign)
        {
            bool status = false;
            if (quickCampaign.QuickCampaignId != 0)
            {
                var checkId = cmsContext.QuickCampaigns.Where(x => x.QuickCampaignId == quickCampaign.QuickCampaignId).FirstOrDefault();
                if (quickCampaign.QuickCampaignName == checkId.QuickCampaignName)
                {
                    return status;
                }
                else
                {
                    var list = cmsContext.QuickCampaigns.Where(x => x.QuickCampaignName == quickCampaign.QuickCampaignName).FirstOrDefault();
                    if (list != null)
                    {
                        status = true;
                    }
                }

            }
            else
            {
                var duplicate = cmsContext.QuickCampaigns.Where(x => x.QuickCampaignName == quickCampaign.QuickCampaignName).FirstOrDefault();
                if (duplicate != null)
                {
                    status = true;
                }
            }
            return status;
        }
    }
}
