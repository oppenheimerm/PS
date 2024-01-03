
using PS.Core.Models;

namespace PS.Datastore.EFCore.Interfaces
{
    public interface IMembersRepository
    {
        Task<Member?> GetMember(Guid id);
        IQueryable<Member> GetAll();
    }
}
