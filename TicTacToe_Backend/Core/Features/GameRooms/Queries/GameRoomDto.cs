namespace Features.GameRooms.Queries;

public record GameRoomDto
{
    public string Id { get; init; }

    public string CreatorUsername { get; init; }

    public DateTime CreatedAtUtc { get; init; }

    public bool IsBusy { get; init; }

    public int MaxAllowedUserRating { get; init; }
}
