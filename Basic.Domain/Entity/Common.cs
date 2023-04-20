using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Basic.Domain.Entity
{
    public class Common
    {
        public int Sno { get; set; }
        public string? StaticKey { get; set; }
        public string? StaticValue { get; set; }
        public string? Description { get; set; }
    }
    public class CommonType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Description { get; set; }
    }
}
