using AccessGroupTest.Infrastructure;
using Newtonsoft.Json;

namespace ConsoleApp.Infrastructure
{
    public static class NewtonsoftCustomSerializer
    {
        public static T DeserializeObject<T>(string value, JsonConverter[] customConverters = null)
        {
            var settings = new JsonSerializerSettings();

            settings.Converters.Add(new BookingsDateConverter());
            settings.Converters.Add(new RoomCodeConverter());

            return JsonConvert.DeserializeObject<T>(value, settings);
        }
    }
}
