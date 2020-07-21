using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Data.Database
{
    public class CampaignStatusForDashboard
    {
        public int ActiveCount { get; set; }
        public int CompletedCount { get; set; }
        public int CreatedCount { get; set; }
        public string date { get; set; }
    }
}
