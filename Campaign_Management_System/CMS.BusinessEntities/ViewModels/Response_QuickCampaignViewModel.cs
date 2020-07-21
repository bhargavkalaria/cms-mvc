using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.BE.ViewModels
{
    public class Response_QuickCampaignViewModel
    {
        public int ResponseId { get; set; }
        public int QuickCampaignId { get; set; }
        public int Positive { get; set; }
        public int Negative { get; set; }
        public int Neutral { get; set; }
        public int NoResponse { get; set; }
    }
}
