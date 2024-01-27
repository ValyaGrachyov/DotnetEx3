namespace Domain.UserStatistics;

public class UserRate
{
    public Guid Id { get; set; }
    public string UserId { get; init; }
    
    public string Username { get; init; }

    public int Rate { get; init; }
}
