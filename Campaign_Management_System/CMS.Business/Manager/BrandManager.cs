using AutoMapper;
using CMS.BE.ViewModels;
using CMS.BL.Interface;
using CMS.Data.Database;
using CMS.DL.Interface;
using System.Collections.Generic;

namespace CMS.BL.Manager
{
    public class BrandManager : IBrandManager
    {
        private IBrandRepository _ibrandRepository;
        public BrandManager(IBrandRepository brandRepository)
        {
            _ibrandRepository = brandRepository;
        }

        public bool AddBrand(BrandViewModel brandViewModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BrandViewModel, Brand>();
            });

            IMapper mapper = config.CreateMapper();
            var source = brandViewModel;
            var dest = mapper.Map<BrandViewModel, Brand>(source);
            return _ibrandRepository.AddBrand(dest);
        }

        public bool CheckSimilar(BrandViewModel brandViewModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BrandViewModel, Brand>();
            });

            IMapper mapper = config.CreateMapper();
            var source = brandViewModel;
            var dest = mapper.Map<BrandViewModel, Brand>(source);
            return _ibrandRepository.CheckSimilar(dest);
        }

        public bool deleteBrand(int id)
        {
            return _ibrandRepository.DeleteBrand(id);
        }

        public bool EditBrand(BrandViewModel brandViewModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<BrandViewModel, Brand>();
            });

            IMapper mapper = config.CreateMapper();
            var source = brandViewModel;
            var dest = mapper.Map<BrandViewModel, Brand>(source);
            return _ibrandRepository.EditBrand(dest);
        }

        public List<BrandViewModel> GetAllBrandsForList()
        {
            List<BrandViewModel> brandViewModels = new List<BrandViewModel>();
            var brands = _ibrandRepository.GetAllBrands();
            foreach (var brand in brands)
            {
                if (!brand.isDeleted)
                {
                    var config = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<Brand, BrandViewModel>();
                    });

                    IMapper mapper = config.CreateMapper();
                    var source = new Brand();
                    source = brand;
                    var dest = mapper.Map<Brand, BrandViewModel>(source);
                    brandViewModels.Add(dest);
                }
            }
            return brandViewModels;
        }

        public List<BrandViewModel> GetAllBrands()
        {
            List<BrandViewModel> brandViewModels = new List<BrandViewModel>();
            var brands = _ibrandRepository.GetAllBrands();
            foreach (var brand in brands)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Brand, BrandViewModel>();
                });

                IMapper mapper = config.CreateMapper();
                var source = new Brand();
                source = brand;
                var dest = mapper.Map<Brand, BrandViewModel>(source);
                brandViewModels.Add(dest);
            }
            return brandViewModels;
        }

        public BrandViewModel getBrandById(int id)
        {
            Brand brand = _ibrandRepository.GetBrandById(id);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Brand, BrandViewModel>();
            });

            IMapper mapper = config.CreateMapper();
            var source = brand;
            var dest = mapper.Map<Brand, BrandViewModel>(source);

            return dest;

        }
    }
}
