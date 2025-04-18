using AccessGroupTest.Infrastructure;
using ConsoleApp.Domain.ValueObjects;
using ConsoleApp.Infrastructure;
using Newtonsoft.Json;

namespace ConsoleApp.Domain.Models;
public record Booking 
{
    public string HotelId { get; init; }

    [JsonConverter(typeof(BookingsDateConverter))]
    public DateOnly Arrival { get; init; }

    [JsonConverter(typeof(BookingsDateConverter))]
    public DateOnly Departure { get; init; }
        
    [JsonConverter(typeof(RoomCodeConverter))]
    public RoomCode RoomType { get; init; }

    public BookingRoomRate RoomRate { get; init; }

    public DateRange DateRange { get; init; }

    public Booking(string hotelId, 
        DateOnly arrival, 
        DateOnly departure, 
        RoomCode roomType, 
        BookingRoomRate roomRate)
    {
        HotelId = hotelId;
        Arrival = arrival;
        Departure = departure;
        RoomType = roomType;
        RoomRate = roomRate;
        DateRange = new DateRange(arrival, departure);
    }

    public enum BookingRoomRate
    {
        Prepaid,
        Standard
    }
}


