
using Newtonsoft.Json;

namespace ConsoleApp.Infrastructure
{

    public class RoomCodeConverter : JsonConverter<RoomCode>
    {
        public override RoomCode ReadJson(JsonReader reader, Type objectType, RoomCode existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType != JsonToken.String || reader.Value == null)
            {
                throw new JsonSerializationException("Invalid value");
            }
            return new RoomCode(reader.Value.ToString());
        }

        public override void WriteJson(JsonWriter writer, RoomCode value, JsonSerializer serializer)
        {
            writer.WriteValue(value);
        }
    }
}
