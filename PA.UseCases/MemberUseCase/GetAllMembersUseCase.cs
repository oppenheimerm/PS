
using PA.Core.Models;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.MemberUseCase
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
