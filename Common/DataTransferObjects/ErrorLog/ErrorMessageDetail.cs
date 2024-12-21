using Common.Constants;

namespace Common.DataTransferObjects.ErrorLog
{
    public class ErrorMessageDetail
    {
        public string TraceId { get; set; }
        public string Type { get; set; }
        public string Message { get; set; }

        public ErrorMessageDetail()
        {

        }

        public ErrorMessageDetail(string traceId, string type, string message)
        {
            TraceId = traceId;
            Type = type;
            Message = message;
        }

        public ErrorMessageDetail(string type, string message)
        {
            Type = type;
            Message = message;
        }

        public ErrorMessageDetail(string message)
        {
            Type = ErrorMessageTypeConstant.BadRequest;
            Message = message;
        }
    }
}