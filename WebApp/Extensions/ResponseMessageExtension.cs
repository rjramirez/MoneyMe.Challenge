using Common.DataTransferObjects.ErrorLog;
using Newtonsoft.Json;

namespace WebApp.Extensions
{
    public static class ResponseMessageExtension
    {
        public static async Task<ErrorMessageDetail> GetErrorMessage(this HttpResponseMessage httpResponseMessage)
        {
            return JsonConvert.DeserializeObject<ErrorMessageDetail>(await httpResponseMessage.Content.ReadAsStringAsync());
        }
    }
}
