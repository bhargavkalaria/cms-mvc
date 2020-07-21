using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.Data.Database
{
    public class Template
    {
        [Key]
        public int TemplateId { get; set; }
        public string TemplateName { get; set; }
        public string TemplateData { get; set; }

        public DateTime LastUpdated { get; set; }


        public DateTime CreatedDate { get; set; }
    }
}
