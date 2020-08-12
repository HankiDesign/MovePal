using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using HSLMapApp.Service.Models;
using HSLMapApp.Views;
using Naxam.Controls.Forms;
using Naxam.Mapbox.Annotations;
using Xamarin.Forms;

namespace HSLMapApp.ViewModels
{
    public class StartViewModel : BaseViewModel
    {
        private MapStyle _currentMapStyle = MapStyle.OUTDOORS;
        private double _zoomLevel = 11;
        private int _pitch = 0;
        private ObservableCollection<Annotation> _annotations = new ObservableCollection<Annotation>();
        private bool _locationSelected;
        private ICommand _continueCommand;

        private double _lon;
        private double _lat;
        private Feature _selectedLocation;
        private string _address;

        public StartViewModel()
        {
            Title = "Start";

            ZoomLevel = Device.RuntimePlatform == Device.Android ? 11 : 10;

            _continueCommand = new Command(async () => await ExecuteContinueCommand());
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

        public double Lon
        {
            get => _lon;
            set => _lon = value;
        }

        public double Lat
        {
            get => _lat;
            set => _lat = value;
        }

        public string Address
        {
            get => _address;
            set => _address = value;
        }

        public ICommand ContinueCommand
        {
            get => _continueCommand;
            set => SetProperty(ref _continueCommand, value);
        }

        public Feature SelectedLocation
        {
            get => _selectedLocation;
            set => _selectedLocation = value;
        }

        public void SetSelectedLocation(Feature selectedLocation, double lat, double lon)
        {
            SelectedLocation = selectedLocation;
            Address = _selectedLocation.Properties.label;
            Title = Address;
            Lat = lat;
            Lon = lon;
        }

        async Task ExecuteContinueCommand()
        {
            await Shell.Current.GoToAsync($"{nameof(SettingsPage)}?{nameof(Lon)}={Lon}&" +
                $"{nameof(Lat)}={Lat}&" +
                $"{nameof(Address)}={Address}");
        }
    }
}