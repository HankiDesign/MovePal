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

namespace HSLMapApp.Views
{
    public partial class StartPage : ContentPage
    {
        private IconImageSource iconImageSource;
        private List<Feature> symbolLayerIconFeatureList = new List<Feature>();
        private FeatureCollection featureCollection;
        private GeoJsonSource source;
        private SymbolLayer symbolLayer;
        private GeocodingService geocodingService;

        public StartPage()
        {
            InitializeComponent();

            map.Center = new LatLng(60.169212, 24.938788);

            map.DidFinishLoadingStyleCommand = new Command<MapStyle>(HandleStyleLoaded);

            geocodingService = new GeocodingService();
        }

        private Feature _feature;
        private LatLng _featureLatLng;

        private void HandleStyleLoaded(MapStyle obj)
        {
            var fillExtrusionLayer = new FillExtrusionLayer("3d-buildings", "composite")
            {
                SourceLayer = "building",
                Filter = Expression.Eq(Expression.Get("extrude"), "true"),
                MinZoom = 10,
                FillExtrusionColor = Color.LightGray,
                FillExtrusionHeight = Expression.Interpolate(
                    Expression.Exponential(1f),
                    Expression.Zoom(),
                    Expression.CreateStop(15, (0)),
                    Expression.CreateStop(16, Expression.Get("height"))
                ),
                FillExtrusionBase = Expression.Get("min_height"),
                FillExtrusionOpacity = 0.9f
            };

            //map.Functions.AddLayer(fillExtrusionLayer);

            map.Functions.AnimateCamera(map.Camera, 1000);

            iconImageSource = (ImageSource)"RE.png";
            map.Functions.AddStyleImage(iconImageSource);

            featureCollection = new FeatureCollection(symbolLayerIconFeatureList);

            source = new GeoJsonSource
            {
                Id = "feature.memory.src",
                Data = featureCollection
            };

            map.Functions.AddSource(source);

            symbolLayer = new SymbolLayer("feature.symbol.layer", source.Id)
            {
                IconAllowOverlap = Expression.Literal(true),
                IconImage = Expression.Literal(iconImageSource.Id),
                IconOffset = Expression.Literal(new[] { -5, -5 }),
                IconSize = Expression.Literal(0.7)
            };

            map.Functions.AddLayer(symbolLayer);

            map.DidTapOnMapCommand = new Command<(LatLng position, Point point)>(HandleMapTapped);
        }

        private async void HandleMapTapped((LatLng position, Point point) obj)
        {
            // Get features at the tapped point
            var features = map.Functions.QueryFeatures(obj.position, null);

            var filtered = features.Where((arg) => arg.Properties != null);
            foreach (Feature feat in filtered)
            {
                var str = JsonConvert.SerializeObject(feat);
                System.Diagnostics.Debug.WriteLine(str);
            }

            // Add the origin
            _featureLatLng = obj.position;
            var feature = new Feature(new GeoJSON.Net.Geometry.Point(new Position(_featureLatLng.Lat, _featureLatLng.Long)));

            symbolLayerIconFeatureList.Clear();
            symbolLayerIconFeatureList.Add(feature);
            featureCollection = new FeatureCollection(symbolLayerIconFeatureList);

            map.Functions.UpdateSource(source.Id, featureCollection);

            (this.BindingContext as StartViewModel).SetSelectedLocation(await geocodingService.ReverseEncodeAddress(_featureLatLng.Lat, _featureLatLng.Long),
                _featureLatLng.Lat, _featureLatLng.Long);

            (this.BindingContext as StartViewModel).LocationSelected = true;
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
        }
    }
}