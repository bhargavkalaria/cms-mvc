using CMS.BE.ViewModels;
using CMS.BL.Interface;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CMS.WebApi.Controllers
{
    public class RolesApiController : ApiController
    {
        private IRoleManager _iroleManager;
        public RolesApiController()
        {

        }
        public RolesApiController(IRoleManager roleManager)
        {
            _iroleManager = roleManager;
        }
        [HttpGet]
        public IHttpActionResult GetUsersList()
        {
            try
            {
                var users = _iroleManager.getAllUsers();

                if (users != null)
                {
                    return Ok(users);
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (System.Exception)
            {

                throw;
            }
           
        }

         [HttpGet]
        public IHttpActionResult GetUser(int id)
        {
            var User = _iroleManager.getUserById(id);
            if (User == null)
            {
                return NotFound();
            }
            return Ok(User);
        }

        [HttpPut]
        public IHttpActionResult UpdateRole(UserViewModel data)
        {
            var roleUpdated = _iroleManager.updateRole(data);
            if (roleUpdated)
            {
                return Ok();
            }
            else
            {
                return InternalServerError();
            }
        }

        [HttpGet]
        public IHttpActionResult GetAddBrandAccess(int id)
        {
            var access = _iroleManager.hasAddBrandAccess(id);
            if (access)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public IHttpActionResult GetViewBrandAccess(int id)
        {
            var access = _iroleManager.hasViewBrandAccess(id);
            if (access)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public IHttpActionResult GetPrintAccess(int id)
        {
            var access = _iroleManager.hasPrintAccess(id);
            if (access)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public IHttpActionResult GetEditBrandAccess(int id)
        {
            var access = _iroleManager.hasEditBrandAccess(id);
            if (access)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpGet]
        public IHttpActionResult GetDeleteBrandAccess(int id)
        {
            var access = _iroleManager.hasDeleteBrandAccess(id);
            if (access)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IHttpActionResult GetUploadCustomerAccess(int id)
        {
            var access = _iroleManager.hasUploadCustomerAccess(id);
            if (access)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
        [HttpDelete]
        public IHttpActionResult DeleteUser(int id)
        {
            if (_iroleManager.DeleteUser(id))
            {
                return Ok("Brand deleted successfully");
            }
            else
            {
                return NotFound();
            }
        }
        [HttpGet]
        public IHttpActionResult GetTemplateAccess(int id)
        {
            var access = _iroleManager.hasViewTemplateAccess(id);
            if (access)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IHttpActionResult GetViewQuickCampaignAccess(int id)
        {
            var access = _iroleManager.hasViewQuickCampaignAccess(id);
            if (access)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IHttpActionResult GetAddQuickCampaignAccess(int id)
        {
            var access = _iroleManager.hasAddQuickCampaignAccess(id);
            if (access)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IHttpActionResult GetReportAccess(int id)
        {
            var access = _iroleManager.hasReportAccess(id);
            if (access)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IHttpActionResult GetAddCampaignAccess(int id)
        {
            var access = _iroleManager.hasAddCampaignAccess(id);
            if (access)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IHttpActionResult GetViewCampaignAccess(int id)
        {
            var access = _iroleManager.hasViewCampaignAccess(id);
            if (access)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IHttpActionResult GetEditCampaignAccess(int id)
        {
            var access = _iroleManager.hasEditCampaignAccess(id);
            if (access)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IHttpActionResult GetDeleteCampaignAccess(int id)
        {
            var access = _iroleManager.hasDeleteCampaignAccess(id);
            if (access)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IHttpActionResult GetAddUserAccess(int id)
        {
            var access = _iroleManager.hasAddUserAccess(id);
            if (access)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IHttpActionResult GetAddTemplateAccess(int id)
        {
            var access = _iroleManager.hasAddTemplateAccess(id);
            if (access)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IHttpActionResult GetViewTemplateAccess(int id)
        {
            var access = _iroleManager.hasViewTemplateAccess(id);
            if (access)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IHttpActionResult GetEditTemplateAccess(int id)
        {
            var access = _iroleManager.hasEditTemplateAccess(id);
            if (access)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public IHttpActionResult GetDeleteTemplateAccess(int id)
        {
            var access = _iroleManager.hasDeleteTemplateAccess(id);
            if (access)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

    }
}
