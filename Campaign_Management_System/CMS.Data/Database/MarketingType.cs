using System.ComponentModel.DataAnnotations;

namespace CMS.Data.Database
{
    public class MarketingType
    {
        [Key]
        public int MarketingTypeId { get; set; }
        public string MarketingTypeName { get; set; }
    }
}
