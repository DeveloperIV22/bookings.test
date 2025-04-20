using ConsoleApp.Domain.Models;
using ConsoleApp.Domain.ValueObjects;

namespace ConsoleApp.Domain.Services.Models
{
    public record FilteredBookingContext
    {
        public Hotel Hotel { get; init; }
        public IReadOnlyList<Booking> Bookings { get; init; }
        public DateRange DateRange { get; init; }

        public FilteredBookingContext(Hotel hotel, List<Booking> bookings, DateRange dateRange)
        {
            Hotel = hotel;
            Bookings = bookings;
            DateRange = dateRange;
        }
    }
}
