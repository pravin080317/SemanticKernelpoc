namespace OpenAISemanticKernelPoc.Models
{
    public class CommodityImportRulePurpose
    {
        public int RuleId { get; set; }
        public int PurposeId { get; set; }

        // Navigation properties
        public virtual CommodityImportRule CommodityImportRule { get; set; }
        public virtual CommodityImportPurpose CommodityImportPurpose { get; set; }
    }
}
