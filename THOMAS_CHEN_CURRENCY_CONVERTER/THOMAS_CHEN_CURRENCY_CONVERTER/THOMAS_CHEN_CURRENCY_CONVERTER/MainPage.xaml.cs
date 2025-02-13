using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;
using System.Diagnostics;

namespace THOMAS_CHEN_CURRENCY_CONVERTER
{
    public partial class MainPage : ContentPage
    {
        bool hasStarted = false;
        bool hasTypedDecimal = false;
        int numberOfDecimalDigits = 0;
        public static string exchangeRateJson;
        Dictionary<string, double> exchangeRates;
        public List<ExchangeRate> exchangeRatesList;

        public MainPage()
        {
            InitializeComponent();
            ButtonIsEnabled(false);
            GetAPI();

        }
        public class ExchangeRateResponse
        {
            public string Disclaimer { get; set; }
            public string Liscence { get; set; }
            public int Timestamp { get; set; }
            public string Base { get; set; }
            public Dictionary<string, double> Rates { get; set; }
        }

        public class ExchangeRate
        {
            public string currencyCode { get; set; }
            public string exchangeRate { get; set; }
        }

        public void PopulateExchangeRateListView()
        {
            exchangeRatesList = new List<ExchangeRate>();
            foreach (var entry in exchangeRates)
            {
                exchangeRatesList.Add(new ExchangeRate { currencyCode = entry.Key, exchangeRate = entry.Value.ToString() });
            }
            exchangeRateListView.ItemsSource = exchangeRatesList;
        }

        public async void GetAPI()
        {
            var client = new HttpClient();
            var response = await client.GetAsync("https://openexchangerates.org/api/latest.json?app_id=744c0e65d9ae400eae78bcbf1151ff54");
            //Error check for response
            if (response.StatusCode != System.Net.HttpStatusCode.OK || response.Content == null)
            {
                await DisplayAlert("Error", string.Format("Response contained status code:{0}", response.StatusCode), "OK");
                return;
            }

            var responseString = await response.Content.ReadAsStringAsync();

            exchangeRateJson = responseString;
            Debug.WriteLine("Received JSON response: " + responseString);
            DeserialiseJsonString(exchangeRateJson);
            PopulateExchangeRateListView();
            ButtonIsEnabled(true);
        }
        public void ButtonIsEnabled(bool enabled)
        {
            zeroButton.IsEnabled = enabled;
            oneButton.IsEnabled = enabled;
            twoButton.IsEnabled = enabled;
            threeButton.IsEnabled = enabled;
            fourButton.IsEnabled = enabled;
            fiveButton.IsEnabled = enabled;
            sixButton.IsEnabled = enabled;
            sevenButton.IsEnabled = enabled;
            eightButton.IsEnabled = enabled;
            nineButton.IsEnabled = enabled;
            cButton.IsEnabled = enabled;
            dotButton.IsEnabled = enabled;
        }

        public void DeserialiseJsonString(string jsonString)
        {
            ExchangeRateResponse exchangeRateResponse = JsonConvert.DeserializeObject<ExchangeRateResponse>(jsonString);
            exchangeRates = exchangeRateResponse.Rates;
        }
        private void button_Clicked(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            //The following code are made to meet these conditions:
            //  1. If the user presses “C”, everything is reset.
            //  2.If the user starts by typing a zero, nothing happens.
            //  3.The user can only enter a decimal place once.
            //  4.There can only be two digits after the decimal place.

            if (clickedButton == cButton)
            {
                //Reset bill and conditions
                audLabel.Text = "$0";
                outputLabel.Text = "$0";
                hasStarted = false;
                hasTypedDecimal = false;
                numberOfDecimalDigits = 0;

            }
            else if (hasStarted == false)
            {
                if (clickedButton == zeroButton)
                {
                    return;
                }
                else if (clickedButton == dotButton)
                {
                    audLabel.Text = "$0.";
                    hasStarted = true;
                    hasTypedDecimal = true;
                }
                else
                {
                    audLabel.Text = "$" + clickedButton.Text;
                    hasStarted = true;
                }

            }
            else if (hasStarted == true)
            {
                if (clickedButton == dotButton && hasTypedDecimal == false)
                {
                    audLabel.Text = audLabel.Text + dotButton.Text + "";
                    hasTypedDecimal = true;
                }
                else if (hasTypedDecimal == false)
                {
                    audLabel.Text = audLabel.Text + clickedButton.Text;
                }

                else if (hasTypedDecimal == true)
                {
                    if (numberOfDecimalDigits < 2)
                    {
                        if (clickedButton == dotButton)
                        {
                            return;
                        }
                        else
                        {
                            audLabel.Text = audLabel.Text + clickedButton.Text;
                            numberOfDecimalDigits++;
                        }
                    }
                }
            }
            Debug.WriteLine(audLabel.Text);
            float audInput = float.Parse(audLabel.Text, System.Globalization.NumberStyles.AllowCurrencySymbol | System.Globalization.NumberStyles.Currency);
            float usd = ConvertAUDtoUSD(audInput);
            outputLabel.Text = usd.ToString("C");

        }

        public float ConvertAUDtoUSD(float aud)
        {
            float usd;
            float exUsdToAud = 0;
            foreach (var item in exchangeRates)
            {
                if (item.Key == "AUD")
                {
                    exUsdToAud = (float)item.Value;
                }
            }
            usd = aud / exUsdToAud;
            return usd;
        }
        
        private void exchangeRateListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ExchangeRate exchangeRate = (ExchangeRate)e.Item;
            float audInput = float.Parse(audLabel.Text, System.Globalization.NumberStyles.AllowCurrencySymbol | System.Globalization.NumberStyles.Currency);

            //Convert input to USD
            float usd = ConvertAUDtoUSD(audInput);

            float exUsdToCurrency = float.Parse(exchangeRate.exchangeRate);

            float currencyOutput = usd * exUsdToCurrency;

            outputLabel.Text = currencyOutput.ToString("C");
            outputCurrencyLabel.Text = exchangeRate.currencyCode.ToString();

            
        }
    }
}
