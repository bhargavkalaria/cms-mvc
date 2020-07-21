using CMS.Data.Database;
using System.Collections.Generic;

namespace CMS.DL.Interface
{
    public interface IEmailMasterRepository
    {
        bool addTemplate(Template template);

        bool EditTemplate(Template template);


        bool DeleteTemplate(int id);

        Template GetTemplateById(int id);
        List<Template> GetAllTemplates();

        bool CheckSimilar(Template template);
    }
}
