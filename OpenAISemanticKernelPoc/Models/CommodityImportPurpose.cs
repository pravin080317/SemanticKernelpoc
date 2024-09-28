namespace OpenAISemanticKernelPoc.Models
{
    public class CommodityImportPurpose
    {
        public int Id { get; set; }
        public string Purpose { get; set; }

        // Navigation properties
        public virtual ICollection<CommodityImportRulePurpose> CommodityImportRulePurposes { get; set; }

    }
}
