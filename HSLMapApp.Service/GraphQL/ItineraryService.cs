using System.Threading.Tasks;
using HSLMapApp.Service.Models.Itinerary;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;

namespace HSLMapApp.Service.GraphQLService
{
    public class ItineraryService
    {
        public async Task<Data> GetItinerary(double originLat, double originLon,
            double destLat, double destLon)
        {
            var itineraryRequest = new GraphQLRequest
            {
                Query = "{plan(from: {lat: " + originLat + ", lon: " + originLon + "}" +
                        "to: {lat: " + destLat + ", lon: " + destLon + "}" +
                        "date: \"2020-08-17\"" +
                        "time: \"07:30:00\"" +
                        "numItineraries: 1) {itineraries {legs {" +
                            @"startTime
                            endTime
                            mode
                            duration
                            realTime
                            distance
                            transitLeg
                            legGeometry {
                                      length
                                      points
                                    }
                          }
                        }
                      }
                    }
                }"
            };


            using (var graphQLClient = new GraphQLHttpClient("https://api.digitransit.fi/routing/v1/routers/hsl/index/graphql", new NewtonsoftJsonSerializer()))
            {
                var graphQLResponse = await graphQLClient.SendQueryAsync<Data>(itineraryRequest);

                return graphQLResponse.Data;
            }
        }
    }
}