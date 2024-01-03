
using PS.Core.Models;
using PS.Datastore.EFCore.Interfaces;
using PS.UseCases.Interfaces;

namespace PS.UseCases.MemberUseCase
{
    public class GetAllMembersUseCase : IGetAllMembersUseCase
    {        
        private readonly IMembersRepository MembersRepository;
        public GetAllMembersUseCase(IMembersRepository membersRepository)
        {
            MembersRepository = membersRepository;
        }

        public IQueryable<Member> Execute()
        {
            return MembersRepository.GetAll();
        }
    }
}
