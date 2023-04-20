using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Basic.Domain.Entity
{
    public class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? Longitude { get; set; }
        public string? Latitude { get; set; }
        public string? CreateDate { get; set; }
        public string? IsActive { get; set; }
    }
    public class LocationList
    {
        public string? Username { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }
    }
    public class DistanceDetail : CommonResponse
    {
        public string Sender { get; set; }
        public string Receiver { get; set; }
        public string SenderApprove { get; set; }
        public string ReceiverApprove { get; set; }
        public double DistanceInKm { get; set; }
    }
    public class MatchFind
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

    }


}
