namespace OpenAISemanticKernelPoc.Models
{
    public class CommodityImportRuleBorderControlPost
    {
        public int RuleId { get; set; }
        public string BcpCode { get; set; }

        // Navigation properties
        public virtual CommodityImportRule CommodityImportRule { get; set; }
    }
}
