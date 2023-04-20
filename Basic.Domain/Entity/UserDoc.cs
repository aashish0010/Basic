using Microsoft.AspNetCore.Http;

namespace Basic.Domain.Entity
{
    public class UserDoc
    {
        public int Id { get; set; }
        public string? UserName { get; set; }
        public string? ImageName { get; set; }
        public string? ImageUrl { get; set; }
        public string? ImageDetails { get; set; }
        public string? IsProfilePic { get; set; }
        public string? ImageCategory { get; set; }
        public string? IsDelete { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; } = DateTime.UtcNow;
        public string? IsActive { get; set; }
    }
    public class UserDocReq
    {
        public IFormFile? Image { get; set; }
        //public string? IsProfilePic { get; set; }
        //public string? ImageCategory { get; set; }
        //public string? ImageDes { get; set; }
    }
}
