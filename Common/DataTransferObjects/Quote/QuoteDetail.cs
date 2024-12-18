namespace Common.DataTransferObjects.Quote
{
    public class QuoteDetail
    {
        public int ErrorLogId { get; set; }

        public string Message { get; set; }

        public DateTime DateCreated { get; set; }

        public string StackTrace { get; set; }

        public string Path { get; set; }

        public string StackTraceId { get; set; }

        public string Source { get; set; }

        public string UserIdentity { get; set; }

        public string BuildVersion { get; set; }
    }
}
