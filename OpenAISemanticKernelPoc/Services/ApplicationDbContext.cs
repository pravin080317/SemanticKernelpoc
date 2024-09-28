using Microsoft.EntityFrameworkCore;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.TermStore;
using OpenAISemanticKernelPoc.Models;
using OpenAISemanticKernelPoc.Services;

namespace OpenAISemanticKernelPoc.Services
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSet represents a table in the database
        //dbset
        public DbSet<Student> Students { get; set; }

        public DbSet<export> exports { get; set; }

        public DbSet<product> products { get; set; }

        public DbSet<Variety> Varietys { get; set; }

        public DbSet<CommodityImportRule> CommodityImportRules { get; set; }
        public DbSet<CommodityImportRuleCountry> CommodityImportRuleCountries { get; set; }
        public DbSet<CommodityImportRuleException> CommodityImportRuleExceptions { get; set; }
        public DbSet<CommodityImportRuleBorderControlPost> CommodityImportRuleBorderControlPosts { get; set; }
        public DbSet<CommodityImportRulePurpose> CommodityImportRulePurposes { get; set; }
        public DbSet<CommodityImportPurpose> CommodityImportPurposes { get; set; }
        public DbSet<CommodityImportRuleCertifiedUse> CommodityImportRuleCertifiedUses { get; set; }
        public DbSet<CommodityImportCertifiedUse> CommodityImportCertifiedUses { get; set; }
        public DbSet<CountryGroupsMapping> CountryGroupsMappings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define relationships and keys if needed
            modelBuilder.Entity<CommodityImportRuleCountry>()
                .HasKey(c => new { c.RuleId, c.CountryOrGroupId });

            modelBuilder.Entity<CommodityImportRuleException>()
                .HasKey(e => new { e.RuleId, e.CountryOrGroupId });

            modelBuilder.Entity<CommodityImportRulePurpose>()
                .HasKey(p => new { p.RuleId, p.PurposeId });

            modelBuilder.Entity<CommodityImportRuleCertifiedUse>()
                .HasKey(cu => new { cu.RuleId, cu.CertifiedUseId });

            modelBuilder.Entity<CountryGroupsMapping>()
                .HasKey(cg => new { cg.CountryGroupsId, cg.CountryId });

            // Further configuration if necessary
        }


    }
}

//The DbContext class is a central part of EF Core. It represents a session with the database, allowing you to query and save data.
//ApplicationDbContext inherits from DbContext. The constructor accepts DbContextOptions and passes them to the base class.
//The DbSet<Student> property represents the table in the database that will store Student entities.
