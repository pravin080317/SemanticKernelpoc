using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OpenAISemanticKernelPoc.Models
{
    public class export
    {

        [Key]
        // Map Id property to StudentId column
        public int Id { get; set; }

        // Map Name property to FullName column
        public long PID { get; set; }

        // Map Age property to StudentAge column



        public long? VID { get; set; }
        public int Rate { get; set; }
        public int Preview { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }
        public bool perma { get; set; }

        public long trigger { get; set; }
        public long total { get; set; }

        public DateTime? Last { get; set; }
        public int UpBY { get; set; }
        public DateTime? Created { get; set; }
        public int CrBY { get; set; }
        public bool act { get; set; }

        public virtual product pro { get; set; }
        public virtual Variety vrt { get; set; }



    }


}
