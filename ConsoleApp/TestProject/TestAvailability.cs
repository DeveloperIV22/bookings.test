using ConsoleApp.Domain.ValueObjects;
using ConsoleApp.Domain.Services;
using ConsoleApp.Domain.Models.Enums;
using TestProject;
using ConsoleApp.Domain.Services.Models;

namespace ConsoleApp.Domain.Models.Tests
{
    [TestFixture]
    public class AvailabilityTest
    {
        private static readonly RoomCode SglRoomCode = new("SGL");
        private const string HotelId = "H1";

        // Common mock bookings
        private static readonly List<Booking> MockBookings = new()
        {
            new Booking
            {
                HotelId = HotelId,
                Arrival = new DateOnly(2024, 9, 1),
                Departure = new DateOnly(2024, 9, 3),
                RoomType = SglRoomCode,
                RoomRate = Booking.BookingRoomRate.Prepaid
            },
            new Booking
            {
                HotelId = HotelId,
                Arrival = new DateOnly(2024, 9, 2),
                Departure = new DateOnly(2024, 9, 5),
                RoomType = SglRoomCode,
                RoomRate = Booking.BookingRoomRate.Standard
            }
        };

        private static readonly List<Booking> OverbookingBookings = new()
        {
            new Booking
            {
                HotelId = HotelId,
                Arrival = new DateOnly(2024, 9, 1),
                Departure = new DateOnly(2024, 9, 3),
                RoomType = SglRoomCode,
                RoomRate = Booking.BookingRoomRate.Standard
            },
            new Booking
            {
                HotelId = HotelId,
                Arrival = new DateOnly(2024, 9, 1),
                Departure = new DateOnly(2024, 9, 3),
                RoomType = SglRoomCode,
                RoomRate = Booking.BookingRoomRate.Standard
            },
            new Booking
            {
                HotelId = HotelId,
                Arrival = new DateOnly(2024, 9, 1),
                Departure = new DateOnly(2024, 9, 3),
                RoomType = SglRoomCode,
                RoomRate = Booking.BookingRoomRate.Standard
            }
        };

        private Hotel CreateMockHotel() => HotelTestData.CreateMockHotel();

        [Test]
        public void Test_FullAvailability_NoBookings()
        {
            var hotel = CreateMockHotel();
            var bookings = new List<Booking>();
            var context = new FilteredBookingContext(
                hotel,
                bookings,
                new DateRange(new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 3)));

            int availableRooms = new BookingService().GetAvailability(context, SglRoomCode);

            Assert.That(availableRooms, Is.EqualTo(2));
        }

        [Test]
        public void Test_PartialAvailability_SingleRoomBooked()
        {
            var hotel = CreateMockHotel();
            var context = new FilteredBookingContext(
                hotel,
                MockBookings,
                new DateRange(new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 2)));

            int availableRooms = new BookingService().GetAvailability(context, SglRoomCode);

            Assert.That(availableRooms, Is.EqualTo(0));
        }

        [Test]
        public void Test_SpansMoreThanbookingRange()
        {
            var hotel = CreateMockHotel();
            var context = new FilteredBookingContext(
                hotel,
                MockBookings,
                new DateRange(new DateOnly(2024, 8, 1), new DateOnly(2024, 10, 1)));

            int availableRooms = new BookingService().GetAvailability(context, SglRoomCode);

            Assert.That(availableRooms, Is.EqualTo(0));
        }

        [Test]
        public void Test_Overbooking_Spans2Bookings()
        {
            var hotel = CreateMockHotel();
            var context = new FilteredBookingContext(
                hotel,
                MockBookings,
                new DateRange(new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 5)));

            int availableRooms = new BookingService().GetAvailability(context, SglRoomCode);

            Assert.That(availableRooms, Is.EqualTo(0));
        }

        [Test]
        public void Test_Overbooking_Spans1Booking_start()
        {
            var hotel = CreateMockHotel();
            var context = new FilteredBookingContext(
                hotel,
                MockBookings,
                new DateRange(new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 1)));

            int availableRooms = new BookingService().GetAvailability(context, SglRoomCode);

            Assert.That(availableRooms, Is.EqualTo(1));
        }

        [Test]
        public void Test_Overbooking_Spans1Booking_end()
        {
            var hotel = CreateMockHotel();
            var context = new FilteredBookingContext(
                hotel,
                MockBookings,
                new DateRange(new DateOnly(2024, 9, 4), new DateOnly(2024, 9, 5)));

            int availableRooms = new BookingService().GetAvailability(context, SglRoomCode);

            Assert.That(availableRooms, Is.EqualTo(1));
        }

        [Test]
        public void Test_Overbooking_Availability()
        {
            var hotel = CreateMockHotel();
            var context = new FilteredBookingContext(
                hotel,
                OverbookingBookings,
                new DateRange(new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 3)));

            int availableRooms = new BookingService().GetAvailability(context, SglRoomCode);

            Assert.That(availableRooms, Is.EqualTo(-1));
        }
    }
}
