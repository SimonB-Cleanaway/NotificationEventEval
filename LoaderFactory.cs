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
        private static JsonSerializerOptions _jsonSerializerOptions = new() { Converters = { new JsonStringEnumConverter() } };

        public static INotificationRule[] LoadNotificationRulesFromFile(string fileName) => LoadNotificationRulesFromJson(File.ReadAllText(fileName));
        public static INotificationRule[] LoadNotificationRulesFromJson(string jsonString) => JsonSerializer.Deserialize<NotificationRule[]>(jsonString, _jsonSerializerOptions);
        public static IStreamEvent LoadStreamEventFromFile(string fileName) => LoadStreamEventFromJson(File.ReadAllText(fileName));
        public static IStreamEvent LoadStreamEventFromJson(string jsonString) => JsonSerializer.Deserialize<StreamEvent>(jsonString, _jsonSerializerOptions);
    }
}
