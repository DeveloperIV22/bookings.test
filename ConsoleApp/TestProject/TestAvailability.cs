using ConsoleApp.Domain.ValueObjects;
using ConsoleApp.Domain.Services;
using ConsoleApp.Domain.Models.Enums;

namespace ConsoleApp.Domain.Models.Tests
{
    [TestFixture]
    public class AvailabilityTest
    {
        // Mock Data Setup
        private Hotel CreateMockHotel()
        {
            return new Hotel
            (
                "H1", // Hotel ID
                "Hotel California", // Hotel Name (from JSON)
                new List<RoomType>
                {
                    new RoomType(("SGL"), 1, "Single Room",
                        new List<Amenity>(),
                        new List<Feature>()),
                    new RoomType("DBL", 2, "Double Room", new List<Amenity>(), new List<Feature>())
                },
                new List<Room>
                {
                    new Room(new RoomCode("SGL"), "101"),
                    new Room(new RoomCode("SGL"), "102"),
                    new Room(new RoomCode("DBL"), "201"),
                    new Room(new RoomCode("DBL"), "202")
                }
            );
        }

        private List<Booking> CreateMockBookings()
        {
            return new List<Booking>
            {
                new Booking("H1", new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 3), new RoomCode("SGL"), Booking.BookingRoomRate.Prepaid),
                new Booking("H1", new DateOnly(2024, 9, 2), new DateOnly(2024, 9, 5), new RoomCode("SGL"), Booking.BookingRoomRate.Standard)
            };
        }

        [Test]
        public void Test_FullAvailability_NoBookings()
        {
            var hotel = CreateMockHotel();
            var bookings = new List<Booking>();
            var dateRange = new DateRange(new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 3));
            var roomCode = new RoomCode("SGL");

            var context = new FilteredBookingContext(hotel, bookings, dateRange);
            int availableRooms = new BookingService().GetAvailability(context, roomCode);

            Assert.That(availableRooms, Is.EqualTo(2));
        }

        [Test]
        public void Test_PartialAvailability_SingleRoomBooked()
        {
            var hotel = CreateMockHotel();
            var bookings = CreateMockBookings();
            var dateRange = new DateRange(new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 2));
            var roomCode = new RoomCode("SGL");

            var context = new FilteredBookingContext(hotel, bookings, dateRange);
            int availableRooms = new BookingService().GetAvailability(context, roomCode);

            Assert.That(availableRooms, Is.EqualTo(0));
        }

        [Test]
        public void Test_SpansMoreThanbookingRange()
        {
            var hotel = CreateMockHotel();
            var bookings = CreateMockBookings();
            var dateRange = new DateRange(new DateOnly(2024, 8, 1), new DateOnly(2024, 10, 1));
            var roomCode = new RoomCode("SGL");

            var context = new FilteredBookingContext(hotel, bookings, dateRange);
            int availableRooms = new BookingService().GetAvailability(context, roomCode);

            Assert.That(availableRooms, Is.EqualTo(0));
        }

        [Test]
        public void Test_Overbooking_Spans2Bookings()
        {
            var hotel = CreateMockHotel();
            var bookings = CreateMockBookings();
            var dateRange = new DateRange(new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 5));
            var roomCode = new RoomCode("SGL");

            var context = new FilteredBookingContext(hotel, bookings, dateRange);
            int availableRooms = new BookingService().GetAvailability(context, roomCode);

            Assert.That(availableRooms, Is.EqualTo(0));
        }

        [Test]
        public void Test_Overbooking_Spans1Booking_start()
        {
            var hotel = CreateMockHotel();
            var bookings = CreateMockBookings();
            var dateRange = new DateRange(new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 1));
            var roomCode = new RoomCode("SGL");

            var context = new FilteredBookingContext(hotel, bookings, dateRange);
            int availableRooms = new BookingService().GetAvailability(context, roomCode);

            Assert.That(availableRooms, Is.EqualTo(1));
        }

        [Test]
        public void Test_Overbooking_Spans1Booking_end()
        {
            var hotel = CreateMockHotel();
            var bookings = CreateMockBookings();
            var dateRange = new DateRange(new DateOnly(2024, 9, 4), new DateOnly(2024, 9, 5));
            var roomCode = new RoomCode("SGL");

            var context = new FilteredBookingContext(hotel, bookings, dateRange);
            int availableRooms = new BookingService().GetAvailability(context, roomCode);

            Assert.That(availableRooms, Is.EqualTo(1));
        }
        [Test]
        public void Test_Overbooking_Availability()
        {
            var hotel = CreateMockHotel();

            var bookings = new List<Booking>
            {
                new Booking("H1", new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 3), new RoomCode("SGL"), Booking.BookingRoomRate.Standard),
                new Booking("H1", new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 3), new RoomCode("SGL"), Booking.BookingRoomRate.Standard),



                new Booking("H1", new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 3), new RoomCode("SGL"), Booking.BookingRoomRate.Standard),
            };

            var dateRange = new DateRange(new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 3));
            var roomCode = new RoomCode("SGL");

            var context = new FilteredBookingContext(hotel, bookings, dateRange);
            int availableRooms = new BookingService().GetAvailability(context, roomCode);

            Assert.That(availableRooms, Is.EqualTo(-1));
        }
    }
}
