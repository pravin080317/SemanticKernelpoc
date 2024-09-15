using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace OpenAISemanticKernelPoc.Models
{
    public class Student
    {

        [Key]
        [Column("StudentId")] // Map Id property to StudentId column
        public int Id { get; set; }

        [Column("FullName")] // Map Name property to FullName column
        public string Name { get; set; }

        [Column("StudentAge")] // Map Age property to StudentAge column
        public int Age { get; set; }
    }


}
