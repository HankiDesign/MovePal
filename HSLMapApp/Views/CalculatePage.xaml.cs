using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Naxam.Controls.Forms;
using Naxam.Mapbox;
using Newtonsoft.Json;
using Xamarin.Forms;
using System.Linq;
using Naxam.Mapbox.Expressions;
using Naxam.Mapbox.Layers;
using Point = Xamarin.Forms.Point;
using HSLMapApp.ViewModels;
using System.Collections.Generic;
using Naxam.Mapbox.Sources;
using HSLMapApp.Service;
using System.Threading.Tasks;
using System.Linq;
using System;
using HSLMapApp.Service.Models.Itinerary;
using HSLMapApp.Utils;

namespace HSLMapApp.Views
{
    public partial class CalculatePage : ContentPage
    {
        private IconImageSource iconImageSource;
        private IconImageSource markerImageSource;
        private List<Feature> symbolLayerIconFeatureList = new List<Feature>();
        private List<Feature> markerLayerIconFeatureList = new List<Feature>();
        private FeatureCollection featureCollection;
        private FeatureCollection markerCollection;
        private GeoJsonSource source;
        private GeoJsonSource markerSource;
        private SymbolLayer symbolLayer;
        private SymbolLayer markerLayer;

        public CalculatePage()
        {
            InitializeComponent();

            map.MapStyle = "mapbox://styles/hankide/ckdqi0ruy00qk19qs60ecjpub";

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);
        }

        private void HandleStyleLoaded(MapStyle obj)
        {
            map.Center = new LatLng(double.Parse((this.BindingContext as CalculateViewModel).Lat),
                double.Parse((this.BindingContext as CalculateViewModel).Lon));

            iconImageSource = (ImageSource)"RE.png";
            markerImageSource = (ImageSource)"Marker.png";

            map.Functions.AddStyleImage(iconImageSource);
            map.Functions.AddStyleImage(markerImageSource);

            featureCollection = new FeatureCollection(symbolLayerIconFeatureList);
            markerCollection = new FeatureCollection(markerLayerIconFeatureList);

            source = new GeoJsonSource
            {
                Id = "feature.memory.src",
                Data = featureCollection
            };

            markerSource = new GeoJsonSource
            {
                Id = "feature.marker.src",
                Data = markerCollection
            };

            map.Functions.AddSource(source);
            map.Functions.AddSource(markerSource);

            symbolLayer = new SymbolLayer("feature.symbol.layer", source.Id)
            {
                IconAllowOverlap = Expression.Literal(true),
                IconImage = Expression.Literal(iconImageSource.Id),
                IconOffset = Expression.Literal(new[] { -5, -5 }),
                IconSize = Expression.Literal(0.7)
            };

            markerLayer = new SymbolLayer("feature.marker.layer", markerSource.Id)
            {
                IconAllowOverlap = Expression.Literal(true),
                IconImage = Expression.Literal(markerImageSource.Id),
                IconOffset = Expression.Literal(new[] { -5, -5 }),
                IconSize = Expression.Literal(0.2)
            };

            map.Functions.AddLayer(symbolLayer);
            map.Functions.AddLayer(markerLayer);

            var feature = new Feature(new GeoJSON.Net.Geometry.Point(new Position(
                (this.BindingContext as CalculateViewModel).Lat,
                (this.BindingContext as CalculateViewModel).Lon)));

            symbolLayerIconFeatureList.Add(feature);
            featureCollection = new FeatureCollection(symbolLayerIconFeatureList);

            map.Functions.UpdateSource(source.Id, featureCollection);
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            map.Functions.AnimateCamera(new CameraPosition(map.Center, map.ZoomLevel, map.Pitch, 0), 1000);

            var pointLists = (this.BindingContext as CalculateViewModel).CalculatePoints();
            var allSuitablePoints = new List<GeoJSON.Net.Geometry.Point>();

            markerLayerIconFeatureList.Clear();

            var originPosition = new Position(double.Parse((this.BindingContext as CalculateViewModel).Lat),
                double.Parse((this.BindingContext as CalculateViewModel).Lon));

            var totalPoints = pointLists.SelectMany(list => list).Count();
            (this.BindingContext as CalculateViewModel).Progress = 0;
            var currentPoint = 0;

            foreach(var pointList in pointLists)
            {
                var suitablePoints = await CheckSuitablePoints(
                (int)double.Parse((this.BindingContext as CalculateViewModel).MaxTravelTime),
                pointList,
                double.Parse((this.BindingContext as CalculateViewModel).Lat),
                double.Parse((this.BindingContext as CalculateViewModel).Lon),
                currentPoint, totalPoints);

                allSuitablePoints.AddRange(suitablePoints);

                foreach (var point in suitablePoints)
                {
                    var feature = new Feature(new GeoJSON.Net.Geometry.Point(new Position(
                    point.Coordinates.Latitude, point.Coordinates.Longitude)));

                    markerLayerIconFeatureList.Add(feature);
                }

                currentPoint += pointList.Count;

                if(suitablePoints.Count > 1)
                {
                    for (int i = 0; i < suitablePoints.Count; i++)
                    {
                        var closest = GetClosestPosition(suitablePoints, i);

                        if (i + 1 < suitablePoints.Count)
                        {
                            DrawOverlay(new List<Position>
                            {
                                PointToPosition(suitablePoints[i]),
                                closest,
                                originPosition,
                                PointToPosition(suitablePoints[i])
                            });
                        }

                        else
                        {
                            DrawOverlay(new List<Position>
                            {
                                PointToPosition(suitablePoints[i]),
                                closest,
                                originPosition,
                                PointToPosition(suitablePoints[i])
                            });
                            
                        }
                    }
                }
            }

            (this.BindingContext as CalculateViewModel).Progress = 0;

            featureCollection = new FeatureCollection(markerLayerIconFeatureList);

            map.Functions.UpdateSource(markerSource.Id, markerCollection);            
        }

        public Position GetClosestPosition(List<GeoJSON.Net.Geometry.Point> points, int index)
        {
            if (index > 0 && index + 1 < points.Count)
            {
                var first = GetDistance(points[index], points[index - 1]);
                var second = GetDistance(points[index], points[index + 1]);

                return PointToPosition(first > second ? points[index + 1] : points[index - 1]);
            }

            else if (index == 0)
            {
                var first = GetDistance(points[index], points[points.Count - 1]);
                var second = GetDistance(points[index], points[index + 1]);

                return PointToPosition(first > second ? points[index + 1] : points[points.Count - 1]);
            }

            else
            {
                var first = GetDistance(points[index], points[index - 1]);
                var second = GetDistance(points[index], points[0]);

                return PointToPosition(first > second ? points[0] : points[points.Count - 1]);
            }
        }

        public double GetDistance(GeoJSON.Net.Geometry.Point first, GeoJSON.Net.Geometry.Point second)
        {
            var d1 = first.Coordinates.Latitude * (Math.PI / 180.0);
            var num1 = first.Coordinates.Longitude * (Math.PI / 180.0);
            var d2 = second.Coordinates.Latitude * (Math.PI / 180.0);
            var num2 = second.Coordinates.Longitude * (Math.PI / 180.0) - num1;
            var d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        private Position PointToPosition(GeoJSON.Net.Geometry.Point point)
        {
            return new Position(point.Coordinates.Latitude, point.Coordinates.Longitude);
        }

        private async Task<List<GeoJSON.Net.Geometry.Point>> CheckSuitablePoints(int maxTravelTime, List<GeoJSON.Net.Geometry.Point> points, double originLat, double originLon, int currentPoint, int maxPoints)
        {
            var result = await (this.BindingContext as CalculateViewModel).GetPointsWithinMaxTravelTime(maxTravelTime, points, originLat, originLon, currentPoint, maxPoints);

            DrawLegs(result.Item2);

            return result.Item1;
        }

        private void DrawLegs(List<LegGeometry> legGeometries)
        {
            foreach (var legGeometry in legGeometries)
            {
                var routeCoordinates = PolylineConverter.Decode(legGeometry.points);

                var lineString = new LineString(routeCoordinates);

                var lineFeatureCollection = new FeatureCollection(new List<Feature> { new Feature(lineString) });

                var lineSourceId = Guid.NewGuid().ToString();
                var lineLayerId = Guid.NewGuid().ToString();

                map.Functions.AddSource(new GeoJsonSource(lineSourceId, lineFeatureCollection)
                {
                    Options = new GeoJsonOptions()
                    {
                        LineMetrics = true
                    }
                });

                map.Functions.AddLayerBelow(new LineLayer(lineLayerId, lineSourceId)
                {
                    LineCap = LayerProperty.LINE_CAP_ROUND,
                    LineJoin = LayerProperty.LINE_JOIN_ROUND,
                    LineWidth = 2f,
                    LineColor = Expression.Rgb(49, 129, 28)
                }, symbolLayer.Id);
            }
        }

        private void DrawOverlay(List<Position> points)
        {
            //points.Add(points.First()); // Close the polygon
            var ls = new List<LineString>();
            ls.Add(new LineString(points));

            var polygon = new Polygon(new List<LineString> { new LineString(points) });

            var lineString = new LineString(points);

            //var polygon = new Polygon(ls);

            var newSourceId = System.Guid.NewGuid().ToString();
            var newLayerId = System.Guid.NewGuid().ToString();

            map.Functions.AddSource(new GeoJsonSource(newSourceId,
              new Feature(new Polygon(new[] { lineString }))));

            var polygonFillLayer = new FillLayer(newLayerId, newSourceId)
            {
                FillColor = Color.FromRgba(72, 217, 50,50)
            };

            //map.Functions.AddSource(new GeoJsonSource("source-id", polygon));

            var layers = map.Functions.GetLayers();

            //map.Functions.AddLayer(polygonFillLayer);
            // landcover, building, tunnel-street-minor-low
            if (map.Functions.GetLayers().Any(x => x.Id == "landcover"))
            {
                map.Functions.AddLayerBelow(polygonFillLayer, "landcover");
            }
            else
            {
                map.Functions.AddLayer(polygonFillLayer);
            }
            
        }
    }
}