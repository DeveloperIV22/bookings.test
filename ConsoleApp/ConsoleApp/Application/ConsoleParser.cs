namespace ConsoleApp.Application;
public class ConsoleParser
{
    static DateOnly ParseDate(string raw)
    {
        return new DateOnly(
            int.Parse(raw.Substring(0, 4)),
            int.Parse(raw.Substring(4, 2)),
            int.Parse(raw.Substring(6, 2))
        );
    }

    public static AvailabilityCommand ParseAvailability(string input)
    {
        input = input.Replace("Availability(", "").Replace(")", "");



        var parts = input.Split(',');

        var hotelId = parts[0].Trim();
        var datePart = parts[1].Trim();
        var roomCode = parts[2].Trim();

        DateOnly startDate, endDate;

        if (datePart.Contains("-"))
        {
            var dates = datePart.Split('-');
            startDate = ParseDate(dates[0]);
            endDate = ParseDate(dates[1]);
        }
        else
        {
            startDate = ParseDate(datePart);
            endDate = startDate;
        }

        return new AvailabilityCommand
        {
            HotelId = hotelId,
            StartDate = startDate,
            EndDate = endDate,
            RoomCode = roomCode
        };
    }
    
    public static RoomTypesCommand ParseRoomTypes(string input)
    {


        input = input.Replace("RoomTypes(", "").Replace(")", "");

        var parts = input.Split(',');

        var hotelId = parts[0].Trim();
        var datePart = parts[1].Trim();
        var numberOfRooms = int.Parse(parts[2].Trim());

        DateOnly startDate, endDate;

        if (datePart.Contains("-"))
        {
            var dates = datePart.Split('-');
            startDate = ParseDate(dates[0]);
            endDate = ParseDate(dates[1]);
        }
        else
        {
            startDate = ParseDate(datePart);
            endDate = startDate;
        }

        return new RoomTypesCommand
        {
            HotelId = hotelId,
            StartDate = startDate,
            EndDate = endDate,
            NumberOfRooms = numberOfRooms
        };
    }

}