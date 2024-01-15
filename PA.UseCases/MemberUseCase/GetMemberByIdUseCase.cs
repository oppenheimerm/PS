
using PA.Core.Models;
using PA.Datastore.EFCore.Interfaces;
using PA.UseCases.Interfaces;

namespace PA.UseCases.MemberUseCase
{
    public class GetMemberByIdUseCase : IGetMemberByIdUseCase
    {
        public readonly IMembersRepository MembersRepository;

        public GetMemberByIdUseCase(IMembersRepository membersRepository)
        {
            MembersRepository = membersRepository;
        }


        public async Task<Member?> ExecuteAsync(Guid id)
        {
            return await MembersRepository.GetMember(id);
        }

    }
}
