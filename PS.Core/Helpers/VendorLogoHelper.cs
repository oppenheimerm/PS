
namespace PS.Core.Helpers
{
    public enum VendorLogoSize
    {
        ExtraSmall,
        Small,
        Medium,
        Large,
    }

    public static class VendorLogoHelper
    {
        public static string GetVendorLogo(string logoName, VendorLogoSize size, string url)
        {
            var logoname = string.Empty;
            logoName = size switch
            {
                VendorLogoSize.ExtraSmall => url + logoName + "_logo_40_x_40.jpg",
                VendorLogoSize.Small => url + logoName + "_logo_80_x_80.jpg",
                VendorLogoSize.Medium => url + logoName + "_logo_100_x_100.jpg",
                VendorLogoSize.Large => url + logoName + "_logo_200_x_200.jpg",
                _ => throw new InvalidOperationException(),
            };

            return logoName;
        }
    }
}
