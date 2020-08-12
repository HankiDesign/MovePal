using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace HSLMapApp.Service.Models
{
    public class Geometry
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "coordinates")]
        public List<double> coordinates { get; set; }
    }
}
