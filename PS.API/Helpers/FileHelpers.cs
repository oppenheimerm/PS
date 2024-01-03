namespace PS.API.Helpers
{
    public static class FileHelpers
    {
        private static string[] permittedExtensions = { ".jpg", ".jpeg", ".png" };

        public static bool ValidImageFile(IFormFile file)
        {
            var ext = Path.GetExtension(file.FileName.ToLowerInvariant());
            if (string.IsNullOrEmpty(ext) || !permittedExtensions.Contains(ext))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
