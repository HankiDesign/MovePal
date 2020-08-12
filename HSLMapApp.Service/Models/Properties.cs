using System;
namespace HSLMapApp.Service.Models
{
    public class Properties
    {
        public string id { get; set; }
        public string gid { get; set; }
        public string layer { get; set; }
        public string source { get; set; }
        public string source_id { get; set; }
        public string name { get; set; }
        public string housenumber { get; set; }
        public string street { get; set; }
        public string postalcode { get; set; }
        public string postalcode_gid { get; set; }
        public double confidence { get; set; }
        public double distance { get; set; }
        public string accuracy { get; set; }
        public string country { get; set; }
        public string country_gid { get; set; }
        public string country_a { get; set; }
        public string region { get; set; }
        public string region_gid { get; set; }
        public string localadmin { get; set; }
        public string localadmin_gid { get; set; }
        public string locality { get; set; }
        public string locality_gid { get; set; }
        public string neighbourhood { get; set; }
        public string neighbourhood_gid { get; set; }
        public string label { get; set; }
    }
}
