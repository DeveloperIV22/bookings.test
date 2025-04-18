namespace ConsoleApp.Application
{
    public record CommonBookingCommand
    {
        public required string HotelId;
        public DateOnly StartDate;
        public DateOnly EndDate;
    }
}
