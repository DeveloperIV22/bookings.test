namespace ConsoleApp.Domain.Models;

public record Hotel
{
    public required string Id { get; init; }
    public required string Name { get; init; }
    public required IReadOnlyList<RoomType> RoomTypes { get; init; }
    public required IReadOnlyList<Room> Rooms { get; init; }
}
