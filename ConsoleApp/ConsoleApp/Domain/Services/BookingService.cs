using ConsoleApp.Application;
using ConsoleApp.Domain.Models;
using ConsoleApp.Domain.ValueObjects;


namespace ConsoleApp.Domain.Services
{
    public class BookingService
    {     
        public FilteredBookingContext Filter(CommonBookingCommand commonBookingCommand, List<Hotel> hotels, List<Booking> bookings)
        {
            var hotel = hotels.Single(h => h.Id == commonBookingCommand.HotelId);
            var hotelBookings = bookings.Where(b => b.HotelId == commonBookingCommand.HotelId).ToList();
            var dateRange = new DateRange(commonBookingCommand.StartDate, commonBookingCommand.EndDate);

            return new FilteredBookingContext(hotel, hotelBookings, dateRange);
        }
        public int GetAvailability(FilteredBookingContext context, RoomCode roomType)
        {
            List<AvailableRoom> availableRoomsPerRoomType = CalculateAvailabilityPerRoomType(
                context.Hotel, context.Bookings, context.DateRange);

            var roomAvailability = availableRoomsPerRoomType
                .FirstOrDefault(x => x.RoomCode == roomType);

            if (roomAvailability == null)
            {
                return 0; // No rooms available for the specified room type
            }
            else
            {
                return roomAvailability.Available;
            }
        }

        public AllocatedRoomsResult AllocateRooms(FilteredBookingContext context,
         int totalPeopleToBook)
        {
            List<AvailableRoom> orderedTotalAvailableRoomsPerRoomType = 
                CalculateAvailabilityPerRoomType(context.Hotel, context.Bookings, context.DateRange)
                .Where(x => x.Available > 0).ToList();

            AllocatedRoomsResult result = new AllocatedRoomsResult(success: true);            

            if (orderedTotalAvailableRoomsPerRoomType.Count == 0)
            {
                result.Success = false;
                return result;
            }

            int remainingPeopleTobook = AllocateOptimallyByRoomSize(context.Hotel, orderedTotalAvailableRoomsPerRoomType, result, totalPeopleToBook);

            if (remainingPeopleTobook > 0)
            {
                result.Success = false;
            }

            return result;
        }

        // this method can probably be split in 2 but for clarity I think it should not.
        private int AllocateOptimallyByRoomSize(Hotel hotel, List<AvailableRoom> orderedTotalAvailableRoomsPerRoomType, AllocatedRoomsResult result, int totalPeopleToBook)
        {
            while (totalPeopleToBook > 0)
            {
                // check if we can fit everybody in one room type.
                foreach (var item in orderedTotalAvailableRoomsPerRoomType)
                {
                    int size = GetRoomTypeByCode(hotel,item.RoomCode).Size;
                    int availableRooms = item.Available;

                    if (totalPeopleToBook % size == 0 &&
                        availableRooms >= totalPeopleToBook / size)
                    {
                        int requiredRooms = totalPeopleToBook / size;

                        for (int i = 0; i < requiredRooms; i++)
                        {
                            result.CurrentlyAllocatedRooms.Add(new(item.RoomCode, false));
                        }

                        totalPeopleToBook = 0;
                        return totalPeopleToBook; // we booked everybody.
                    }
                }
                // if we didnt exactly fit everybody then try to fit other way 
                if (totalPeopleToBook > 0)
                {
                    var rooms = orderedTotalAvailableRoomsPerRoomType
                        .Where(x => x.Available > 0);

                    if(!rooms.Any()) // we dont have any rooms to book.
                    {
                        return totalPeopleToBook;
                    }
                    
                    AvailableRoom? selectedRoom = rooms. // maybe we have a room that fits exactly remaining peple to book.
                                       SingleOrDefault(x => GetRoomTypeByCode(hotel, x.RoomCode).Size >= totalPeopleToBook);

                    if(selectedRoom == null) // if not then just select the first, they are ordered by sie, from smallest to biggest.
                    {
                        selectedRoom = rooms.FirstOrDefault();
                    }
                     
                    if(selectedRoom != null)
                    {
                        int size = GetRoomTypeByCode(hotel, selectedRoom.RoomCode).Size;

                        result.CurrentlyAllocatedRooms.Add(new(selectedRoom.RoomCode, size > totalPeopleToBook ? true : false));

                        selectedRoom.Available = selectedRoom.Available - 1;

                        totalPeopleToBook = totalPeopleToBook - size;
                    }
                    else
                    {
                        // no more available rooms, return remaining people to book;
                        return totalPeopleToBook;
                    }                   
                }
            }
            return totalPeopleToBook;
        }


        private List<AvailableRoom> CalculateAvailabilityPerRoomType(Hotel hotel, IReadOnlyList<Booking> bookings, DateRange requiredDateRange)
        {
            Dictionary<RoomCode, Dictionary<DateOnly, int>> bookedCountPerRoomTypeForRequiredDateRange = GetBookedCountPerRoomPerCurrentDateInterval(hotel.Id, bookings, requiredDateRange);
            Dictionary<RoomCode, int> totalAvailableRoomsPerRoomType = GetAvailableCountPerRoomForCurrentDateInterval(hotel.RoomTypes, hotel.Rooms, bookedCountPerRoomTypeForRequiredDateRange);

            List<AvailableRoom> orderedTotalAvailableRoomsPerRoomType = totalAvailableRoomsPerRoomType
                .OrderBy(x => GetRoomTypeByCode(hotel,x.Key).Size)
                .Select(x => new AvailableRoom(x.Key,x.Value))
                .ToList();

            return orderedTotalAvailableRoomsPerRoomType;
        }

        private  Dictionary<RoomCode, int> GetAvailableCountPerRoomForCurrentDateInterval(IReadOnlyList<RoomType> roomTypes, IReadOnlyList<Room> rooms, Dictionary<RoomCode, Dictionary<DateOnly, int>> bookedCountPerRoomTypeForRequiredDateRange)
        {
            Dictionary<RoomCode, int> totalAvailableRoomsPerRoomType = new Dictionary<RoomCode, int>();

            foreach (var roomType in roomTypes)
            {
                int totalRoomsOfCurrentType = rooms.Count(r => r.RoomType == roomType.Code);
                int currentIntervalBooking = 0;
                if (bookedCountPerRoomTypeForRequiredDateRange.ContainsKey(roomType.Code))
                {
                    currentIntervalBooking = bookedCountPerRoomTypeForRequiredDateRange[roomType.Code].Max(f => f.Value);
                }

                totalAvailableRoomsPerRoomType[roomType.Code] = totalRoomsOfCurrentType - currentIntervalBooking;
            }

            return totalAvailableRoomsPerRoomType;
        }

        private RoomType GetRoomTypeByCode(Hotel hotel, RoomCode code)
        {
            return hotel.RoomTypes.Single(x => x.Code == code);
        }

        private static Dictionary<RoomCode, Dictionary<DateOnly, int>> GetBookedCountPerRoomPerCurrentDateInterval(string hotelId, IReadOnlyList<Booking> bookings, DateRange requiredDateRange)
        {
            Dictionary<RoomCode, Dictionary<DateOnly, int>> bookedCountPerRoomTypeForRequiredDateRange = new Dictionary<RoomCode, Dictionary<DateOnly, int>>();

            IReadOnlyList<Booking> filteredBookings = bookings.Where(b => b.HotelId == hotelId &&
                                                            b.DateRange.Overlaps(requiredDateRange)).ToList();

            foreach (var booking in filteredBookings)
            {
                List<DateOnly> bookingDates = new DateRange(booking.Arrival, booking.Departure).GetDates();
                RoomCode roomTypeKey = booking.RoomType;

                if (!bookedCountPerRoomTypeForRequiredDateRange.ContainsKey(roomTypeKey))
                {
                    bookedCountPerRoomTypeForRequiredDateRange[roomTypeKey] = new Dictionary<DateOnly, int>();
                }

                foreach (var date in bookingDates)
                {
                    if (!bookedCountPerRoomTypeForRequiredDateRange[roomTypeKey].ContainsKey(date))
                    {
                        bookedCountPerRoomTypeForRequiredDateRange[roomTypeKey][date] = 1;
                    }
                    else
                    {
                        bookedCountPerRoomTypeForRequiredDateRange[roomTypeKey][date]++;
                    }
                }
            }

            return bookedCountPerRoomTypeForRequiredDateRange;
        }

    }
}
