
using Microsoft.AspNetCore.Identity;

namespace PS.Core.Models
{
    public class Person : IdentityUser
    {
        public DateTime? JoinDate { get; } = DateTime.Now;
        public DateTime? LastLogin { get; set; }
        /// <summary>
        /// If instance is employee, it's a photo of person, for member
        /// it's the car photo
        /// </summary>
        [PersonalData]
        public string? Photo { get; set; } = string.Empty;
        [PersonalData]
        public DateTime? DOB { get; set; }
    }
}
