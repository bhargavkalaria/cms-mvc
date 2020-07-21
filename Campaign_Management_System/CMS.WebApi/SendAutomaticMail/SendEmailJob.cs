using CMS.BE.ViewModels;
using CMS.Common;   
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using CMS.BL.Interface;

namespace CMS.BL.Manager.SendAutomaticMail
{
    public class SendEmailJob : IJob
    {
        private ICampaignManager _icampaignManager;
        public SendEmailJob() 
        {
            _icampaignManager = new CampaignManager();
        }
        public SendEmailJob(ICampaignManager campaignManager)
        {
            _icampaignManager = campaignManager;
        }
        public void Execute(IJobExecutionContext context)
        {
            DateTime currentTime = System.DateTime.Now;
            IEnumerable<CampaignViewModel> allCampaign = _icampaignManager.GetAllCampaigns();
            foreach (var campaign in allCampaign)
            {
                bool checkEnd = currentTime >= campaign.End_Date;
                bool checkStart = currentTime >= campaign.Start_Date;
                
                if (checkStart && (campaign.CampaignStatusId == 1 || campaign.CampaignStatusId == 2 || campaign.CampaignStatusId == 4))
                {
                    if(campaign.CampaignStatusId == 1)
                    _icampaignManager.ChangeCampaignStatus(campaign.CampaignId, 2);
                    _icampaignManager.SendCampaignMailAsync(campaign.CampaignId, campaign.TemplateId);
                }
                if (checkEnd && campaign.CampaignStatusId != 5)
                {
                    _icampaignManager.ChangeCampaignStatus(campaign.CampaignId, 5);
                }
            }
        }
    }
}
