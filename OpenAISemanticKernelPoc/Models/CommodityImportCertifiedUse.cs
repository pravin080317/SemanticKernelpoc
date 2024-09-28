namespace OpenAISemanticKernelPoc.Models
{
    public class CommodityImportCertifiedUse
    {
        public int Id { get; set; }
        public string CertifiedFor { get; set; }

        // Navigation properties
        public virtual ICollection<CommodityImportRuleCertifiedUse> CommodityImportRuleCertifiedUses { get; set; }

    }
}
