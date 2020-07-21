using System.ComponentModel.DataAnnotations;

namespace CMS.Data.Database
{
    public class Brand
    {
        [Key]
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public bool isDeleted { get; set; }
    }
}
