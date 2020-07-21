using CMS.BE.ViewModels;
using System.Collections.Generic;

namespace CMS.BL.Interface
{
    public interface IEmailMasterManager
    {
        bool AddTemplate(TemplateViewModel templateViewModel);
        List<TemplateViewModel> GetAllTemplate();

        bool EditTemplate(TemplateViewModel templateViewModel);

        bool DeleteTemplate(int id);

        TemplateViewModel GetTemplateById(int id);
        bool CheckSimilar(TemplateViewModel templateViewModel);
    }
}
