using CommandLine;

namespace ConsoleApp.Application
{
    public class HotelAndBookingOptions
    {
        // Use the `Option` attribute to define the command-line arguments
        [Option('h', "hotels", Required = true, HelpText = "Path to the hotels JSON file.")]
        public required string HotelsFile { get; init; }

        [Option('b', "bookings", Required = true, HelpText = "Path to the bookings JSON file.")]
        public required string BookingsFile { get; init; }
    }    
}
