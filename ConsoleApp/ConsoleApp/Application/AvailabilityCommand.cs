namespace ConsoleApp.Application;
public record AvailabilityCommand : CommonBookingCommand
{
    public required string RoomCode;
}