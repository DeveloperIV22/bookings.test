using Newtonsoft.Json;

namespace AccessGroupTest.Infrastructure;

public class BookingsDateConverter : JsonConverter<DateOnly>
{
    private const string format = "yyyyMMdd";

    public override DateOnly ReadJson(JsonReader reader, Type objectType,
        DateOnly date, bool hasExistingValue, JsonSerializer serializer)
    {
        return ReturnDate(reader, format);
    }

    private static DateOnly ReturnDate(JsonReader reader, string format)
    {
        if (reader.Value == null || (reader.Value is string stringV
                                    && string.IsNullOrWhiteSpace(stringV)))
            throw new Exception($"We have a NULL value in DateOnlyConverter.");

        string stringifiedDate = reader.Value.ToString();
        if (DateOnly.TryParseExact(stringifiedDate, format, out var dateToReturn))
        {
            return dateToReturn;
        }
        else
        {
            // this should be catch in some global error handler 
            throw new Exception($"Cannot convert date ${stringifiedDate}");
        }
    }

    public override void WriteJson(JsonWriter writer, DateOnly value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString(format));
    }
}