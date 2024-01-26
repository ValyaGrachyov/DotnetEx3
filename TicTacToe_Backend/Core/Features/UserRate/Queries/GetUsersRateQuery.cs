using Shared.CQRS;

namespace Features.UserRate.Queries;

public record GetUsersRateQuery() : IQuery<IEnumerable<UsersRateDto>>;
