using CMS.Data.Database;
using CMS.DL.Interface;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CMS.DL.Implementation
{
    public class BrandRepository : IBrandRepository
    {
        private CMSContext cmsContext;
        public BrandRepository()
        {
            cmsContext = new CMSContext();
        }

        public bool AddBrand(Brand brand)
        {
            bool status = false;
            cmsContext.brands.Add(brand);
            int c;
            c = cmsContext.SaveChanges();
            if (c > 0)
            {
                status = true;
            }
            return status;
        }

        public bool CheckSimilar(Brand brand)
        {
            bool status = false;
            if (brand.BrandId != 0)
            {
                var checkId = cmsContext.brands.Where(x => x.BrandId == brand.BrandId).FirstOrDefault();
                if (brand.BrandName == checkId.BrandName)
                {
                    return status;
                }
                else
                {
                    var list = cmsContext.brands.Where(x => x.BrandName == brand.BrandName).FirstOrDefault();
                    if (list != null)
                    {
                        status = true;
                    }
                }
            }
            else
            {
                var duplicate = cmsContext.brands.Where(x => x.BrandName == brand.BrandName).FirstOrDefault();
                if (duplicate != null)
                {
                    status = true;
                }
            }
           
            return status;
        }

        public bool DeleteBrand(int id)
        {
            bool status = false;
            var brand = cmsContext.brands.Where(x => x.BrandId == id).FirstOrDefault();
            brand.isDeleted = true;
            var local = cmsContext.Set<Brand>()
                        .Local
                        .FirstOrDefault(f => f.BrandId == id);
            if (local != null)
            {
                cmsContext.Entry(local).State = EntityState.Detached;
            }
            cmsContext.Entry(brand).State = EntityState.Modified;
            int c = cmsContext.SaveChanges();
            if (c > 0)
            {
                status = true;
            }
            return status;
        }

        public bool EditBrand(Brand brand)
        {
            bool status = false;
            Brand ca = new Brand();
            ca = brand;
            var local = cmsContext.Set<Brand>()
                        .Local
                        .FirstOrDefault(f => f.BrandId == brand.BrandId);
            if (local != null)
            {
                cmsContext.Entry(local).State = EntityState.Detached;
            }
            cmsContext.Entry(brand).State = EntityState.Modified;
            int c;
            c = cmsContext.SaveChanges();
            if (c > 0)
            {
                status = true;
            }
            return status;
        }

        public List<Brand> GetAllBrands()
        {
            return cmsContext.brands.ToList();
        }

        public Brand GetBrandById(int id)
        {
            return cmsContext.brands.Where(x => x.BrandId == id).FirstOrDefault();
        }

        public IList<BrandBudgetData> topFiveBrandBudget()
        {
            IList<Brand> brands = GetAllBrands();
            List<BrandBudgetData> brandBudgets = new List<BrandBudgetData>();
            foreach (var item in brands)
            {
                IList<Campaign> campaignBudget= cmsContext.Campaigns.Where(a => a.BrandId == item.BrandId).ToList();
                decimal brandBudegetSum = 0;
                foreach (var campaign in campaignBudget)
                {
                    brandBudegetSum += campaign.CampaignBudget;
                }
                int brandCount = cmsContext.Campaigns.Where(a => a.BrandId == item.BrandId).Count();

                brandBudgets.Add(new BrandBudgetData
                {
                    BrandName = item.BrandName,
                    Budget = brandBudegetSum,
                    countBrand = brandCount
                });
            }
            brandBudgets = brandBudgets.OrderByDescending(a=>a.Budget).ToList();
            return brandBudgets;
        }
    }
}
