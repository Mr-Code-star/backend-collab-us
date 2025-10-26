using backend_collab_us.IAM.domain.model.agregates;
using backend_collab_us.IAM.domain.model.queries;
using backend_collab_us.IAM.domain.repositories;
using backend_collab_us.IAM.domain.Services;

namespace backend_collab_us.IAM.Application.Internal.QueryServices;

public class UserQueryService(IUserRepository userRepository) : IUserQueryService
{
    public async Task<User?> Handle(GetUserByIdQuery query)
    {
        return await userRepository.FindByIdAsync(query.UserId);
    }

    public async Task<IEnumerable<User>> Handle(GetAllUsersQuery query)
    {
        return await userRepository.ListAsync();
    }

    public async Task<User?> Handle(GetUserByEmailQuery query)
    {
        return await userRepository.FindByEmail(query.Email);
    }
}