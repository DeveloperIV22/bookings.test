namespace ConsoleApp.Domain.Models;

public record Room
{
    public readonly RoomCode RoomType;
    public readonly string RoomId; 

    public Room(RoomCode roomType, string roomId)
    {
        RoomType = roomType;
        RoomId = roomId;
    }
}
