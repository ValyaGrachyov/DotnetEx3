using Shared.CQRS;
using Shared.Results;

namespace Features.UserRate.Queries;

public class GetUsersRateQueryHandler: IQueryHandler<GetUsersRateQuery, IEnumerable<UsersRateDto>>
{
    public Task<Result<IEnumerable<UsersRateDto>>> Handle(GetUsersRateQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}