﻿using CommandLine;
using ConsoleApp.Application;
using ConsoleApp.Domain.Models;
using ConsoleApp.Domain.Services;
using Newtonsoft.Json;

namespace AccessGroupTest;
class Program
{
    static void Main(string[] args)
    {

        Parser.Default.ParseArguments<HotelAndBookingOptions>(args)
                .WithParsed(options =>
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
                        Console.WriteLine($"BookingsFile not found");
                        return; 
                    }

                    string jsonB = File.ReadAllText(options.BookingsFile);
                    string jsonH = File.ReadAllText(options.HotelsFile);

                    var bookings = JsonConvert.DeserializeObject<List<Booking>>(jsonB);
                    var hotels = JsonConvert.DeserializeObject<List<Hotel>>(jsonH);


                    while (true)
                    {
                        Console.WriteLine(System.Environment.NewLine);

                        string input = Console.ReadLine().Trim();

                        var bookingService = new BookingService();

                        if (input.StartsWith("Availability"))
                        {
                            AvailabilityCommand avail = ConsoleParser.ParseAvailability(input);

                            int availability = bookingService.GetAvailability(
                                bookingService.Filter(avail, hotels, bookings),
                                new RoomCode(avail.RoomCode));

                            Console.Write(availability.ToString());

                        }
                        else if (input.StartsWith("RoomTypes"))
                        {
                            RoomTypesCommand roomTypes = ConsoleParser.ParseRoomTypes(input);

                            AllocatedRoomsResult allocatedRooms = bookingService.AllocateRooms(
                                bookingService.Filter(roomTypes, hotels, bookings),
                                roomTypes.NumberOfRooms);

                            if (allocatedRooms.Success)
                            {
                                var summary = roomTypes.HotelId + ": " +
                                string.Join(" ", allocatedRooms.CurrentlyAllocatedRooms.Select(r => $"{r.RoomType}{(r.UnderBooked ? "!" : "")}"));

                                Console.WriteLine(summary);
                            }
                            else
                            {
                                Console.WriteLine("Cannot allocate all rooms");
                            }
                        }
                        else
                        {
                            Console.WriteLine("invalid command");
                        }
                    }

                }).WithNotParsed(errors =>
                {
                    Console.WriteLine("Invalid input.");
                }); 
    }

}
