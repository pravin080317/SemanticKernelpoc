namespace OpenAISemanticKernelPoc.Models
{
    public class CommodityImportRuleException
    {
        public int RuleId { get; set; }
        public int CountryOrGroupId { get; set; }
        public int Type { get; set; }

        // Navigation properties
        public virtual CommodityImportRule CommodityImportRule { get; set; }
    }
}
