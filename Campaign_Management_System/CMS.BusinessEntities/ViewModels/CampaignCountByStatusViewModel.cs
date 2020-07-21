using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BE.ViewModels
{
    public class CampaignCountByStatusViewModel
    {
        public int ActiveCount { get; set; }
        public int CompletedCount { get; set; }
        public int CreatedCount { get; set; }
        public string date { get; set; }
    }
}
