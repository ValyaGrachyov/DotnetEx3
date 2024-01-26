using DataAccess;
using Shared.CQRS;
using Shared.Results;

namespace Features.UserRate.Queries;

public class GetUsersRateQueryHandler: IQueryHandler<GetUsersRateQuery, IEnumerable<UsersRateDto>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersRateQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public Task<Result<IEnumerable<UsersRateDto>>> Handle(GetUsersRateQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}