using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HSLMapApp.Service.Models
{
    public class FeatureCollection
    {
        [JsonProperty(PropertyName = "geocoding")]
        public Geocoding Geocoding { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "features")]
        public List<Feature> Features { get; set; }
        [JsonProperty(PropertyName = "bbox")]
        public List<double> Bbox { get; set; }
    }
}
