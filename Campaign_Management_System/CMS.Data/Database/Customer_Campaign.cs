using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace CMS.Data.Database
{
    public class Customer_Campaign
    {
        [Key]
        public int Id { get; set; }

        public int CustomerID { get; set; }
        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }
        public int CampaignId { get; set; }
        public string CustomTemplate { get; set; }
        public bool isEmailSent { get; set; }
        public string Response { get; set; }
        [ForeignKey("CampaignId")]
        public virtual Campaign Campaign { get; set; }
        public virtual ICollection<Campaign> campaigns { get; set; }
    }
}
