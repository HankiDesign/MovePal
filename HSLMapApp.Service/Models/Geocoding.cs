using System;
using Newtonsoft.Json;

namespace HSLMapApp.Service.Models
{
    public class Geocoding
    {
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }
        [JsonProperty(PropertyName = "attribution")]
        public string Attribution { get; set; }
        [JsonProperty(PropertyName = "query")]
        public Query Query { get; set; }
        [JsonProperty(PropertyName = "engine")]
        public Engine Engine { get; set; }
        [JsonProperty(PropertyName = "timestamp")]
        public long Timestamp { get; set; }
    }
}
