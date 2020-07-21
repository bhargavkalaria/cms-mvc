using CMS.Data.Database;
using CMS.DL.Interface;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CMS.DL.Implementation
{
    public class EmailMasterRepository : IEmailMasterRepository
    {
        private CMSContext CMSContext;
        
        public EmailMasterRepository()
        {
            CMSContext = new CMSContext();
        }
        public bool addTemplate(Template template)
        {
            bool status = false;
            int c = 0;
            CMSContext.Templates.Add(template);
            c = CMSContext.SaveChanges();
            if (c > 0)
            {
                status = true;
            }
            return status;
        }
        public bool CheckSimilar(Template template)
        {
            bool status = false;
            var duplicate = CMSContext.Templates.Where(x => x.TemplateName == template.TemplateName).FirstOrDefault();
            if (duplicate != null)
            {
                status = true;
            }
            return status;
        }
        public bool DeleteTemplate(int id)
        {
            bool status = false;
            Template tm = new Template();
            tm = CMSContext.Templates.Where(x => x.TemplateId == id).FirstOrDefault();
            CMSContext.Templates.Remove(tm);
            int c = 0;
            c = CMSContext.SaveChanges();
            if (c > 0)
            {
                status = true;
            }
            return status;
        }

        public bool EditTemplate(Template template)
        {
            bool status = false;
            Template tp = new Template();
            tp = template;
            var local = CMSContext.Set<Template>()
                      .Local
                      .FirstOrDefault(f => f.TemplateId == template.TemplateId);
            if (local != null)
            {
                CMSContext.Entry(local).State = EntityState.Detached;
            }
            CMSContext.Entry(template).State = EntityState.Modified;
            int c = 0;
            c = CMSContext.SaveChanges();
            if (c > 0)
            {
                status = true;
            }
            return status;
        }

        public List<Template> GetAllTemplates()
        {
            return CMSContext.Templates.ToList();
        }

        public Template GetTemplateById(int id)
        {
            return CMSContext.Templates.Where(x => x.TemplateId == id).FirstOrDefault();
        }
    }
}
