using Newtonsoft.Json;

namespace TestEventService.Events
{
    public struct Event
    {
        [JsonProperty("type")]
        public string Type;

        [JsonProperty("data")]
        public string Data;
    }
}