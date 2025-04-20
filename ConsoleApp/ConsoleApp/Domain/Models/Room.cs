namespace ConsoleApp.Domain.Models;

public record Room
{
    public required RoomCode RoomType { get; init; }
    public required  string RoomId { get; init; }  
}
