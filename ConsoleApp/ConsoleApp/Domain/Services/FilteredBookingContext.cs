using ConsoleApp.Domain.Models;
using ConsoleApp.Domain.ValueObjects;

namespace ConsoleApp.Domain.Services
{
    public record FilteredBookingContext
    {
        public Hotel Hotel { get; set; }
        public List<Booking> Bookings { get; set; }
        public DateRange DateRange { get; set; }

        public FilteredBookingContext(Hotel hotel, List<Booking> bookings, DateRange dateRange)
        {
            Hotel = hotel;
            Bookings = bookings;
            DateRange = dateRange;
        }
    }
}
