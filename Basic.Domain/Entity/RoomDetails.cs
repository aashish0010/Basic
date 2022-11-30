namespace Basic.Domain.Entity
{
    public class RoomType
    {
        public int Id { get; set; }
        public string? Type { get; set; }
        public string? Description { get; set; }
        public int? Createby { get; set; }

        public DateTime? Createdate { get; set; } = DateTime.UtcNow;
        public DateTime? Updatedate { get; set; }
    }
}
