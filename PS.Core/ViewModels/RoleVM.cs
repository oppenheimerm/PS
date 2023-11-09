
namespace PS.Core.ViewModels
{
    //https://www.c-sharpcorner.com/article/list-of-users-with-roles-in-mvc-asp-net-identity//
    public class RoleVM
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
