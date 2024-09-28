namespace OpenAISemanticKernelPoc.Models
{
    public class CommodityImportRuleCertifiedUse
    {
        public int RuleId { get; set; }
        public int CertifiedUseId { get; set; }

        // Navigation properties
        public virtual CommodityImportRule CommodityImportRule { get; set; }
        public virtual CommodityImportCertifiedUse CommodityImportCertifiedUse { get; set; }

    }
}
