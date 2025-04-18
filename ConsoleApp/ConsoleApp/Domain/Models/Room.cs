using System.Text.Json.Serialization;
using ConsoleApp.Infrastructure;

namespace ConsoleApp.Domain.Models;

public record Room
{
    [JsonConverter(typeof(RoomCodeConverter))]
    public readonly RoomCode RoomType;
    public readonly string RoomId; 

    [JsonConstructor]
    public Room(string roomType, string roomId)
    {
        RoomType = new RoomCode(roomType);
        RoomId = roomId;
    }
}
