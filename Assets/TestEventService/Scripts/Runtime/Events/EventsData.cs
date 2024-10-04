using System.Collections.Generic;
using Newtonsoft.Json;

namespace TestEventService.Events
{
    public sealed class EventsData
    {
        [JsonProperty("events")]
        public List<Event> Events = new();
    }
}