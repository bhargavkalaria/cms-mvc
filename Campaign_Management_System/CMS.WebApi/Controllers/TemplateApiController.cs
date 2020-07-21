using CMS.BE.ViewModels;
using CMS.BL.Interface;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CMS.WebApi.Controllers
{
    public class TemplateApiController : ApiController
    {
        private IEmailMasterManager _emailMasterManager;
        public TemplateApiController()
        {

        }
        public TemplateApiController(IEmailMasterManager emailMasterManager)
        {
            _emailMasterManager = emailMasterManager;
        }
        [HttpGet]
        public IHttpActionResult GetAllTemplates()
        {
            var list = _emailMasterManager.GetAllTemplate();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        [HttpGet]
        public IHttpActionResult GetTemplate(int id)
        {
            var template = _emailMasterManager.GetTemplateById(id);
            if (template == null)
            {
                return NotFound();
            }
            return Ok(template);
        }

        [HttpPost]
        public IHttpActionResult InsertTemplate([FromBody]TemplateViewModel vm)
        {
            if (_emailMasterManager.CheckSimilar(vm))
            {
                return BadRequest("Template with same name already exist");
            }
            else
            {
                var Added = _emailMasterManager.AddTemplate(vm);
                if (Added)
                {
                    return Ok("Template "+vm.TemplateName+" created successfully");
                }
                else
                {
                    return InternalServerError();
                }
            }
            
        }

        [HttpPut]
        public IHttpActionResult UpdateTemplate([FromBody]TemplateViewModel vm)
        {
            var Edited = _emailMasterManager.EditTemplate(vm);
            if (Edited)
            {
                return Ok("Template " + vm.TemplateName + " updated successfully");
            }
            else
            {
                return InternalServerError();
            }
        }

        [HttpDelete]
        public IHttpActionResult DeleteTemplate(int id)
        {
            if (_emailMasterManager.DeleteTemplate(id))
            {
                return Ok("Template deleted successfully");
            }
            else
            {
                return InternalServerError();
            }
        }

    }
}
