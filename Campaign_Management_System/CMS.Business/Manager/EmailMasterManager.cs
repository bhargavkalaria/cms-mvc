using AutoMapper;
using CMS.BE.ViewModels;
using CMS.BL.Interface;
using CMS.Data.Database;
using CMS.DL.Interface;
using System.Collections.Generic;

namespace CMS.BL.Manager
{
    public class EmailMasterManager : IEmailMasterManager
    {
        private IEmailMasterRepository _iemailMasterRepository;
        public EmailMasterManager(IEmailMasterRepository emailMasterRepository)
        {
            _iemailMasterRepository = emailMasterRepository;
        }
        public bool AddTemplate(TemplateViewModel templateViewModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TemplateViewModel, Template>();
            });

            IMapper mapper = config.CreateMapper();
            var source = templateViewModel;
            var dest = mapper.Map<TemplateViewModel, Template>(source);
            return _iemailMasterRepository.addTemplate(dest);
        }
        public bool CheckSimilar(TemplateViewModel templateViewModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TemplateViewModel, Template>();
            });

            IMapper mapper = config.CreateMapper();
            var source = templateViewModel;
            var dest = mapper.Map<TemplateViewModel, Template>(source);
            return _iemailMasterRepository.CheckSimilar(dest);
        }
        public bool DeleteTemplate(int id)
        {
            return _iemailMasterRepository.DeleteTemplate(id);
        }

        public bool EditTemplate(TemplateViewModel templateViewModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TemplateViewModel, Template>();
            });

            IMapper mapper = config.CreateMapper();
            var source = templateViewModel;
            var dest = mapper.Map<TemplateViewModel, Template>(source);
            return _iemailMasterRepository.EditTemplate(dest);
        }

        public List<TemplateViewModel> GetAllTemplate()
        {
            List<TemplateViewModel> templateViewModel = new List<TemplateViewModel>();
            var template = _iemailMasterRepository.GetAllTemplates();
            foreach (var user in template)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Template, TemplateViewModel>();
                });

                IMapper mapper = config.CreateMapper();
                var source = new Template();
                source = user;
                var dest = mapper.Map<Template, TemplateViewModel>(source);
                templateViewModel.Add(dest);
            }
            return templateViewModel;
        }

        public TemplateViewModel GetTemplateById(int id)
        {
            Template tm = _iemailMasterRepository.GetTemplateById(id);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Template, TemplateViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            var source = tm;
            var dest = mapper.Map<Template, TemplateViewModel>(source);

            return dest;
        }
    }
}
