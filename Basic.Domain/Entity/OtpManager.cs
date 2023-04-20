using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Basic.Domain.Entity
{
    public class OtpManager
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? ProcessId { get; set; }
        public string? OtpCode { get; set; }

        public string? IsValid { get; set; }
        public string? IsVerified { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? VerifiedDate { get; set; }
    }
}
