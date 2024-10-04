using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using UnityEngine;

namespace TestEventService.Utilities
{
    public static class SerializationUtility
    {
        private static readonly JsonSerializerSettings Settings = new()
        {
            Error = OnError,
            NullValueHandling = NullValueHandling.Ignore
        };

        private static void OnError(object sender, ErrorEventArgs errorEventArgs)
        {
            var errorContext = errorEventArgs.ErrorContext;

            Debug.LogException(errorContext.Error);

            errorContext.Handled = true;
        }

        public static string Serialize<T>(T value, bool prettyPrint)
            where T : class
        {
            var formatting = prettyPrint
                ? Formatting.Indented
                : Formatting.None;

            return JsonConvert.SerializeObject(value, formatting, Settings);
        }

        public static T Deserialize<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json, Settings);
        }
    }
}