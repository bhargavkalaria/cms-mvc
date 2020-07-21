using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace CMS.Data.Database
{
    public class CMSContext : DbContext
    {
        public CMSContext() : base("CMSDBContext")
        {
            //this.Configuration.LazyLoadingEnabled = true;
            //this.Configuration.ProxyCreationEnabled = false;
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<MarketingStrategy> MarketingStrategies { get; set; }
        public DbSet<MarketingType> MarketingTypes { get; set; }
        public DbSet<CampaignStatus> CampaignStatuses { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<QuickCampaign> QuickCampaigns { get; set; }
        public DbSet<Response> Responses { get; set; }

        public DbSet<Template> Templates { get; set; }
        public DbSet<Brand> brands { get; set; }
        public DbSet<Log> Logs { get; set; }
        public DbSet<Customer_Campaign> Customer_Campaigns { get; set; }
        public DbSet<Customer_QuickCampaign> Customer_QuickCampaigns { get; set; }
        public DbSet<Response_QuickCampaign> Response_QuickCampaigns { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Primary Key User
            modelBuilder.Entity<User>().HasKey(c => c.UId);
            modelBuilder.Entity<User>().Property(c => c.UId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //Primary Key Campaign
            modelBuilder.Entity<Campaign>().HasKey(c => c.CampaignId);
            modelBuilder.Entity<Campaign>().Property(c => c.CampaignId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //Primary Key CampaignStatus
            modelBuilder.Entity<CampaignStatus>().HasKey(s => s.CampaignStatusId);
            modelBuilder.Entity<CampaignStatus>().Property(s => s.CampaignStatusId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //Primary Key MarketingType
            modelBuilder.Entity<MarketingType>().HasKey(t => t.MarketingTypeId);
            modelBuilder.Entity<MarketingType>().Property(t => t.MarketingTypeId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //Primary Key MarketingStrategy
            modelBuilder.Entity<Customer_Campaign>().HasKey(s => s.Id);
            modelBuilder.Entity<Customer_Campaign>().Property(s => s.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //Primary Key MarketingStrategy
            modelBuilder.Entity<MarketingStrategy>().HasKey(s => s.MarketingStrategyId);
            modelBuilder.Entity<MarketingStrategy>().Property(s => s.MarketingStrategyId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //Primary Key Template
            modelBuilder.Entity<Template>().HasKey(s => s.TemplateId);
            modelBuilder.Entity<Template>().Property(s => s.TemplateId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //Primary Key Brand
            modelBuilder.Entity<Brand>().HasKey(s => s.BrandId);
            modelBuilder.Entity<Brand>().Property(s => s.BrandId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            //Primary Key Quick Campaign
            modelBuilder.Entity<QuickCampaign>().HasKey(s => s.QuickCampaignId);
            modelBuilder.Entity<QuickCampaign>().Property(s => s.QuickCampaignId)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            modelBuilder.Entity<Response>().HasRequired(s => s.Campaign).WithMany().WillCascadeOnDelete(true);

            modelBuilder.Entity<Customer_Campaign>().HasRequired(s => s.Campaign).WithMany().WillCascadeOnDelete(true);

            base.OnModelCreating(modelBuilder);
        }
    }
}
