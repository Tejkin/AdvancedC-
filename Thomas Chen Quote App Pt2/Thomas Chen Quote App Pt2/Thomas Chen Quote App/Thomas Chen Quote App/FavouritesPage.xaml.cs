using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Thomas_Chen_Quote_App
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FavouritesPage : ContentPage
    {

        public List<FamousQuote> passedData = new List<FamousQuote>();
        public class Favourites
        {
            public string quote { get; set; }
            public string author { get; set; }
            public bool favouriteBool { get; set; }

        }

        public ObservableCollection<Favourites> favourites;
        public FavouritesPage()
        {
            InitializeComponent(); 
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            favourites = new ObservableCollection<Favourites>();
            for (int i = 0; i < passedData.Count; i++)
            {
                if (passedData[i].favourite == true)
                {
                    favourites.Add(new Favourites { 
                        quote = passedData[i].quote,author = passedData[i].author
                    });
                }

            }

            favouriteQuotes.ItemsSource = favourites;
        }

        private async void previousPageButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void favouriteQuotes_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            Favourites favourite = favourites[e.ItemIndex];
            favourites.Remove(favourite);
            
            for (int i = 0; i < passedData.Count;i++)
            {
                if (passedData[i].quote == favourite.quote)
                {
                    passedData[i].favourite = false;
                }
            }

        }
    }
}