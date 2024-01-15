
using PA.Core.Models;

namespace PA.UseCases.Interfaces
{
    public interface IGetAllMembersUseCase
    {
        IQueryable<Member> Execute();
    }
}
