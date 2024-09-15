using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OpenAISemanticKernelPoc.Models
{
    public class product
    {

       
        public int Id { get; set; }


        public string Prod { get; set; }
        public string ProdG { get; set; }


        public IEnumerable<Variety> vas { get; set; }
    }


}
