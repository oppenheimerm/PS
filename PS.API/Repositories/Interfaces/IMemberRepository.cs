using PS.Core.Models;

namespace PS.API.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        Task<Member?> GetMemberById(Guid Id);
        Task<(Member? member, bool Success, string ErrorMessage)> UpdateUserPhoto(Member? member, string photoName);
    }
}
