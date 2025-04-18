namespace ConsoleApp.Domain.Services;

public class AllocatedRoomsResult
{
    public List<AllocatedRooms> CurrentlyAllocatedRooms { get; set; } = new List<AllocatedRooms>();
    public bool Success { get; set; }

    public class AllocatedRooms
    {
        public RoomCode RoomType { get; set; }


        public bool UnderBooked { get; set; }

        public AllocatedRooms(RoomCode roomType, bool underBooked)
        {
            RoomType = roomType;
            UnderBooked = underBooked;
        }
    }

}
