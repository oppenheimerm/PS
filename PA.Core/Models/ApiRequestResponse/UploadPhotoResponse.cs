
namespace PA.Core.Models.ApiRequestResponse
{
	public class UploadPhotoResponse : BaseResponse
	{
		public string? FileName { get; set; }
		public Member? Member { get; set; }
	}
}
