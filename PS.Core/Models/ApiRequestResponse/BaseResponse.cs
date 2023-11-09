
namespace PS.Core.Models.ApiRequestResponse
{
    public class BaseResponse
    {
        public int StatusCode { get; set; } = 401;
        public string Message { get; set; } = string.Empty;
    }
}
