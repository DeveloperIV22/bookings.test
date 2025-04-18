using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace ConsoleApp.Domain.Models.Enums;

[JsonConverter(typeof(StringEnumConverter))]
public enum Feature
{
    [EnumMember(Value = "Non-smoking")]
    NonSmoking,

    [EnumMember(Value = "Sea View")]
    SeaView
}