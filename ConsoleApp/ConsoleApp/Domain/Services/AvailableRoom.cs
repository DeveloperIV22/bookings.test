public record AvailableRoom
{
    public RoomCode RoomCode { get; init; }
    public int Available { get; set; } 
    public AvailableRoom(RoomCode roomCode, int available)
    {
        RoomCode = roomCode;
        Available = available;
    }
}
