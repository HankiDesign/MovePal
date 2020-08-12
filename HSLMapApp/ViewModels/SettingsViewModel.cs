using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using HSLMapApp.Service.Models;
using HSLMapApp.Views;
using Xamarin.Forms;

namespace HSLMapApp.ViewModels
{
    [QueryProperty(nameof(Lon), nameof(Lon))]
    [QueryProperty(nameof(Lat), nameof(Lat))]
    [QueryProperty(nameof(Address), nameof(Address))]
    public class SettingsViewModel : BaseViewModel
    {
        private string _lon;
        private string _lat;
        private string _address;
        private double _maxTravelTime = 25;
        private double _searchStepRadius = 700;
        private short _searchStepCount = 1;
        private int _pointCount = 4;

        private List<int> _searchStepsList;
        private List<int> _radiusPointsList;

        private ICommand _continueCommand;

        private Feature _selectedLocation;        

        public SettingsViewModel()
        {
            Title = "Settings";

            _continueCommand = new Command(async () => await ExecuteContinueCommand());
            _searchStepsList = Enumerable.Range(1, 30).ToList();
            _radiusPointsList = Enumerable.Range(1, 40).ToList();
        }

        public List<int> SearchStepsList
        {
            get => _searchStepsList;
            set => SetProperty(ref _searchStepsList, value);
        }

        public List<int> RadiusPointsList
        {
            get => _radiusPointsList;
            set => SetProperty(ref _radiusPointsList, value);
        }

        public string Lon
        {
            get => _lon;
            set => SetProperty(ref _lon, value);
        }

        public string Lat
        {
            get => _lat;
            set => SetProperty(ref _lat, value);
        }

        public string Address
        {
            get => _address;
            set => SetProperty(ref _address, Uri.UnescapeDataString(value ?? string.Empty));
        }

        public double MaxTravelTime
        {
            get => _maxTravelTime;
            set => SetProperty(ref _maxTravelTime, value);
        }

        public double SearchStepRadius
        {
            get => _searchStepRadius;
            set => SetProperty(ref _searchStepRadius, value);
        }

        public int PointCount
        {
            get => _pointCount;
            set => SetProperty(ref _pointCount, value);
        }

        public short SearchStepCount
        {
            get => _searchStepCount;
            set => SetProperty(ref _searchStepCount, value);
        }

        public ICommand ContinueCommand
        {
            get => _continueCommand;
            set => SetProperty(ref _continueCommand, value);
        }

        async Task ExecuteContinueCommand()
        {
            await Shell.Current.GoToAsync($"{nameof(CalculatePage)}?{nameof(Lon)}={Lon}&" +
                $"{nameof(Lat)}={Lat}&" +
                $"{nameof(MaxTravelTime)}={MaxTravelTime}&" +
                $"{nameof(PointCount)}={PointCount}&" +
                $"{nameof(SearchStepRadius)}={SearchStepRadius}&" +
                $"{nameof(SearchStepCount)}={SearchStepCount}");
        }
    }
}