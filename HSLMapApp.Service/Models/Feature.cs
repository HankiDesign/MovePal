using System;
using Newtonsoft.Json;

namespace HSLMapApp.Service.Models
{
    public class Feature
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "geometry")]
        public Geometry Geometry { get; set; }
        [JsonProperty(PropertyName = "properties")]
        public Properties Properties { get; set; }
    }
}
