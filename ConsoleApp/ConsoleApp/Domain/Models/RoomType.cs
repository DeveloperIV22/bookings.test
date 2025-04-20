using ConsoleApp.Domain.Models.Enums;

namespace ConsoleApp.Domain.Models;

public record RoomType
{
    public required RoomCode Code;
    public required int Size;
    public required string Description;
    public required List<Amenity> Amenities;
    public required List<Feature> Features;
  
}
