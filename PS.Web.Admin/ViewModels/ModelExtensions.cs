﻿using PS.Core.Models;
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
                    VendorPostcode = vm.VendorPostcode,
                    CountryId = vm.CountryId,
                    VendorCode = vm.VendorCode.ToUpperInvariant(),
                    Logo = vm.Logo
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
                    CountryId = vm.CountryId,
                    CountryCode = vm.CountryCode,
                    AccessibleToiletNearby = vm.AccessibleToiletNearby,
                    PayAtPump = vm.PayAtPump,
                    PayByApp = vm.PayByApp,
                };
            }
        }


        public static EditPetrolStationVM ToEditStationVM(this Station station)
        {
            if (station == null)
            {
                throw new ArgumentNullException(nameof(station));
            }
            else
            {
                return new EditPetrolStationVM
                {
                    StationName = station.StationName,
                    StationAddress = station.StationAddress,
                    StationPostcode = station.StationPostcode,
                    Latitude = station.Latitude,
                    Longitude = station.Longitude,
                    VendorId = station.VendorId,
                    CountryId = station.CountryId,
                    CountryCode = station.CountryCode,
                    Id = station.Id,
                    PayAtPump = station.PayAtPump,
                    PayByApp = station.PayByApp,
                    AccessibleToiletNearby = station.AccessibleToiletNearby
                };
            }
        }

        public static Station ToStation(this EditPetrolStationVM vm)
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
                    StationPostcode = vm.StationPostcode,
                    Latitude = vm.Latitude,
                    Longitude = vm.Longitude,
                    VendorId = vm.VendorId,
                    CountryId = vm.CountryId,
                    CountryCode = vm.CountryCode,
                    AccessibleToiletNearby = vm.AccessibleToiletNearby,
                    PayAtPump = vm.PayAtPump,
                    PayByApp = vm.PayByApp
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
