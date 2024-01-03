using PS.API.Helpers;
using PS.API.Repositories.Interfaces;

namespace PS.API.Repositories
{
    public class PhotoFileRepository : IPhotoFileRepository
    {
        private IWebHostEnvironment Environment;
        private ImageProcessor Processor;
        private readonly ILogger<PhotoFileRepository> Logger;

        public PhotoFileRepository(IWebHostEnvironment environment, ILogger<PhotoFileRepository> logger)
        {
            Environment = environment;
            Processor = new ImageProcessor();
            Logger = logger;
        }

        public async Task<(FileInfo fileInfo, bool Success, string ErrorMessage)> AddPhotoAsync(IFormFile file, string path) {

            try {
                //  We'll be creating a new file name
                var fileNameExt = Path.GetExtension(file.FileName).ToLower();
                string newFileName = Guid.NewGuid().ToString("N") + fileNameExt;
                string filePath = Path.Combine(Environment.WebRootPath, Helpers.Constants.RootDirectory, Path.GetFileName(newFileName));

                if (File.Exists(filePath))
                {
                    filePath = Path.ChangeExtension(filePath, file.GetHashCode() + Path.GetExtension(filePath));
                }


                var imageStream = file.OpenReadStream();
                var fileInfo = Processor.CreateImage(imageStream, filePath);

                //var fiileInfo = new FileInfo(filePath);
                Logger.LogInformation($"Photo: {file.FileName} added  at {DateTime.UtcNow}");
                return (new FileInfo(filePath), true, string.Empty);
            }
            catch(Exception ex)
            {
                //  Log true error
                Logger.LogError($"Failed to add {file.FileName}, the following error occured: {ex.ToString()} at {DateTime.UtcNow}");
                //  Return friendly error
                return (new FileInfo(string.Empty), false, "Faild to uplaod file");
            }
        }
    }
}
