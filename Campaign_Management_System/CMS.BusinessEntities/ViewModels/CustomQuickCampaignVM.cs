using System.Collections.Generic;

namespace CMS.BE.ViewModels
{
    public class CustomQuickCampaignVM
    {
        public QuickCampaignViewModel QuickCampaignViewModel { get; set; }

        public List<int> CustomerID { get; set; }

        public string Temp { get; set; }
    }
}
