
namespace PS.Core.Helpers
{
    public enum RoleType { 
        AdminVendorsManager, 
        AdminStationsManager, 
        AdminUserManager, 
        Employee, 
        User, 
        Vendor 
    }

    public enum UserPrefix { 
        Mr, 
        Mrs, 
        Miss, 
        Dr 
    }

    public static class Constants
    {
		public const string VendorLogoUrlPrefix = "~/img/assets/vendorLogos/";
	}
    
}
