using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Geolocator;
using Xamarin.Forms.PlatformConfiguration;
using System.Runtime.ConstrainedExecution;
using Plugin.Geolocator.Abstractions;

namespace THOMAS_CHEN_LOCATION
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            StartListening();
            
        }
        public async Task<Position> GetLocation()
        {
            var available = CrossGeolocator.Current.IsGeolocationAvailable;
            var enabled = CrossGeolocator.Current.IsGeolocationEnabled;
            Position position = null;

            try
            {
                position = await CrossGeolocator.Current.GetLastKnownLocationAsync();
                if (position != null)
                {
                    return position;
                }

                else if (!available || !enabled)
                {
                    DisplayAlert("Location Error", "Geolocator is not currently available or enabled", "OK");
                    return null;
                }

                position = await CrossGeolocator.Current.GetPositionAsync();
            }
            catch (Exception ex)
            {
                DisplayAlert("Location Error", ex.ToString(), "OK");
            }

            if (position == null)
            {
                return null;
            }

            return position;
        }

        public async Task<string> PositionConverter(Position position)
        {
            try
            {
                var addresses = await CrossGeolocator.Current.GetAddressesForPositionAsync(position);
                var address = addresses.FirstOrDefault();

                if (address == null)
                {
                    return "No address found";
                }
                else
                {
                    return $"Address: {address.Thoroughfare}, {address.Locality}, {address.CountryName}";
                }
            }
            catch (Exception ex)
            {
                return $"Unable to get address: {ex}";
            }
        }

        public async void StartListening()
        {
            if (CrossGeolocator.Current.IsListening) { return; }

            await CrossGeolocator.Current.StartListeningAsync(TimeSpan.FromSeconds(5), 10, true, new Plugin.Geolocator.Abstractions.ListenerSettings
            {
                AllowBackgroundUpdates = true,
                PauseLocationUpdatesAutomatically = true,
                ListenForSignificantChanges = true,
                DeferLocationUpdates = true,

            });

            CrossGeolocator.Current.PositionChanged += PositionChanged;
            CrossGeolocator.Current.PositionError += PositionError;
        }

        private void PositionError(object sender, PositionErrorEventArgs e)
        {
            var available = CrossGeolocator.Current.IsGeolocationAvailable;
            var enabled = CrossGeolocator.Current.IsGeolocationEnabled;

            if (!available || !enabled)
            {
                DisplayAlert("Location Error", "Geolocator is not currently available or enabled", "OK");
                return;
            }

            Debug.WriteLine(e.Error);
            DisplayAlert("Location Error", e.Error.ToString(), "OK") ;
        }

        private void PositionChanged(object sender, PositionEventArgs e)
        {
            var position = e.Position;

            Task<string> addressTask = PositionConverter(position);

            UpdateLabel(addressTask);
        }

        public async void UpdateLabel(Task<string> addressTask)
        {
            string address = await addressTask;
            locationLabel.Text = address;
        }
        
    }
}
