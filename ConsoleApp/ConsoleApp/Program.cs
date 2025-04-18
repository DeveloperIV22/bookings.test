using CommandLine;
using ConsoleApp.Application;
using ConsoleApp.Domain.Models;
using ConsoleApp.Domain.Services;
using Newtonsoft.Json;

namespace AccessGroupTest;
class Program
{
    static void Main(string[] args)
    {
#if DEBUG
        RunBookingConsoleLoop(new HotelAndBookingOptions() {  BookingsFile = "bookings.json", HotelsFile = "hotels.json" });
#endif
#if !DEBUG
  Parser.Default.ParseArguments<HotelAndBookingOptions>(args)
                .WithParsed(options =>
                {
                    RunBookingConsoleLoop(options);
                }).WithNotParsed(errors =>
                {
                    Console.WriteLine("Invalid input.");
                }); 
#endif

    }

    public static void RunBookingConsoleLoop(HotelAndBookingOptions options)
    {
        Console.WriteLine($"Hotels File: {options.HotelsFile}");
        Console.WriteLine($"Bookings File: {options.BookingsFile}");

        if (!File.Exists(options.HotelsFile))
        {
            Console.WriteLine("Hotel file not found");
            return;
        }

        if (!File.Exists(options.BookingsFile))
        {
            Console.WriteLine("BookingsFile not found");
            return;
        }

        string jsonB = File.ReadAllText(options.BookingsFile);
        string jsonH = File.ReadAllText(options.HotelsFile);

        List<Booking> bookings = JsonConvert.DeserializeObject<List<Booking>>(jsonB) ?? throw new Exception("We cannot have null bookings");
        List<Hotel> hotels = JsonConvert.DeserializeObject<List<Hotel>>(jsonH) ?? throw new Exception("We cannot have null hotels");


        while (true)
        {
            Console.WriteLine(Environment.NewLine);

            string? input = Console.ReadLine()?.Trim();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("You did not write anything, write something please.");
                continue;
            }
            

            var bookingService = new BookingService();

            if (input.StartsWith("Availability", StringComparison.OrdinalIgnoreCase))
            {
                AvailabilityCommand avail = ConsoleParser.ParseAvailability(input);

                int availability = bookingService.GetAvailability(
                    bookingService.Filter(avail, hotels, bookings),
                    new RoomCode(avail.RoomCode));

                Console.WriteLine(availability);
            }
            else if (input.StartsWith("RoomTypes", StringComparison.OrdinalIgnoreCase))
            {
                RoomTypesCommand roomTypes = ConsoleParser.ParseRoomTypes(input);

                AllocatedRoomsResult allocatedRooms = bookingService.AllocateRooms(
                    bookingService.Filter(roomTypes, hotels, bookings),
                    roomTypes.NumberOfRooms);

                if (allocatedRooms.Success)
                {
                    var summary = roomTypes.HotelId + ": " +
                        string.Join(" ", allocatedRooms.CurrentlyAllocatedRooms
                            .Select(r => $"{r.RoomType}{(r.UnderBooked ? "!" : "")}"));

                    Console.WriteLine(summary);
                }
                else
                {
                    Console.WriteLine("Cannot allocate all rooms");
                }
            }
            else
            {
                Console.WriteLine("Invalid command");
            }
        }
    }


}
