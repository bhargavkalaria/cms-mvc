using System;
using System.ComponentModel.DataAnnotations;

namespace CMS.BE.ViewModels
{
    public class TemplateViewModel
    {
        public int TemplateId { get; set; }

        [Display(Name = "Name of the Template")]
        [Required(ErrorMessage = "Template Name is required")]
        public string TemplateName { get; set; }

        [Display(Name = "Preview")]
        [Required(ErrorMessage = "Template Data is required")]
        public string TemplateData { get; set; }

        [Display(Name = "Last Updated")]
        [Required(ErrorMessage = "Last Updated is required")]
        public DateTime LastUpdated { get; set; }

        [Display(Name = "Created Date")]
        [Required(ErrorMessage = "Created Date is required")]

        public DateTime CreatedDate { get; set; }
    }
}
