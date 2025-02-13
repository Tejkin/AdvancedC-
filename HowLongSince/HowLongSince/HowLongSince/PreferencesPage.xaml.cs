using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HowLongSince
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Page1 : ContentPage
    {
        public Page1()
        {
            InitializeComponent();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            DarkMode();
            if (Application.Current.Properties.ContainsKey("fontSize"))
            {
                var fontSizeObject = Application.Current.Properties["fontSize"];
                int fontSize = (int)fontSizeObject / 2;
                fontSizeStepper.Value = fontSize;
            }
            FontSize();
            
            Application.Current.Properties["activeScreen"] = 1;
            await Task.Delay(200);
        }

        private async void previousPage_Clicked(object sender, EventArgs e)
        {
            Application.Current.Properties["fontSize"] = int.Parse(fontSizeEntry.Text);
            await Navigation.PopModalAsync();
        }

        private void darkModeSwitch_Toggled(object sender, ToggledEventArgs e)
        {
            if (darkModeSwitch.IsToggled)
            {
                Application.Current.Properties["darkMode"] = true;
                DarkMode();
                return;
            }
            Application.Current.Properties["darkMode"] = false;
            DarkMode();
        }

        private void fontSizeStepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            int fontSize = (int)fontSizeStepper.Value * 2;

            ChangeFontSize(fontSize);
            fontSizeEntry.Text = fontSize.ToString();
           
        }

        public void ChangeButtonColor(Color color)
        {
            previousPage.BackgroundColor = color;
            fontSizeStepper.BackgroundColor = color;
        }

        public void ChangeTextColor(Color color)
        {
            settingsLabel.TextColor = color;
            darkLabel.TextColor = color;
            fontSizeLabel.TextColor = color;
            fontSizeEntry.TextColor = color;
        }

        public void ChangeBackground(Color color)
        {
            initialStack.BackgroundColor = color;
        }

        public void ChangeFontSize(int fontSize)
        {
            darkLabel.FontSize = fontSize;
            fontSizeLabel.FontSize = fontSize;
            fontSizeEntry.FontSize = fontSize;
            
        }

        public void FontSize()
        {
            if (Application.Current.Properties.ContainsKey("fontSize"))
            {
                var fontSizeObject = Application.Current.Properties["fontSize"];
                int fontSize = (int)fontSizeObject;
                Debug.WriteLine(fontSize.ToString());
                ChangeFontSize(fontSize);  
            }
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
    }
}