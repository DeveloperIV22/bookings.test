
namespace ConsoleApp.Domain.ValueObjects;

public record DateRange
{
    public readonly DateOnly Start;
    public readonly DateOnly End;

    public DateRange(DateOnly start, DateOnly end)
    {
        if (end < start)
            throw new ArgumentException("End date must be after start date.");

        Start = start;
        End = end;
    }
    public  List<DateOnly> GetDates()
    {
        List<DateOnly> datesToReturn = new List<DateOnly>();

        for (var date = this.Start; date <= this.End; date = date.AddDays(1))
        {
            datesToReturn.Add(date);
        }
        return datesToReturn;
    }

    public bool Overlaps(DateRange other) => Start <= other.End && End >= other.Start;

    public bool Contains(DateOnly date)
    {
        return date >= Start && date <= End;
    }

}
