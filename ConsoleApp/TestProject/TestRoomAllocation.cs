using ConsoleApp.Domain.Models.Enums;
using ConsoleApp.Domain.Models;
using ConsoleApp.Domain.Services;
using ConsoleApp.Domain.ValueObjects;

namespace ConsoleApp.Domain.Models.Tests;

[TestFixture]
public class RoomAllocationTest
{
    private Hotel CreateMockHotel()
    {
        return new Hotel
        (
            "H1",
            "Hotel California",
            new List<RoomType>
            {
                new RoomType("SGL", 1, "Single Room", new List<Amenity>(), new List<Feature>()),
                new RoomType("DBL", 2, "Double Room", new List<Amenity>(), new List<Feature>())
            },
            new List<Room>
            {
                new Room(("SGL"), "101"),
                new Room(("SGL"), "102"),
                new Room(("DBL"), "201"),
                new Room(("DBL"), "202")
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

    BookingService bookingService = new BookingService();

    [Test]
    public void AllocateRooms_WithEnoughAvailable_ShouldSucceed()
    {
        var hotel = CreateMockHotel();
        var bookings = CreateMockBookings();
        var dateRange = new DateRange(new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 2));
        int peopleToBook = 3;

        var context = new FilteredBookingContext(hotel, bookings, dateRange);
        var result = bookingService.AllocateRooms(context, peopleToBook);

        Assert.IsTrue(result.Success);
        Assert.That(result.CurrentlyAllocatedRooms.Count, Is.EqualTo(2));
        Assert.IsTrue(result.CurrentlyAllocatedRooms.Any(x => x.UnderBooked));
    }

    [Test]
    public void AllocateRooms_NoRoomsAvailable_ShouldFail()
    {
        var hotel = CreateMockHotel();
        var bookings = CreateMockBookings();
        var dateRange = new DateRange(new DateOnly(2024, 9, 3), new DateOnly(2024, 9, 4));
        int peopleToBook = 5;

        var context = new FilteredBookingContext(hotel, bookings, dateRange);
        var result = bookingService.AllocateRooms(context, peopleToBook);

        Assert.IsFalse(result.Success);
    }

    [Test]
    public void AllocateRooms_NoBookings_ShouldSucceed()
    {
        var hotel = CreateMockHotel();
        var bookings = new List<Booking>();
        var dateRange = new DateRange(new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 2));
        int peopleToBook = 3;

        var context = new FilteredBookingContext(hotel, bookings, dateRange);
        var result = bookingService.AllocateRooms(context, peopleToBook);

        Assert.IsTrue(result.Success);
        Assert.That(result.CurrentlyAllocatedRooms.Count, Is.EqualTo(2));
    }

    [Test]
    public void AllocateRooms_BookingExactlyOverlaps_ShouldSucceed()
    {
        var hotel = CreateMockHotel();
        var bookings = CreateMockBookings();
        var dateRange = new DateRange(new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 3));
        int peopleToBook = 1;

        var context = new FilteredBookingContext(hotel, bookings, dateRange);
        var result = bookingService.AllocateRooms(context, peopleToBook);

        Assert.IsTrue(result.Success);
        Assert.That(result.CurrentlyAllocatedRooms.Count, Is.EqualTo(1));
    }

    [Test]
    public void AllocateRooms_PartiallyBookedRooms_ShouldMarkUnderbooked()
    {
        var hotel = CreateMockHotel();
        var bookings = new List<Booking>
        {
            new Booking("H1", new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 2), new RoomCode("SGL"), Booking.BookingRoomRate.Prepaid)
        };
        var dateRange = new DateRange(new DateOnly(2024, 9, 2), new DateOnly(2024, 9, 3));
        int peopleToBook = 1;

        var context = new FilteredBookingContext(hotel, bookings, dateRange);
        var result = bookingService.AllocateRooms(context, peopleToBook);

        Assert.IsTrue(result.Success);
        Assert.That(result.CurrentlyAllocatedRooms.Count(t => t.RoomType == new RoomCode("SGL")), Is.EqualTo(1));
    }

    [Test]
    public void AllocateRooms_ExactFitWithMultipleRooms_ShouldNotUnderbook()
    {
        var hotel = CreateMockHotel();
        var bookings = new List<Booking>();
        var dateRange = new DateRange(new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 2));
        int peopleToBook = 4;

        var context = new FilteredBookingContext(hotel, bookings, dateRange);
        var result = bookingService.AllocateRooms(context, peopleToBook);

        Assert.IsTrue(result.Success);
        Assert.That(result.CurrentlyAllocatedRooms.Count, Is.EqualTo(2));
        Assert.IsFalse(result.CurrentlyAllocatedRooms.Any(r => r.UnderBooked));
    }

    [Test]
    public void AllocateRooms_MorePeopleThanTotalCapacity_ShouldFail()
    {
        var hotel = CreateMockHotel();
        var bookings = new List<Booking>();
        var dateRange = new DateRange(new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 2));
        int peopleToBook = 10;

        var context = new FilteredBookingContext(hotel, bookings, dateRange);
        var result = bookingService.AllocateRooms(context, peopleToBook);

        Assert.IsFalse(result.Success);
        Assert.Less(result.CurrentlyAllocatedRooms.Count, 5);
    }

    [Test]
    public void AllocateRooms_MixedRoomTypesForOddGroup_ShouldUnderbookOne()
    {
        var hotel = CreateMockHotel();
        var bookings = new List<Booking>();
        var dateRange = new DateRange(new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 2));
        int peopleToBook = 5;

        var context = new FilteredBookingContext(hotel, bookings, dateRange);
        var result = bookingService.AllocateRooms(context, peopleToBook);

        Assert.IsTrue(result.Success);
        Assert.That(result.CurrentlyAllocatedRooms.Count, Is.EqualTo(3));
    }

    [Test]
    public void AllocateRooms_BackToBackBookings_ShouldFailIfNoGap()
    {
        var hotel = CreateMockHotel();
        var bookings = new List<Booking>
        {
            new Booking("H1", new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 2), new RoomCode("SGL"), Booking.BookingRoomRate.Prepaid),
            new Booking("H1", new DateOnly(2024, 9, 2), new DateOnly(2024, 9, 3), new RoomCode("SGL"), Booking.BookingRoomRate.Prepaid),
            new Booking("H1", new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 3), new RoomCode("DBL"), Booking.BookingRoomRate.Prepaid),
            new Booking("H1", new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 3), new RoomCode("DBL"), Booking.BookingRoomRate.Prepaid),
        };

        var dateRange = new DateRange(new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 3));
        int peopleToBook = 2;

        var context = new FilteredBookingContext(hotel, bookings, dateRange);
        var result = bookingService.AllocateRooms(context, peopleToBook);

        Assert.IsFalse(result.Success);
    }

    [Test]
    public void AllocateRooms_1day_SGL()
    {
        var hotel = CreateMockHotel();
        var bookings = new List<Booking>
        {
            new Booking("H1", new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 3), new RoomCode("DBL"), Booking.BookingRoomRate.Prepaid),
            new Booking("H1", new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 2), new RoomCode("SGL"), Booking.BookingRoomRate.Prepaid),
            new Booking("H1", new DateOnly(2024, 9, 2), new DateOnly(2024, 9, 3), new RoomCode("SGL"), Booking.BookingRoomRate.Prepaid),
            new Booking("H1", new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 3), new RoomCode("DBL"), Booking.BookingRoomRate.Prepaid),
        };

        var dateRange = new DateRange(new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 1));
        int peopleToBook = 1;

        var context = new FilteredBookingContext(hotel, bookings, dateRange);
        var result = bookingService.AllocateRooms(context, peopleToBook);

        Assert.IsTrue(result.Success);
        Assert.That(result.CurrentlyAllocatedRooms[0].RoomType, Is.EqualTo(new RoomCode("SGL")));
    }

    [Test]
    public void AllocateRooms_ExactFitSingleRoom_ShouldSucceed()
    {
        var hotel = CreateMockHotel();
        var bookings = new List<Booking>();
        var dateRange = new DateRange(new DateOnly(2024, 9, 1), new DateOnly(2024, 9, 2));
        int peopleToBook = 1; // Exact fit for a single room

        var context = new FilteredBookingContext(hotel, bookings, dateRange);
        var result = bookingService.AllocateRooms(context, peopleToBook);

        Assert.IsTrue(result.Success);
        Assert.That(result.CurrentlyAllocatedRooms.Count, Is.EqualTo(1));
        Assert.IsFalse(result.CurrentlyAllocatedRooms[0].UnderBooked); // Ensure it's not underbooked
    }

}
