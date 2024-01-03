namespace PS.API.Repositories.Interfaces
{
    public interface IPhotoFileRepository
    {
        Task<(FileInfo fileInfo, bool Success, string ErrorMessage)> AddPhotoAsync(IFormFile file, string path);
    }
}
