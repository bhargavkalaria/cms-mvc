using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BE.ViewModels
{
    public class DashboardBrandViewModel
    {
        public string BrandName { get; set; }
        public int BrandCampaignCount { get; set; }
        public decimal BrandBudget { get; set; }
    }
}
