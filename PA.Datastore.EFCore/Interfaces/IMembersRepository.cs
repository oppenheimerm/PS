
using PA.Core.Models;

namespace PA.Datastore.EFCore.Interfaces
{
    public interface IMembersRepository
    {
        Task<Member?> GetMember(Guid id);
        IQueryable<Member> GetAll();
        Task<Member?> GetMemberById(Guid Id);
        Task<(Member? member, bool Success, string ErrorMessage)> UpdateUserPhoto(Member? member, string photoName);
    }
}
