
using PS.Core.Models;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace PS.Core.ViewModels
{
    public class AddEmployeeVM
    {
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [StringLength(8)]
        public string? Initials { get; set; }
        [Required]
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public DateTime? LastLogin { get; set; } = DateTime.Now;
        [Required]
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        public PrimaryDepartment PrimaryDepartment { get; set; } = PrimaryDepartment.GENERAL_SERVICE;
        [Required]
        [StringLength(5)]
        public string Title { get; set; } = string.Empty;
        [StringLength(256)]
        public string? Photo { get; set; } = string.Empty;

    }
}
