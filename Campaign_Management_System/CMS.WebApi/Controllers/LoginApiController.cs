using CMS.BE.ViewModels;
using CMS.BL.Interface;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CMS.WebApi.Controllers
{
    [EnableCors(origins: "https://localhost:44308", headers: "*", methods: "*")]
    public class LoginApiController : ApiController
    {
        private ILoginManager _iLoginManager;
        public LoginApiController()
        {

        }
        public LoginApiController(ILoginManager loginManager)
        {
            _iLoginManager = loginManager;
        }

        [HttpGet]
        public IHttpActionResult CheckEmail(string email)
        {
            if (_iLoginManager.CheckEmailExist(email))
            {
                return InternalServerError();
            }
            else
            {
                return Ok();
            }
        }

        [HttpPost]
        public IHttpActionResult Login([FromBody]UserViewModel vm)
        {
            var userDetail = _iLoginManager.GetUserByEmailPassword(vm.Email, vm.Password);
            if (userDetail == null)
            {
                return NotFound();
            }
            return Ok(userDetail);
        }

        [HttpPost]
        public IHttpActionResult Register([FromBody]UserViewModel vm)
        {
            if (_iLoginManager.ServerSideEmptyValidation(vm))
            {
                return InternalServerError();
            }
            if (!_iLoginManager.CheckEmail(vm.Email))
            {
                return InternalServerError();
            }
            if (!_iLoginManager.CheckPassword(vm.Password))
            {
                return InternalServerError();
            }
            if (!_iLoginManager.CheckName(vm.FName) || !_iLoginManager.CheckName(vm.LName))
            {
                return InternalServerError();
            }

            var status = _iLoginManager.AddUser(vm);
            if (status)
            {
                return Ok(vm);
            }
            else
            {
                return InternalServerError();
            }
        }

        [HttpGet]
        public IHttpActionResult ResetPassword(string email, string cur_pwd, string new_pwd)
        {
            var User = _iLoginManager.GetUserByEmail(email);
            string enc_pwd = _iLoginManager.Hash(cur_pwd);
            if (enc_pwd.Equals(User.Password))
            {
                User.Password = new_pwd;
                if (_iLoginManager.ResetPassword(User) == 1)
                {
                    return Ok();
                }
                else
                {
                    return InternalServerError();
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        public IHttpActionResult ForgotPassword(string Email)
        {
            var user = _iLoginManager.GetUserByEmail(Email);
            if (user != null)
            {
                string pwd = _iLoginManager.UpdateUserPassword(user);
                return Ok(pwd);
            }
            else
            {
                return null;
            }
                
        }
    }
}
