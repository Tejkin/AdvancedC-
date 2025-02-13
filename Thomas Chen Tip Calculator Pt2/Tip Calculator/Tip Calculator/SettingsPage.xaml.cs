using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tip_Calculator
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage ()
		{
			InitializeComponent ();
		}

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            Application.Current.Properties["scheme"] = "none";
            schemePicker.SelectedIndex = -1;
            if (darkModeSwitch.IsToggled)
			{
                Application.Current.Properties["darkMode"] = true;
                DarkMode();
				return;
            }
			Application.Current.Properties["darkMode"] = false;
            DarkMode();
			
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            DarkMode();
            Sound();
            Scheme();
            Application.Current.Properties["activeScreen"] = 1;
            await Task.Delay(200);
        }

        private async void previousPage_Clicked(object sender, EventArgs e)
        {
			await Navigation.PopModalAsync();
        }

        public void ChangeBackground(Color color)
        {
            initialStack.Background = color;
        }

        public void ChangeTextColor(Color color)
        {
            darkLabel.TextColor = color;
            schemeLabel.TextColor = color;
            settingsLabel.TextColor = color;
            soundLabel.TextColor = color;
            schemePicker.TitleColor = color;
        }

        public void ChangeButtonColor(Color color)
        {
            previousPage.BackgroundColor = color;
        }

        public void DarkMode()
        {

            if (Application.Current.Properties.ContainsKey("darkMode"))
            {
                var darkMode = Application.Current.Properties["darkMode"];
                Debug.WriteLine(darkMode);
                if (darkMode is bool && (bool)darkMode)
                {
                    darkModeSwitch.IsToggled = true;
                    ChangeBackground(Color.DarkSlateGray);
                    ChangeButtonColor(Color.SlateGray);
                    ChangeTextColor(Color.White);
                }
                if (darkMode is bool && !(bool)darkMode)
                {
                    ChangeBackground(Color.White);
                    ChangeButtonColor(Color.LightGray);
                    ChangeTextColor(Color.Black);
                }
            }
        }

        private void soundSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (soundSwitch.IsToggled)
            {
                Application.Current.Properties["sound"] = true;
                return;
            }
            Application.Current.Properties["sound"] = false;
        }

        public void Sound()
        {
            if (Application.Current.Properties.ContainsKey("sound"))
            {
                var sound = Application.Current.Properties["sound"];
                if (sound is bool && (bool)sound)
                {
                    soundSwitch.IsToggled = true;
                    return;
                }
                soundSwitch.IsToggled = false;
            }
        }

        private void schemePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Application.Current.Properties["darkMode"] = false;
            darkModeSwitch.IsToggled = false;
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                if (selectedIndex == 0) //Blue
                {
                    Blue();
                }

                if (selectedIndex == 1) //Red
                {
                    Red();
                }

                if (selectedIndex == 2) //Neon
                {
                    Neon();
                }
            }
        }

        public void Scheme()
        {
            
            if (Application.Current.Properties.ContainsKey("scheme"))
            {
                var scheme = Application.Current.Properties["scheme"];
                Debug.WriteLine(scheme);
                if ((string)scheme == "blue")
                {
                    Blue();
                }
                else if ((string)scheme == "red")
                {
                    Red();
                }
                else if ((string)scheme == "neon")
                {
                    Neon();
                }
            }
        }

        public void Blue()
        {
            ChangeBackground(Color.PowderBlue);
            ChangeButtonColor(Color.SkyBlue);
            ChangeTextColor(Color.Black);
            Application.Current.Properties["scheme"] = "blue";
        }

        public void Red()
        {
            ChangeBackground(Color.Crimson);
            ChangeButtonColor(Color.DarkRed);
            ChangeTextColor(Color.White);
            Application.Current.Properties["scheme"] = "red";
        }

        public void Neon()
        {
            ChangeBackground(Color.Lime);
            ChangeButtonColor(Color.SeaGreen);
            ChangeTextColor(Color.Black);
            Application.Current.Properties["scheme"] = "neon";
        }
    }
}