using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Naxam.Controls.Forms;
using Naxam.Mapbox.Annotations;
using Xamarin.Forms;
using System.Collections.Generic;
using Point = GeoJSON.Net.Geometry.Point;
using System;
using GeoJSON.Net.Geometry;
using HSLMapApp.Service.GraphQLService;
using HSLMapApp.Service.Models.Itinerary;

namespace HSLMapApp.ViewModels
{
    [QueryProperty(nameof(Lon), nameof(Lon))]
    [QueryProperty(nameof(Lat), nameof(Lat))]
    [QueryProperty(nameof(MaxTravelTime), nameof(MaxTravelTime))]
    [QueryProperty(nameof(SearchStepRadius), nameof(SearchStepRadius))]
    [QueryProperty(nameof(SearchStepCount), nameof(SearchStepCount))]
    [QueryProperty(nameof(PointCount), nameof(PointCount))]
    public class CalculateViewModel : BaseViewModel
    {
        private MapStyle _currentMapStyle = MapStyle.DARK;
        private double _zoomLevel = 11;
        private int _pitch = 0;
        private ObservableCollection<Annotation> _annotations = new ObservableCollection<Annotation>();
        private bool _locationSelected;
        private ICommand _continueCommand;

        private string _address;
        private string _lon;
        private string _lat;
        private string _maxTravelTime;
        private string _searchStepRadius;
        private string _searchStepCount;
        private string _pointCount;

        private float _progress;

        private readonly ItineraryService _itineraryService;

        public CalculateViewModel()
        {
            Title = "Calculate";

            ZoomLevel = Device.RuntimePlatform == Device.Android ? 11 : 10;
            _itineraryService = new ItineraryService();
        }

        public MapStyle CurrentMapStyle
        {
            get => _currentMapStyle;
            set => SetProperty(ref _currentMapStyle, value);
        }

        public double ZoomLevel
        {
            get => _zoomLevel;
            set => SetProperty(ref _zoomLevel, value);
        }

        public int Pitch
        {
            get => _pitch;
            set => SetProperty(ref _pitch, value);
        }

        public ObservableCollection<Annotation> Annotations
        {
            get => _annotations;
            set => SetProperty(ref _annotations, value);
        }

        public bool LocationSelected
        {
            get => _locationSelected;
            set => SetProperty(ref _locationSelected, value);
        }

        public string Lon
        {
            get => _lon;
            set => _lon = value;
        }

        public string Lat
        {
            get => _lat;
            set => _lat = value;
        }

        public string Address
        {
            get => _address;
            set => _address = value;
        }

        public string PointCount
        {
            get => _pointCount;
            set => _pointCount = value;
        }

        public ICommand ContinueCommand
        {
            get => _continueCommand;
            set => SetProperty(ref _continueCommand, value);
        }

        public string MaxTravelTime
        {
            get => _maxTravelTime;
            set => SetProperty(ref _maxTravelTime, value);
        }

        public string SearchStepRadius
        {
            get => _searchStepRadius;
            set => SetProperty(ref _searchStepRadius, value);
        }

        public string SearchStepCount
        {
            get => _searchStepCount;
            set => SetProperty(ref _searchStepCount, value);
        }

        public float Progress
        {
            get => _progress;
            set => SetProperty(ref _progress, value);
        }

        public List<List<Point>> CalculatePoints()
        {
            IsBusy = true;

            var stepCount = int.Parse(_searchStepCount);
            double r = double.Parse(_searchStepRadius);
            var rIncrease = r;
            var pointCount = int.Parse(_pointCount);
            var pointLists = new List<List<Point>>();

            for(int i = 0; i < stepCount; i++)
            {
                pointLists.Add(GetPointsAroundOrigin(double.Parse(_lat), double.Parse(_lon),
                    r, pointCount));

                r += rIncrease;
                pointCount = (int)(pointCount * 1.5);
            }

            return pointLists;
        }

        private List<Point> GetPointsAroundOrigin(double originLat, double originLon, double r, int pointCount)
        {
            var points = new List<Point>();

            double angle = 0;
            var angleIncrease = (2 * Math.PI) / pointCount;

            double rEarth = 6371000; // in metres

            for (int i = 0; i < pointCount; i++)
            {
                var xDiff = r * Math.Cos(angle);
                var yDiff = r * Math.Sin(angle);

                angle += angleIncrease;

                var newLat = originLat + (yDiff / rEarth) * (180 / Math.PI);
                var newLon = originLon + (xDiff / rEarth) * (180 / Math.PI) / Math.Cos(newLat * Math.PI / 180);

                points.Add(new Point(new Position(newLat, newLon)));
            }

            return points;
        }

        public async Task<Tuple<List<Point>, List<LegGeometry>>> GetPointsWithinMaxTravelTime(int maxTravelTime, List<Point> points, double originLat, double originLon, int currentPoint, int totalPoints, int delayBetweenRequests = 100)
        {
            var suitablePoints = new List<Point>();
            var legs = new List<LegGeometry>();

            foreach (var point in points)
            {
                var itineraries = await _itineraryService.GetItinerary(originLat, originLon, point.Coordinates.Latitude, point.Coordinates.Longitude);

                foreach(var itinerary in itineraries.plan.itineraries)
                {
                    double totalTime = 0;

                    // Calculate the total time
                    foreach(var leg in itinerary.legs)
                    {
                        totalTime += leg.duration;
                    }

                    // Check if total time is below the maximum travel time in seconds
                    if (totalTime < maxTravelTime * 60)
                    {
                        suitablePoints.Add(point);
                        foreach (var leg in itinerary.legs)
                        {
                            legs.Add(leg.legGeometry);
                        }
                        continue;
                    }
                }

                currentPoint++;
                Progress = (float)currentPoint / totalPoints;

                await Task.Delay(delayBetweenRequests);
            }

            return new Tuple<List<Point>, List<LegGeometry>> (suitablePoints, legs);
        }
    }
}