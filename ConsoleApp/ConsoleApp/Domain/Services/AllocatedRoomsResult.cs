namespace ConsoleApp.Domain.Services;

public class AllocatedRoomsResult
{
    public List<AllocatedRooms> CurrentlyAllocatedRooms;
    public bool Success;
    public AllocatedRoomsResult(bool success)
    {
        Success = success;
        CurrentlyAllocatedRooms = new List<AllocatedRooms>();
    }

    public record AllocatedRooms
    {
        public RoomCode RoomType { get; init; }
        public bool UnderBooked { get; init; }
        public AllocatedRooms(RoomCode roomType, bool underBooked)
        {
            RoomType = roomType;
            UnderBooked = underBooked;
        }
    }
}
