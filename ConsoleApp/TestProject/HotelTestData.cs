
using ConsoleApp.Domain.Models.Enums;
using ConsoleApp.Domain.Models;
using ConsoleApp.Domain.ValueObjects;

namespace TestProject
{
    public class HotelTestData
    {
        public static Hotel CreateMockHotel()
        {
            return new Hotel
            {
                Id = "H1", 
                Name = "Hotel California", 
                RoomTypes = new List<RoomType>
                    {
                        new()
                        {
                            Code = new RoomCode("SGL"),
                            Size = 1,
                            Description = "Single Room",
                            Amenities = new List<Amenity>(),
                            Features = new List<Feature>()
                        },
                        new()
                        {
                            Code = new RoomCode("DBL"),
                            Size = 2,
                            Description = "Double Room",
                            Amenities = new List<Amenity>(),
                            Features = new List<Feature>()
                        }
                    },
                Rooms = new List<Room>
                    {
                        new() { RoomType = new RoomCode("SGL"), RoomId = "101" },
                        new() { RoomType = new RoomCode("SGL"), RoomId = "102" },
                        new() { RoomType = new RoomCode("DBL"), RoomId = "201" },
                        new() { RoomType = new RoomCode("DBL"), RoomId = "202" }
                    }
            };
        }

        public static List<Booking> CreateMockBookings()
        {
            return new List<Booking>
                {
                    new()
                    {
                        HotelId = "H1",
                        Arrival = new DateOnly(2024, 9, 1),
                        Departure = new DateOnly(2024, 9, 3),
                        RoomType = new RoomCode("SGL"),
                        RoomRate = Booking.BookingRoomRate.Prepaid
                    },
                    new()
                    {
                        HotelId = "H1",
                        Arrival = new DateOnly(2024, 9, 2),
                        Departure = new DateOnly(2024, 9, 5),
                        RoomType = new RoomCode("SGL"),
                        RoomRate = Booking.BookingRoomRate.Standard
                    }
                };
        }
    }
}
