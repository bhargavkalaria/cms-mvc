using System.Collections.Generic;

namespace CMS.BE.ViewModels
{
    public class CustomCampaignVM
    {
        public CampaignViewModel CampaignViewModel { get; set; }

        public List<int> CustomerID { get; set; }

        public string Temp { get; set; }
    }
}
