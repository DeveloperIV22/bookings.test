using ConsoleApp.Domain.ValueObjects;

namespace ConsoleApp.Domain.Models;
public record Booking 
{
    public required string HotelId { get; init; }
    public required DateOnly Arrival { get; init; }
    public required DateOnly Departure { get; init; }        
    public required RoomCode RoomType { get; init; }
    public required BookingRoomRate RoomRate { get; init; }
    public DateRange DateRange { get => new DateRange(Arrival, Departure); }



    public enum BookingRoomRate
    {
        Prepaid,
        Standard
    }
}


