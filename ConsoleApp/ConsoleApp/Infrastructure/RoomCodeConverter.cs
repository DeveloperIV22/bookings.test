
using Newtonsoft.Json;

namespace ConsoleApp.Infrastructure
{

    public class RoomCodeConverter : JsonConverter<RoomCode>
    {
        public override RoomCode ReadJson(JsonReader reader, Type objectType, RoomCode existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // Ensure the reader's value is a string and convert it to a RoomCode
            return new RoomCode(reader.Value.ToString());
        }

        public override void WriteJson(JsonWriter writer, RoomCode value, JsonSerializer serializer)
        {
            // Serialize the RoomCode as a string
            writer.WriteValue(value);
        }
    }
}
