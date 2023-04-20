namespace Basic.Domain.Entity
{
    public class CommonResponse
    {
        public int Code { get; set; }
        public string? Message { get; set; }
    }
    public class CommonResponseOpt
    {
        public int Code { get; set; }
        public string? Message { get; set; }
        public string? Otp { get; set; }
        public string? ProcessId { get; set; }
        public string? Username { get; set; }
    }
}
