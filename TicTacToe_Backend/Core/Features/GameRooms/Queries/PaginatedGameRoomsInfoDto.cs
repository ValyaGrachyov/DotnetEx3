namespace Features.GameRooms.Queries;

public record PaginatedGameRoomsInfoDto
{
    public IEnumerable<GameRoomDto> RequestedRooms { get; set; } = Array.Empty<GameRoomDto>();

    public long TotalRooms { get; set; }
}
