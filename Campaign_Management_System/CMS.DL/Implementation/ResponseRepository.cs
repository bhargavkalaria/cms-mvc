using CMS.Data.Database;
using CMS.DL.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CMS.DL.Implementation
{
    public class ResponseRepository : IResponseRepository
    {
        private CMSContext cmsContext;
        public ResponseRepository()
        {
            cmsContext = new CMSContext();
        }
        public Response GetResponseDetailsById(int id)
        {
                var responses = cmsContext.Responses.Where(a => a.CampaignId.Equals(id)).FirstOrDefault();
                return responses;
        }
        public Response GetResponseDetailsByCampaignId(int id)
        {
                var responses = cmsContext.Responses.Where(a => a.CampaignId.Equals(id)).FirstOrDefault();
                return responses;
        }
        public bool UpdateResponse(Response response)
        {
                try
                {
                    var local = cmsContext.Set<Response>()
                             .Local
                             .FirstOrDefault(f => f.CampaignId == response.CampaignId);
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

        public bool AddResponseDefault(int id)
        {
                int cs = cmsContext.Customer_Campaigns.Where(a => a.CampaignId == id).Count();
                Response resposneModel = new Response
                {
                    CampaignId = id,
                    Positive = 0,
                    Negative = 0,
                    Neutral = 0,
                    NoResponse = cs
                };
                cmsContext.Responses.Add(resposneModel);
                if (cmsContext.SaveChanges() > 0)
                {
                    return true;
                }
                return false;
        }

        public IList<Response> GetResponseByDate(DateTime startDate, DateTime endDate)
        {
            List<Campaign> campaigns = new List<Campaign>();
            campaigns = cmsContext.Campaigns.Where(a => a.Start_Date >= startDate || a.Start_Date<=endDate).ToList();
            List<Response> responses = new List<Response>();

            foreach (var campaign in campaigns)
            {
                responses.Add(GetResponseDetailsById(campaign.CampaignId));
            }

            return responses;
        }

        public ResponseRate GetTotalResponse()
        {
            ResponseRate responseRate = new ResponseRate();
            double positiveCount = 0;
            double totalCount = 0;
            double responseAfterNewCampaign = 0;

            List<Response> responseList = cmsContext.Responses.ToList();
            foreach (var item in responseList)
            {
                positiveCount += item.Positive;
                totalCount = totalCount + (item.Positive + item.Negative + item.Neutral + item.NoResponse);
            }
            
            Campaign latestCampaign = cmsContext.Campaigns.Where(a => a.Start_Date <= DateTime.Now).OrderByDescending(a => a.Start_Date).FirstOrDefault();
            Response currentResponse = new Response();
            if (latestCampaign!=null)
             currentResponse = GetResponseDetailsById(latestCampaign.CampaignId);

            double positivePercentage = 0;
            if (positiveCount > 0)
            {
                positivePercentage = (positiveCount / totalCount) * 100;
                positivePercentage = Math.Round(positivePercentage, 2);
            }

            if (currentResponse.Positive > 0)
            {
                double total = currentResponse.Positive + currentResponse.Negative + currentResponse.Neutral + currentResponse.NoResponse;
                responseAfterNewCampaign = (currentResponse.Positive / total) * 100;
                responseAfterNewCampaign = Math.Round(positivePercentage, 2);
            }
            responseRate.SuccessRate = positivePercentage;
            if(latestCampaign!=null)
            responseRate.SuccessIncrease = responseAfterNewCampaign + "% increased after campaign "+ latestCampaign.CampaignName + " Started";
            return responseRate;
        }

        public IList<Response> GetAllResponses()
        {
            using (var cmsContext = new CMSContext())
            {
                return cmsContext.Responses.OrderByDescending(a => a.Positive).ToList();
            }

        }
    }
}
