using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.IO;
using System.Diagnostics;
using SQLite;
using Xamarin.Essentials;
using SQLitePCL;

namespace THOMAS_CHEN_MEDIA_TRACKER
{

    public partial class MainPage : ContentPage
    {
        SQLiteAsyncConnection database;
        string filename = "mediasql.sql";


        public MainPage()
        {
            InitializeComponent();
        }

        private async void addMediaBtn_Clicked(object sender, EventArgs e)
        {
            SaveMedia saveMedia = new SaveMedia();
            await Navigation.PushModalAsync(saveMedia);
        }

        private void mediaPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = mediaPicker.SelectedIndex; // 0 - Movie, 1 - Book, 2 - TV series
            var localDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var fullFilePath = Path.Combine(localDataPath, filename);
            Debug.WriteLine(fullFilePath);

            database = new SQLiteAsyncConnection(fullFilePath);

            if (selectedIndex ==  0) // Movie
            {
                try
                {
                    List<Movie> movieList = database.Table<Movie>().ToListAsync().Result;

                    mediaNameListView.ItemsSource = movieList;
                    mediaDateListView.ItemsSource = movieList;
                }
                catch
                {
                    mediaNameListView.ItemsSource = null;
                    mediaDateListView.ItemsSource = null;
                    return;
                }
                
            }
            else if (selectedIndex ==  1) // Book
            {
                try
                {
                    List<Book> bookList = database.Table<Book>().ToListAsync().Result;

                    mediaNameListView.ItemsSource = bookList;
                    mediaDateListView.ItemsSource = bookList;
                }
                catch
                {
                    mediaNameListView.ItemsSource= null;
                    mediaDateListView.ItemsSource = null;
                    return;
                }
                
            }
            else if (selectedIndex == 2) // Series
            {
                try
                {
                    List<Series> seriesList = database.Table<Series>().ToListAsync().Result;

                    mediaNameListView.ItemsSource = seriesList;
                    mediaDateListView.ItemsSource = seriesList;
                }
                
                catch
                {
                    mediaNameListView.ItemsSource = null;
                    mediaDateListView.ItemsSource = null;
                    return;
                }
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            mediaPicker.SelectedIndex = 0;

        }
    }
}
