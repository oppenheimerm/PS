using PS.Core.Models;
using PS.Core.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PS.Web.Admin.ViewModels
{
    /// <summary>
    /// Extension methods for ViewModels to Model.  Yes, I know I can use AutoMapper here,
    /// but at this juncture I think it's a bit of overkill.
    /// </summary>
    public static class ModelExtensions
    {
        public static Country ToCountryVM(this AddCountryVM vm)
        {
            if (vm == null)
            {
                throw new ArgumentNullException(nameof(vm));
            }else
            {
                return new Country
                {
                    CountryCode = vm.CountryCode,
                    CountryName = vm.CountryName
                };
            }
        }

        public static Vendor ToVendorVM(this AddVendorVM vm)
        {
            if (vm == null)
            {
                throw new ArgumentNullException(nameof(vm));
            }
            else
            {
                return new Vendor
                {
                    VendorName = vm.VendorName,
                    VendorAddress = vm.VendorAddress,
                    VendorAddress2 = vm.VendorAddress2,
                    VendorAddress3 = vm.VendorAddress3,
                    VendorPostcode = vm.VendorPostcode,
                    CountryId = vm.CountryId,
                    VendorCode = vm.VendorCode.ToUpperInvariant(),
                };
            }
        }

        public static Station ToStationVM(this AddPetrolStationVM vm)
        {
            if (vm == null)
            {
                throw new ArgumentNullException(nameof(vm));
            }
            else
            {
                return new Station
                {
                    StationName = vm.StationName,
                    StationAddress = vm.StationAddress,
                    StationAddress2 = vm.StationAddress2,
                    StationPostcode = vm.StationPostcode,
                    Latitude = vm.Latitude,
                    Longitude = vm.Longitude,
                    VendorId = vm.VendorId,
                    CountryId = vm.CountryId

                };
            }
        }

        public static Employee ToEmployee(this AddEmployeeVM vm)
        {
            if(vm == null)
            {
                throw new ArgumentNullException(nameof(vm));
            }
            else
            {
                return new Employee
                {
                    //Id = 
                    JoinDate = DateTime.UtcNow,
                    FirstName = vm.FirstName,
                    LasttName = vm.LastName,
                    Initials = vm.Initials,
                    UserName = vm.UserName.ToLower(),
                    PrimaryDepartment = vm.PrimaryDepartment,
                    Title = vm.Title,
                    Photo = vm.Photo,
                    Email = vm.Email
                };
            }
        }
    }
}
