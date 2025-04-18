public record RoomCode
{
    private readonly string Value;

    public RoomCode(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new Exception("Cannot have nuill for RoomCode");
        }

        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }
}