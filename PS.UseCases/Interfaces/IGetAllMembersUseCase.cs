
using PS.Core.Models;

namespace PS.UseCases.Interfaces
{
    public interface IGetAllMembersUseCase
    {
        IQueryable<Member> Execute();
    }
}
