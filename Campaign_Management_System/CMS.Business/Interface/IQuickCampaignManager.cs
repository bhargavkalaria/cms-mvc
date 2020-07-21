using CMS.BE.ViewModels;
using System.Collections.Generic;

namespace CMS.BL.Interface
{
    public interface IQuickCampaignManager
    {
        bool AddQuickCampaign(QuickCampaignViewModel quickModel);
        int GetLatestQuickCampaignId();

        bool AddToList(int quickcampaignId, List<int> customerIds);

        List<int> GetCustomerIdsListByQuickCampaignId(int id);
        List<QuickCampaignViewModel> GetAllQuickCampaigns();

        bool sendMail(int quickcampaignId, List<int> customerIds, int templateId, string temp);
        bool CheckSimilar(QuickCampaignViewModel quickCampaignViewModel);

        string EmailPreview(QuickCampaignViewModel quickcampaignViewModel, string templateData, List<int> customerId);

    }
}
