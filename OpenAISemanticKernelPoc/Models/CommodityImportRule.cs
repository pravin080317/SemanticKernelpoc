namespace OpenAISemanticKernelPoc.Models
{
    public class CommodityImportRule
    {
        public int Id { get; set; }
        public int CommodityId { get; set; }
        public decimal Rate { get; set; }
        public bool Triggered { get; set; }
        public decimal Total { get; set; }
        public string RiskCategorisation { get; set; }
        public bool AllowMultipleInspections { get; set; }
        public int IsActive { get; set; }
        public string Regulator { get; set; }
        public int Permanent { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string VarietyId { get; set; }
        public string Eppo { get; set; }

        // Navigation properties
        public virtual ICollection<CommodityImportRuleCountry> CommodityImportRuleCountries { get; set; }
        public virtual ICollection<CommodityImportRuleException> CommodityImportRuleExceptions { get; set; }
        public virtual ICollection<CommodityImportRulePurpose> CommodityImportRulePurposes { get; set; }
        public virtual ICollection<CommodityImportRuleCertifiedUse> CommodityImportRuleCertifiedUses { get; set; }
        public virtual ICollection<CommodityImportRuleBorderControlPost> CommodityImportRuleBorderControlPosts { get; set; }

    }
}
