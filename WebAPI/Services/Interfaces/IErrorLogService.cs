using Common.DataTransferObjects.ErrorLog;

namespace WebAPI.Services.Interfaces
{
    public interface IErrorLogService
    {
        Task<ErrorMessageDetail> LogApiError(HttpContext context);
    }
}
