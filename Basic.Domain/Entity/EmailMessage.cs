//using MimeKit;

namespace Basic.Domain.Entity
{
    public class EmailMessage
    {
        public string? EmailToId { get; set; }
        public string? EmailToName { get; set; }
        public string? EmailSubject { get; set; }
        public string? EmailBody { get; set; }
    }
}
