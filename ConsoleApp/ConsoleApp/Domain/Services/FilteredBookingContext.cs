using ConsoleApp.Domain.Models;
using ConsoleApp.Domain.ValueObjects;

namespace ConsoleApp.Domain.Services
{
    public record FilteredBookingContext
    {
        public Hotel Hotel;
        public IReadOnlyList<Booking> Bookings;
        public DateRange DateRange;

        public FilteredBookingContext(Hotel hotel, List<Booking> bookings, DateRange dateRange)
        {
            Hotel = hotel;
            Bookings = bookings;
            DateRange = dateRange;
        }
    }
}
