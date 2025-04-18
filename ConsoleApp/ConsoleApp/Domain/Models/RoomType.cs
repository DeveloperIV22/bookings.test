using System.Text.Json.Serialization;
using ConsoleApp.Domain.Models.Enums;

namespace ConsoleApp.Domain.Models;

public record RoomType
{
    public readonly RoomCode Code;
    public readonly int Size;
    public readonly string Description;
    public readonly List<Amenity> Amenities;
    public readonly List<Feature> Features;
  
    [JsonConstructor]
    public RoomType(string code, int size, string description, List<Amenity> amenities, List<Feature> features)
    {
        Code = new RoomCode(code);
        Size = size;
        Description = description;
        Amenities = amenities;
        Features = features;
    }
}
