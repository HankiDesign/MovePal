using System;
namespace HSLMapApp.Service.Models.Itinerary
{
    public class Leg
    {
        public object startTime { get; set; }
        public object endTime { get; set; }
        public string mode { get; set; }
        public double duration { get; set; }
        public bool realTime { get; set; }
        public double distance { get; set; }
        public bool transitLeg { get; set; }
        public LegGeometry legGeometry { get; set; }
    }
}
