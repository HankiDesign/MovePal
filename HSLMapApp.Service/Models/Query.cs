using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HSLMapApp.Service.Models
{
    public class Query
    {
        [JsonProperty(PropertyName = "size")]
        public int Size { get; set; }
        [JsonProperty(PropertyName = "private")]
        public bool Private { get; set; }
        [JsonProperty(PropertyName = "point.lat")]
        public double PointLat { get; set; }
        [JsonProperty(PropertyName = "point.lon")]
        public double PointLon { get; set; }
        [JsonProperty(PropertyName = "boundary.circle.lat")]
        public double BoundaryCircleLat { get; set; }
        [JsonProperty(PropertyName = "boundary.circle.lon")]
        public double BoundaryCircleLon { get; set; }
        [JsonProperty(PropertyName = "lang")]
        public string Lang { get; set; }
        [JsonProperty(PropertyName = "querySize")]
        public int QuerySize { get; set; }
    }
}