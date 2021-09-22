using System;
using System.Text.Json;
using System.IO;
using System.Text.Json.Serialization;

namespace SampleApp
{
    public class NotificationRule : INotificationRule
    {
        public NotificationType NotificationType { get; set; }
        public string Details { get; set; }
    }

    public class StreamEvent : IStreamEvent
    {
        public string VehicleRego { get; set; }
        public DateTime RegisteredTo { get; set; }
        public VehicleStatus VehicleStatus { get; set; }
    }


    public static class LoaderFactory
    {
        private static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions { Converters = { new JsonStringEnumConverter() } };

        public static T LoadFromFile<T>(string fileName) => LoadFromJson<T>(File.ReadAllText(fileName));
        public static T LoadFromJson<T>(string jsonString) => JsonSerializer.Deserialize<T>(jsonString, _jsonSerializerOptions);

        public static INotificationRule[] LoadNotificationRulesFromFile(string fileName) => LoadFromJson<NotificationRule[]>(File.ReadAllText(fileName));
        public static IStreamEvent LoadStreamEventFromFile(string fileName) => LoadFromJson<StreamEvent>(File.ReadAllText(fileName));
    }
}
