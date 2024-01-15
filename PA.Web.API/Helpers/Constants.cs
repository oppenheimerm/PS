namespace PA.Web.API.Helpers
{
    public static class Constants
    {
        public static readonly string RootDirectory = "usersImages";

        public enum ImageType
        {
            Thumbnail = 240,
            Cover = 540,
            Full = 1200
        }
    }
}
