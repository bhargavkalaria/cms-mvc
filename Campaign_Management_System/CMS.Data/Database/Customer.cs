using System.ComponentModel.DataAnnotations;

namespace CMS.Data.Database
{
    public class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string Mobile { get; set; }
        public int Age { get; set; }

        public string State { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
    }
}
