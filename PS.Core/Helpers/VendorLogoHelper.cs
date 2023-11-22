
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
        public static string GetVendorLogo(string logoName, VendorLogoSize size)
        {
            logoName = size switch
            {
                VendorLogoSize.ExtraSmall => Constants.VendorLogoUrlPrefix + logoName + "_logo_40_x_40.jpg",
                VendorLogoSize.Small => Constants.VendorLogoUrlPrefix + logoName + "_logo_80_x_80.jpg",
                VendorLogoSize.Medium => Constants.VendorLogoUrlPrefix + logoName + "_logo_100_x_100.jpg",
                VendorLogoSize.Large => Constants.VendorLogoUrlPrefix + logoName + "_logo_200_x_200.jpg",
                _ => throw new InvalidOperationException(),
            };

            return logoName;
        }
    }
}
