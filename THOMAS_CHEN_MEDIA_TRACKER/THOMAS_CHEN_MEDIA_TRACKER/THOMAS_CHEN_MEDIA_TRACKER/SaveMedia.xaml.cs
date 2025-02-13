using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.IO;
using Xamarin.Essentials;
using SQLitePCL;

namespace THOMAS_CHEN_MEDIA_TRACKER
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class Media
    {
        [PrimaryKey, AutoIncrement] public int ID { get; set; }
        public String name { get; set; }
        public DateTime releaseDate { get; set; }
        public bool released { get; set; }
        public bool finished { get; set; }

        public void Finished()
        {
            finished = true;
        }

        public void Edit(string name, DateTime releaseDate, bool released, bool finished)
        {
            this.name = name;
            this.releaseDate = releaseDate;
            this.released = released;
            this.finished = finished;
        }

    }

    public class Movie : Media
    {
        public int duration { get; set; }
        public String director { get; set; }

        public void SetDirector(String director)
        {
            this.director = director;
        }

        public void SetDuration(int duration)
        {
            this.duration = duration;
        }
    }

    public class Book : Media
    {
        public int pages { get; set; }
        public String author { get; set; }

        public void SetAuthor(String author)
        {
            this.author = author;
        }
        public void SetPages(int pages)
        {
            this.pages = pages;
        }
    }

    public class Series : Media
    {
        public String director { get; set; }
        public int season { get; set; }
        public int episodes { get; set; }
        public bool finaleAired { get; set; }

        public void SetDirector(String director)
        {
            this.director = director;
        }

        public void SetSeason(int season)
        {
            this.season = season;
        }

        public void SetEpisodes(int episodes)
        {
            this.episodes = episodes;
        }

        public void isFinaleAired(bool aired)
        {
            this.finaleAired = aired;
        }
    }
    public partial class SaveMedia : ContentPage
    {
        SQLiteAsyncConnection database;
        string filename = "mediasql.sql";

        public Movie movie;
        public Book book;
        public Series series;
        public SaveMedia()
        {
            InitializeComponent();
        }
        public void OpenDataBase(string mediaType)
        {
            var localDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var fullFilePath = Path.Combine(localDataPath, filename);
            Debug.WriteLine(fullFilePath);

            database = new SQLiteAsyncConnection(fullFilePath);

            if (mediaType == "movie")
            {
                database.CreateTableAsync<Movie>().Wait();
                database.InsertAsync(movie);
            }
            else if (mediaType == "book")
            {
                database.CreateTableAsync<Book>().Wait();
                database.InsertAsync(book);
            }
            else if (mediaType == "series")
            {
                database.CreateTableAsync<Series>().Wait();
                database.InsertAsync(series);
            }
            else
            {
                DisplayAlert("SQL error", "Invalid media type", "OK");
                return;
            }
        }

        private void saveBtn_Clicked(object sender, EventArgs e)
        {
            int selectedIndex = mediaPicker.SelectedIndex; //0 - Movie, 1 - Book , 2 - TV Series
            

            if (selectedIndex == 0) // Movie
            {
                movie = new Movie();
                movie.name = nameEntry.Text;
                movie.director = directorEntry.Text;
                movie.releaseDate = releaseDatePicker.Date;
                if (int.TryParse(durationEntry.Text, out int duration))
                {
                    movie.duration = duration;
                }
                else
                {
                    DisplayAlert("Duration Entry Error", "Please put in a valid number in minutes", "OK");
                }
                movie.finished = finishedCheck.IsChecked;

                OpenDataBase("movie");
            }

            else if (selectedIndex == 1) // Book
            {
                book = new Book();
                book.name = nameEntry.Text;
                book.author = directorEntry.Text;
                book.releaseDate = releaseDatePicker.Date;
                if (int.TryParse(durationEntry.Text, out int duration))
                {
                    book.pages = duration;
                }
                else
                {
                    DisplayAlert("Duration Entry Error", "Please put in a valid number in pages", "OK");
                    return;
                }
                book.finished = finishedCheck.IsChecked;

                OpenDataBase("book");
            }
            else if (selectedIndex == 2) // Book
            {
                series = new Series();
                series.name = nameEntry.Text;
                series.director = directorEntry.Text;
                series.releaseDate = releaseDatePicker.Date;
                if (int.TryParse(durationEntry.Text, out int duration))
                {
                    series.episodes = duration;
                }
                else
                {
                    DisplayAlert("Duration Entry Error", "Please put in a valid number in Episodes", "OK");
                }
                series.finished = finishedCheck.IsChecked;

                OpenDataBase("series");
            }

            Navigation.PopModalAsync();
        }

        private void mediaPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            int selectedIndex = mediaPicker.SelectedIndex; //0 - Movie, 1 - Book , 2 - TV Series

            if (selectedIndex == 0) // Movie
            {
                directorLabel.Text = "Director";
                durationLabel.Text = "Duration (minutes)"; 
            }
            else if (selectedIndex == 1) // Movie
            {
                directorLabel.Text = "Author";
                durationLabel.Text = "Pages";
            }
            else if (selectedIndex == 2) // Movie
            {
                directorLabel.Text = "Director";
                durationLabel.Text = "Episodes";
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            mediaPicker.SelectedIndex = 0;

        }
    }
}
