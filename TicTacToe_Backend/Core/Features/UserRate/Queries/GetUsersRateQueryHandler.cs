using DataAccess;
using MassTransit.Initializers;
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

    public async Task<Result<IEnumerable<UsersRateDto>>> Handle(GetUsersRateQuery request, CancellationToken cancellationToken)
    {
        var usersRate = await _userRepository.GetUsersRate(); 
        
         return  new Ok<IEnumerable<UsersRateDto>>(usersRate.Select(x => new UsersRateDto()
        {
            Username = x.Username,
            Rate = x.Rate
        }));
    }
}