using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PCLStorage;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections;
using System.Security.Cryptography.X509Certificates;

namespace Thomas_Chen_Quote_App
{
    public class FamousQuote
    {
        public string quote;
        public string author;
        public bool favourite;

        public FamousQuote(string quote, string author) 
        {
            this.quote = quote;
            this.author = author;
            this.favourite = false;
        }
    }
    public partial class MainPage : ContentPage
    {
        String folderName = "quotesFolder";
        String fileName = "quotesFile.txt";
        List<FamousQuote> quotesList = new List<FamousQuote>();
       
        public int randomIndex;
        public MainPage()
        {
            InitializeComponent();

            loadQuote();

        }
        public async void saveQuote(String quote)
        {
            IFolder folder = FileSystem.Current.LocalStorage;
            folder = await folder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);

            IFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            await file.WriteAllTextAsync(quote);
            Debug.WriteLine(folder.Path);
        }

        public async void loadQuote()
        {
            IFolder folder = FileSystem.Current.LocalStorage;
            folder = await folder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);

            IFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            
            string loadedQuotes = await file.ReadAllTextAsync();

            var tempList = JsonConvert.DeserializeObject<List<FamousQuote>>(loadedQuotes);
            if (tempList != null ) {
                quotesList = tempList;
            }
        }

        public string randomQuote()
        {
            //Randomizes number and refers number in position of quotes list
            Random rnd = new Random();
            int randomInt = rnd.Next(0, quotesList.Count);
            randomIndex = randomInt;
            try
            {
                if (quotesList[randomInt].favourite == false) //displays appropriate image button depending on boolean quotesList.favourite
                {
                    unfavouriteButton.IsVisible = true;
                    favouriteButton.IsVisible = false;
                }
                if (quotesList[randomInt].favourite == true)
                {
                    favouriteButton.IsVisible = true;
                    unfavouriteButton.IsVisible= false;
                }
                
                return "\"" + quotesList[randomInt].quote + "\"" + " by " + quotesList[randomInt].author;
            }
            catch(ArgumentOutOfRangeException)
            {
                Debug.WriteLine("There are no quotes in the saved file");
                return "There are no quotes saved";
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        private void addButton_Clicked(object sender, EventArgs e)
        {
            if (quoteEntry.Text == "" || authorEntry.Text == "")
            {
                Debug.WriteLine("Entry cannot be empty");
            }
            else
            {
                quotesList.Add(new FamousQuote(quoteEntry.Text, authorEntry.Text));

                String jsonString = JsonConvert.SerializeObject(quotesList);
                Debug.WriteLine(jsonString);
                saveQuote(jsonString);

                quoteEntry.Text = "";
                authorEntry.Text = "";
            }
        }

        private void randomizeButton_Clicked(object sender, EventArgs e)
        {
            displayQuote.Text = randomQuote();
        }

        private async void showFavouriteButton_Clicked(object sender, EventArgs e)
        {
            FavouritesPage favouritePage = new FavouritesPage();

            favouritePage.passedData = quotesList;


            await Navigation.PushModalAsync(favouritePage);
        }

        private void favouriteButton_Clicked(object sender, EventArgs e)
        {
            favouriteButton.IsVisible = false;
            unfavouriteButton.IsVisible = true;
            quotesList[randomIndex].favourite = false;

            String jsonString = JsonConvert.SerializeObject(quotesList); //saves changes made to favourite
            saveQuote(jsonString);
        }

        private void unfavouriteButton_Clicked(object sender, EventArgs e)
        {
            unfavouriteButton.IsVisible = false;
            favouriteButton.IsVisible = true;
            quotesList[randomIndex].favourite = true;

            String jsonString = JsonConvert.SerializeObject(quotesList); //saves changed made to favourite
            saveQuote(jsonString);
        }
    }
}

    
