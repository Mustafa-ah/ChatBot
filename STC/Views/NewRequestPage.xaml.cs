using STC.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace STC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewRequestPage : ContentPage
    {
        NewRequestPageViewModel ViewModel;
        public NewRequestPage()
        {
            InitializeComponent();
            ViewModel = BindingContext as NewRequestPageViewModel;

            map.MapClicked += Map_MapClicked;

            MessagingCenter.Subscribe<MapPage,Location>(this, "LocationSelected", OnLocationSelected);

            MapAppearing();

            if (Device.RuntimePlatform != Device.iOS)
            {
                MyLocationStack.IsVisible = false;
            }
        }

        private void OnLocationSelected(MapPage obj, Location location)
        {

            try
            {
                if (ViewModel != null)
                {

                    ViewModel.UserLocation = new Location(location.Latitude, location.Longitude);

                    MoveCameraToCurrentLocation();
                    AddPinToCurrentLocation();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected async Task MapAppearing()
        {
            base.OnAppearing();

            try
            {
                MoveCameraToCurrentLocation();
                AddPinToCurrentLocation();
                await GetCurrentLocation();
            }

            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        CancellationTokenSource cts;

        async Task GetCurrentLocation()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (status == PermissionStatus.Granted)
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(10));
                    cts = new CancellationTokenSource();
                    var cLcation = await Geolocation.GetLocationAsync(request, cts.Token);

                    if (cLcation != null)
                    {
                        ViewModel.UserLocation = cLcation;

                        MoveCameraToCurrentLocation();
                        AddPinToCurrentLocation();
                    }
                }

            }
            catch (FeatureNotSupportedException fnsEx)
            {
                // Handle not supported on device exception
            }
            catch (FeatureNotEnabledException fneEx)
            {
                // Handle not enabled on device exception
            }
            catch (PermissionException pEx)
            {
                // Handle permission exception
            }
            catch (Exception ex)
            {
                // Unable to get location
            }
        }

        protected override void OnDisappearing()
        {
            if (cts != null && !cts.IsCancellationRequested)
                cts.Cancel();
            base.OnDisappearing();
        }

        protected override bool OnBackButtonPressed()
        {
            ViewModel.NewRequestBackCommand.Execute(null);
            return true;
        }
        private void Map_MapClicked(object sender, MapClickedEventArgs e)
        {

            map.Pins.Clear();
            map.Pins.Add(new Pin() { Position = e.Position, Label = "" });

            ViewModel.UserLocation = new Location(e.Position.Latitude, e.Position.Longitude);
        }

        private void AddPinToCurrentLocation()
        {
           
            map.Pins.Clear();

            Pin pin = new Pin()
            {
                Position = new Position(ViewModel.UserLocation.Latitude, ViewModel.UserLocation.Longitude),
                Label = ""
            };

            map.Pins.Add(pin);
        }

        private void MoveCameraToCurrentLocation()
        {
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(ViewModel.UserLocation.Latitude, ViewModel.UserLocation.Longitude),
                                             Distance.FromMiles(1)));
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            ViewModel.OpenMap(e);
        }

        async void TapGestureRecognizer_Tapped_MyLocationStack(System.Object sender, System.EventArgs e)
        {
            await GetCurrentLocation();
        }
    }
}