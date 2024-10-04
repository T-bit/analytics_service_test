using System.Collections.Generic;
using Newtonsoft.Json;

namespace TestEventService.Events
{
    public struct EventsData
    {
        [JsonProperty("events")]
        public List<Event> Events;
    }
}