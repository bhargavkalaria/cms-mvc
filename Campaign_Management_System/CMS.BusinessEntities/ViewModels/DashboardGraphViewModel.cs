using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BE.ViewModels
{
    public class DashboardGraphViewModel
    {
        public double SuccessRate { get; set; }
        public string SuccessIncrease { get; set; }
        public int CampaignCount { get; set; }
        public string CampaignIncrease { get; set; }
        public int ActiveCampaignCount { get; set; }
        public string ActiveCampaignIncrease { get; set; }
    }
}
