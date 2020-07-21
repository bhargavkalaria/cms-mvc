using CMS.Data.Database;
using CMS.DL.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CMS.DL.Implementation
{
    public class CampaignRepository : ICampaignRepository
    {
        private CMSContext cmsContext;
        private IResponseRepository _responseRepository;
        

        public CampaignRepository(IResponseRepository responseRepository)
        {
            _responseRepository = responseRepository;
            cmsContext = new CMSContext();
        }
        public List<Campaign> GetAllCampaigns()
        {
            List<Campaign> campaigns = new List<Campaign>();
            try
            {
                campaigns = cmsContext.Campaigns.Include(x => x.Brand).Include(x => x.CampaignStatus)
                    .Include(x => x.MarketingStrategy).Include(x => x.MarketingType).
                    Include(x => x.Template).ToList();
                // campaigns = cmsContext.Campaigns.ToList();
                //campaigns = (from c in cmsContext.Campaigns
                //             join s in cmsContext.CampaignStatuses on c.CampaignStatusId equals s.CampaignStatusId
                //             join mt in cmsContext.MarketingTypes on c.MarketingTypeId equals mt.MarketingTypeId
                //             join ms in cmsContext.MarketingStrategies on c.MarketingStrategyId equals ms.StrategyId
                //             join t in cmsContext.Templates on c.TemplateId equals t.TemplateId
                //             join o in cmsContext.brands on c.BrandId equals o.BrandId
                //             orderby c.CreatedBy
                //             select c).ToList();
            }
            catch (Exception e)
            {
            }
            return campaigns;
        }

        public bool AddCampaign(Campaign campaign)
        {
            bool status = false;
            cmsContext.Campaigns.Add(campaign);
          
                int c = cmsContext.SaveChanges();
                if (c > 0)
                {
                    status = true;
                }
            return status;

        }

        public List<CampaignStatus> GetAllCampaignStatus()
        {
            return cmsContext.CampaignStatuses.ToList();
        }

        public List<MarketingStrategy> GetAllMarketingStrategy()
        {
            return cmsContext.MarketingStrategies.ToList();
        }

        public List<MarketingType> GetAllMarketingTypes()
        {
            return cmsContext.MarketingTypes.ToList();


        }

        public CampaignRepository()
        {
            _responseRepository = new ResponseRepository();
            cmsContext = new CMSContext();
        }
        public bool EditCampaign(Campaign campaign)
        {

            bool status = false;
            Campaign cm = new Campaign();
            cm = campaign;
            var local = cmsContext.Set<Campaign>()
                       .Local
                       .FirstOrDefault(f => f.CampaignId == campaign.CampaignId);
            if (local != null)
            {
                cmsContext.Entry(local).State = EntityState.Detached;
            }
            cmsContext.Entry(campaign).State = EntityState.Modified;

            if (cmsContext.SaveChanges() > 0)
            {
                status = true;
            }
            return status;

        }

        public Campaign GetCampaignByid(int id)
        {
            return cmsContext.Campaigns.
              Where(x => x.CampaignId == id).FirstOrDefault();
        }

        public bool DeleteCampaign(int id)
        {
            int c = 0;
            bool status = false;
            Campaign cm = new Campaign();
            
            cm = GetCampaignByid(id);
            cm.isDeleted = true;
            var local = cmsContext.Set<Campaign>()
                       .Local
                       .FirstOrDefault(f => f.CampaignId == id);
            if (local != null)
            {
                cmsContext.Entry(local).State = EntityState.Detached;
            }
            cmsContext.Entry(cm).State = EntityState.Modified;

            c = cmsContext.SaveChanges();
            if (c > 0)
            {
                status = true;
            }
            return status;
        }

        public string GetCampaignStatusById(int id)
        {
            CampaignStatus cm = new CampaignStatus();
            cm = cmsContext.CampaignStatuses.Where(a => a.CampaignStatusId == id).FirstOrDefault();
            return cm.Status;
        }

        public string GetMarketingStrategyById(int id)
        {
            MarketingStrategy ms = new MarketingStrategy();
            ms = cmsContext.MarketingStrategies.Where(x => x.MarketingStrategyId == id).FirstOrDefault();
            return ms.StrategyName;
        }

        public string GetMarketingTypeById(int id)
        {

            MarketingType mt = new MarketingType();
            mt = cmsContext.MarketingTypes.Where(x => x.MarketingTypeId == id).FirstOrDefault();
            return mt.MarketingTypeName;
        }

        public int GetLatestCampaignId()
        {
            return cmsContext.Campaigns.Max(x => x.CampaignId);
        }

        public IList<Campaign> GetCampaignByDate(DateTime startDate, DateTime endDate)
        {
            List<Campaign> campaigns = new List<Campaign>();
            campaigns = cmsContext.Campaigns.Where(a => a.Start_Date >= startDate && a.Start_Date <= endDate).ToList();
            return campaigns;
        }

        public TotalCampaign GetCampaignTotal()
        {
            DateTime checkTime = DateTime.Now.AddMonths(-1);
            double totalCampaigns = cmsContext.Campaigns.Count();
            double newCampaigns = cmsContext.Campaigns.Where(a => a.CreatedOn <= checkTime).Count();

            double newPercentage = 0;
            if (newCampaigns > 0)
            {
                newPercentage = (newCampaigns / totalCampaigns) * 100;
                newPercentage = Math.Round(newPercentage, 2);
            }

            double activeCampaignCount = cmsContext.Campaigns.Where(a => a.CampaignStatusId==2).Count();
            double activeIncrease = cmsContext.Campaigns.Where(a=>a.CampaignStatusId==2 && a.Start_Date >= checkTime).Count();
            double activePercentage = 0;
            if (activeIncrease > 0)
            {
                activePercentage = (activeIncrease / activeCampaignCount) * 100;
                activePercentage = Math.Round(activePercentage, 2);
            }

            TotalCampaign campaignTotal = new TotalCampaign();
            campaignTotal.CampaignCount = Convert.ToInt32(totalCampaigns);
            campaignTotal.CampaignIncrease = newPercentage + "% Increase In Last 30 Days";
            campaignTotal.activeCampaignCount = Convert.ToInt32(activeCampaignCount);
            campaignTotal.activeCampaignIncrease = activePercentage + "% Increase In Last 30 Days";

            return campaignTotal;
        }

        public IList<CamapignByMonthYear> GetCampaignByMonthOrYear()
        {
            DateTime checkTime = DateTime.Now.Date.AddMonths(-1);
            checkTime = checkTime.AddHours(48);
            IList<CamapignByMonthYear> campaignByMonthYear = new List<CamapignByMonthYear>();

            while (!(checkTime >= DateTime.Now))
            {
                DateTime nightTime = checkTime.AddHours(24);
                
                int dateCampaign = cmsContext.Campaigns.Where(a => a.Start_Date >= checkTime && a.Start_Date<=nightTime).Count();
                string date = checkTime.Day + "/" + checkTime.Month;

                campaignByMonthYear.Add(new CamapignByMonthYear {
                    CampaignCount = dateCampaign,
                    DateOrMonth = date
                });
                checkTime = nightTime;
            }

            return campaignByMonthYear;
        }

        public IList<CampaignStatusForDashboard> GetCampaignStatusForDashboard()
        {
            DateTime checkDate = DateTime.Now.Date.AddDays(-7);
            checkDate = checkDate.AddHours(24);

            IList<CampaignStatusForDashboard> campaignStatusForDashboard = new List<CampaignStatusForDashboard>();
            while (!(checkDate >= DateTime.Now))
            {
                DateTime nightTime = checkDate.AddHours(24);

                int activeCount = cmsContext.Campaigns.Where(a => a.Start_Date >= checkDate && a.Start_Date <= nightTime && a.CampaignStatusId == 2).Count();
                int completedCount = cmsContext.Campaigns.Where(a => a.Start_Date >= checkDate && a.Start_Date <= nightTime && a.CampaignStatusId == 5).Count();
                int startedCount = cmsContext.Campaigns.Where(a => a.Start_Date >= checkDate && a.Start_Date <= nightTime && a.CampaignStatusId == 1).Count();

                campaignStatusForDashboard.Add(new CampaignStatusForDashboard
                {
                    ActiveCount = activeCount,
                    CompletedCount = completedCount,
                    CreatedCount = startedCount,
                    date = checkDate.Day + "/" + checkDate.Month
                });
                checkDate = nightTime;
            }
            return campaignStatusForDashboard;
        }

        public bool ChangeCampaignStatus(int id, int statusId)
        {
            bool status = false;
           var camp =  cmsContext.Campaigns.Where(x => x.CampaignId == id).FirstOrDefault();
            camp.CampaignStatusId = statusId;
            int c = cmsContext.SaveChanges();
            if (c > 0)
            {
                status = true;
            }
            return status;
            throw new NotImplementedException();
        }

        public bool CheckSimilar(Campaign campaign)
        {
            bool status = false;
            if (campaign.CampaignId != 0)
            {
                var checkId = cmsContext.Campaigns.Where(x => x.CampaignId == campaign.CampaignId).FirstOrDefault();
                if (campaign.CampaignName == checkId.CampaignName)
                {
                    return status;
                }
                else
                {
                    var list = cmsContext.Campaigns.Where(x => x.CampaignName == campaign.CampaignName).FirstOrDefault();
                    if (list != null)
                    {
                        status = true;
                    }
                }

            }
            else
            {
                var duplicate = cmsContext.Campaigns.Where(x => x.CampaignName == campaign.CampaignName).FirstOrDefault();
                if (duplicate != null)
                {
                    status = true;
                }
            }
            return status;
        }
    }
}
