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
    public class Response_QuickCampaignRepository : IResponse_QuickCampaignRepository
    {
        private CMSContext cmsContext = null;
        public Response_QuickCampaignRepository()
        {
            cmsContext = new CMSContext();
        }
        public bool AddResponseDefault(int id)
        {
            using (cmsContext = new CMSContext())
            {
                int cs = cmsContext.Customer_QuickCampaigns.Where(a => a.QuickCampaignId == id).Count();
                Response_QuickCampaign resposneModel = new Response_QuickCampaign
                {
                    QuickCampaignId = id,
                    Positive = 0,
                    Negative = 0,
                    Neutral = 0,
                    NoResponse = cs
                };
                cmsContext.Response_QuickCampaigns.Add(resposneModel);
                if (cmsContext.SaveChanges() > 0)
                {
                    return true;
                }
                return false;
            }
        }

        public Response_QuickCampaign GetResponseDetailsByQuickCampaignId(int id)
        {
            using (cmsContext = new CMSContext())
            {
                var responses = cmsContext.Response_QuickCampaigns.Where(a => a.QuickCampaignId == id).FirstOrDefault();
                return responses;
            }
        }

        public Response_QuickCampaign GetResponseQuickCampaignById(int id)
        {
            using (cmsContext = new CMSContext())
            {
                var responses = cmsContext.Response_QuickCampaigns.Find(id);
                return responses;
            }
        }

        public bool UpdateResponse(Response_QuickCampaign response)
        {
            using (cmsContext = new CMSContext())
            {
                try
                {
                    var local = cmsContext.Set<Response_QuickCampaign>()
                             .Local
                             .FirstOrDefault(f => f.QuickCampaignId == response.QuickCampaignId);
                    if (local != null)
                    {
                        cmsContext.Entry(local).State = EntityState.Detached;
                    }
                    cmsContext.Entry(response).State = EntityState.Modified;


                    //cmsContext.Entry(response).State = EntityState.Modified;
                    cmsContext.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }
    }
}
