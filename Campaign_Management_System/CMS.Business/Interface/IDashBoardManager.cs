using CMS.BE.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BL.Interface
{
    public interface IDashBoardManager
    {
        DashboardGraphViewModel GetBasicDashBoardDetails();
        IList<CampaignMonthYearViewModel> GetCampaignByMonthOrYear();
        IList<DashboardBrandViewModel> getTopFiveBrandByBudget();
        IList<CampaignCountByStatusViewModel> GetCampaignStatusList();
        IList<ResponseCampaignViewModel> GetTopThreeResponse();
        IList<MarketingStrategyViewModel> getMarketingStatergies();
    }
}
