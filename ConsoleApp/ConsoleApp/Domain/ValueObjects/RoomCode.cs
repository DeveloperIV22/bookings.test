public record RoomCode
{
    private string Value;

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

    public static implicit operator string(RoomCode v)
    {
        throw new NotImplementedException();
    }
}