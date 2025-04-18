namespace ConsoleApp.Application
{
    public record CommonBookingCommand
    {
        public string HotelId;
        public DateOnly StartDate;
        public DateOnly EndDate;
    }
}
