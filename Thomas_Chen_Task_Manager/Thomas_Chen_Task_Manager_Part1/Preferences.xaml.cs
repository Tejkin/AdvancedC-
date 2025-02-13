using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Thomas_Chen_Task_Manager
{
    public sealed partial class PreferencesPage : Page
    {
        public PreferencesPage()
        {
            this.InitializeComponent();
            LoadPreferences();
        }

        private void LoadPreferences()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            var colorScheme = localSettings.Values["ColorScheme"] as string;
            var sortOrder = localSettings.Values["SortOrder"] as string;

            if (colorScheme == "Dark")
            {
                ColorSchemeComboBox.SelectedIndex = 1;
            }
            else
            {
                ColorSchemeComboBox.SelectedIndex = 0;
            }
            if (sortOrder == "Alphabetically")
            {
                SortOrderComboBox.SelectedIndex = 1;
            }
            else
            {
                SortOrderComboBox.SelectedIndex = 0;
            }
        }

        private void ColorSchemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            string selectedScheme = (ColorSchemeComboBox.SelectedItem as ComboBoxItem).Content as string;
            var sortOrder = localSettings.Values["SortOrder"] as string;

            if (selectedScheme == "Dark")
            {
                localSettings.Values["ColorScheme"] = "Dark";
                ((Frame)Window.Current.Content).RequestedTheme = ElementTheme.Dark;
            }
            else
            {
                localSettings.Values["ColorScheme"] = "Light";
                ((Frame)Window.Current.Content).RequestedTheme = ElementTheme.Light;
            }
        }
        private void SortOrderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            string selectedSortOrder = (SortOrderComboBox.SelectedItem as ComboBoxItem).Content as string;

            // Save selected sort order to local settings
            localSettings.Values["SortOrder"] = selectedSortOrder;

            // Notify that the sort order has changed
            EventAggregator.OnSortOrderChanged();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
        public static class EventAggregator
        {
            public static event Action SortOrderChangedEvent;

            public static void OnSortOrderChanged()
            {
                SortOrderChangedEvent?.Invoke();
            }
        }

        private async void SaveJsonToFile(string json)
        {
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await localFolder.CreateFileAsync("preferences.json", CreationCollisionOption.ReplaceExisting);

            using (StreamWriter writer = new StreamWriter(await file.OpenStreamForWriteAsync()))
            {
                await writer.WriteAsync(json);
            }
        }
    }
}
