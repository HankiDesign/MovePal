using Newtonsoft.Json;

namespace HSLMapApp.Service.Models
{
    public class Engine
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
        [JsonProperty(PropertyName = "author")]
        public string Author { get; set; }
        [JsonProperty(PropertyName = "version")]
        public string Version { get; set; }
    }
}