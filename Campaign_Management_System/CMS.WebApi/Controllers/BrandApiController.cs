using CMS.BE.ViewModels;
using CMS.BL.Interface;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CMS.WebApi.Controllers
{
    public class BrandApiController : ApiController
    {
        private IBrandManager _brandManager;
        public BrandApiController()
        {

        }
        public BrandApiController(IBrandManager brandManager)
        {
            _brandManager = brandManager;
        }
        [HttpGet]
        public IHttpActionResult GetAllBrands()
        {
            var list = _brandManager.GetAllBrandsForList();
            if (list == null)
            {
                return NotFound();
            }
            return Ok(list);
        }

        [HttpGet]
        public IHttpActionResult GetBrand(int id)
        {
            var Brand = _brandManager.getBrandById(id);
            if (Brand == null)
            {
                return NotFound();
            }
            return Ok(Brand);
        }

        [HttpPost]
        public IHttpActionResult InsertBrand([FromBody]BrandViewModel vm)
        {
            if (_brandManager.CheckSimilar(vm))
            {
                return BadRequest("Brand with same name already exist");
            }
            else
            {
                var insertTask = _brandManager.AddBrand(vm);
                if (insertTask)
                {
                    return Ok("Brand " + vm.BrandName + " added successfully");
                }
                else
                {
                    return InternalServerError();
                }
            }
          
        }

        [HttpPut]
        public IHttpActionResult UpdateBrand(BrandViewModel brandViewModel)
        {
            if (_brandManager.CheckSimilar(brandViewModel))
            {
                return BadRequest("Brand with same name already exist");
            }
            else
            {
                var editTask = _brandManager.EditBrand(brandViewModel);
                if (editTask)
                {
                    return Ok("Brand " + brandViewModel.BrandName + " updated  successfully");
                }
                else
                {
                    return InternalServerError();
                }
            }
              
        }

        [HttpDelete]
        public IHttpActionResult DeleteBrand(int id)
        {
            if (_brandManager.deleteBrand(id))
            {
                return Ok("Brand deleted successfully");
            }
            else
            {
                return NotFound();
            }
        }

    }
}
