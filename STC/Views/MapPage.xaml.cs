using STC.ViewModels;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace STC.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : ContentPage
    {
        MapPageViewModel ViewModel;
        public MapPage()
        {
            InitializeComponent();

            ViewModel = BindingContext as MapPageViewModel;
            map.MapClicked += Map_MapClicked;

            if (Device.RuntimePlatform != Device.iOS)
            {
                MyLocationStack.IsVisible = false;
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                MessagingCenter.Subscribe<MapPageViewModel>(this, "OnNavigatedToMap", OnNavigatedToMap);

                //MoveCameraToCurrentLocation();
                //AddPinToCurrentLocation();
                //await GetCurrentLocation();
            }
           
            catch (Exception ex)
            {
                // Unable to get location
            }


        }

        private void OnNavigatedToMap(MapPageViewModel obj)
        {
            MoveCameraToCurrentLocation();
            AddPinToCurrentLocation();
        }

        //protected override bool OnBackButtonPressed()
        //{

        //    return true;
        //}

        CancellationTokenSource cts;

        async Task GetCurrentLocation()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (status == PermissionStatus.Granted)
                {
                    var request = new GeolocationRequest(GeolocationAccuracy.Low, TimeSpan.FromSeconds(10));
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

            MessagingCenter.Unsubscribe<MapPageViewModel>(this, "OnNavigatedToMap");
        }

        private void Map_MapClicked(object sender, MapClickedEventArgs e)
        {
            map.Pins.Clear();
            map.Pins.Add(new Pin() { Position = e.Position, Label = "" });

            Location location = new Location(e.Position.Latitude, e.Position.Longitude);
            ViewModel.UserLocation = location;

            MessagingCenter.Send<MapPage, Location>(this, "LocationSelected", location);
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


        async void TapGestureRecognizer_Tapped_MyLocationStack(System.Object sender, System.EventArgs e)
        {
            await GetCurrentLocation();
        }
    }
}