using Plugin.Geolocator;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace THOMAS_CHEN_EMERGENCY_LOCATION
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new MainPage();

            GetLocation();
        }
        public async void GetLocation()
        {
            var available = CrossGeolocator.Current.IsGeolocationAvailable;

            var location = await CrossGeolocator.Current.GetLastKnownLocationAsync();

            Debug.WriteLine(available.ToString(), location.ToString());
        }
        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
