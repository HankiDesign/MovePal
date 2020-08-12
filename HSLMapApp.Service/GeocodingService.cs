using System.Threading.Tasks;
using HSLMapApp.Service.Models;
using RestSharp;
using System.Linq;

namespace HSLMapApp.Service
{
    public class GeocodingService
    {
        private const string baseUrl = "http://api.digitransit.fi/geocoding/v1";

        public async Task<Feature> ReverseEncodeAddress(double lat, double lon)
        {
            var client = new RestClient(baseUrl);

            var request = new RestRequest("reverse")
                .AddParameter("point.lat", lat.ToString())
                .AddParameter("point.lon", lon.ToString())
                .AddParameter("lang", "fi")
                .AddParameter("size", "1");

            var address = await client.GetAsync<FeatureCollection>(request);

            return address.Features.First();
        }
    }
}
