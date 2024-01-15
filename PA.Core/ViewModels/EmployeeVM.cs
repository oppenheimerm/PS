using PA.Core.Models;

namespace PA.Core.ViewModels
{
    public class EmployeeVM
    {
        public Guid Id { get; set; }
        public DateTime? JoinDate { get; set; } = DateTime.Now;
        public string FirstName { get; set; } = string.Empty;
        public string LasttName { get; set; } = string.Empty;
        public string? Initials { get; set; } = string.Empty;
        public PrimaryDepartment? PrimaryDepartment { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Photo { get; set; } = string.Empty;
    }
}
