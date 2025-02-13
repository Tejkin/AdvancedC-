using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.SimpleAudioPlayer;
using Xamarin.Forms.Xaml;

namespace Tip_Calculator
{
    public partial class MainPage : ContentPage
    {
        bool hasStarted = false;
        bool hasTypedDecimal = false;
        int numberOfDecimalDigits = 0;
        public MainPage()
        {
            InitializeComponent();


        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            DarkMode();
            Scheme();
            Application.Current.Properties["activeScreen"] = 1;
            await Task.Delay(200);
        }
        class SoundEffect
        {
            static ISimpleAudioPlayer buttonSound = null;

            public static void PlayButton()
            {
                if (Application.Current.Properties.ContainsKey("sound"))
                {
                    var sound = Application.Current.Properties["sound"];
                    if (sound is bool && (bool)sound)
                    {
                        if (buttonSound == null)
                        {
                            buttonSound = CrossSimpleAudioPlayer.Current;
                            buttonSound.Load("tap.wav");
                        }

                        buttonSound.Play();
                        Debug.WriteLine("Sound played");
                        return;
                    }
                    return;
                }
            }
        }

        private void percentageSlider_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            percentageLabel.Text = percentageSlider.Value.ToString() + "%";

            //Uses the Classes in TipCalculationDataModel to calculate tip and total amount
            tipAmountLabel.Text = "$" + TipCalculatorDataModel.CalculateTip(billAmountLabel.Text, percentageLabel.Text);

            totalAmountLabel.Text = "$" + TipCalculatorDataModel.CalculateTotal(billAmountLabel.Text, tipAmountLabel.Text);

            dinerStepper_ValueChanged(sender, e);

        }

        private void button_Clicked(object sender, EventArgs e)
        {

            Button clickedButton = (Button)sender;
            SoundEffect.PlayButton();

            //The following code are made to meet these conditions:
            //  1. If the user presses “C”, everything is reset.
            //  2.If the user starts by typing a zero, nothing happens.
            //  3.The user can only enter a decimal place once.
            //  4.There can only be two digits after the decimal place.

            if (clickedButton == cButton)
            {
                //Reset bill and conditions
                billAmountLabel.Text = "$0";
                tipAmountLabel.Text = "$0";
                totalAmountLabel.Text = "$0";
                dinerCostLabel.Text = "$0";
                dinerStepper.Value = 1;
                hasStarted = false;
                hasTypedDecimal= false;
                numberOfDecimalDigits = 0;
            }
            else if (hasStarted == false)
            {
                if(clickedButton == zeroButton)
                {
                    return;
                }
                else if (clickedButton == dotButton)
                {
                    billAmountLabel.Text = "$0.";
                    hasStarted = true;
                    hasTypedDecimal= true;
                }
                else
                {
                    billAmountLabel.Text = "$" + clickedButton.Text;
                    hasStarted = true;
                }

            }
            else if (hasStarted == true)
            {
                if (clickedButton == dotButton && hasTypedDecimal == false)
                {
                    billAmountLabel.Text = billAmountLabel.Text + dotButton.Text;
                    hasTypedDecimal = true;
                }
                else if (hasTypedDecimal == false)
                {
                    billAmountLabel.Text = billAmountLabel.Text + clickedButton.Text;
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
                            billAmountLabel.Text = billAmountLabel.Text + clickedButton.Text;
                            numberOfDecimalDigits++;
                        }
                    }
                }
            }
        }

        private void dinerStepper_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            dinerLabel.Text = dinerStepper.Value.ToString();

            dinerCostLabel.Text = "$" + TipCalculatorDataModel.CalculateCostPerDiner(dinerStepper.Value, totalAmountLabel.Text).ToString("F2");
        }

        private async void settings_Clicked(object sender, EventArgs e)
        {
            SettingsPage settings = new SettingsPage();
            await Navigation.PushModalAsync(settings);
        }

        public void changeButtonBackground(Color color)
        {
            sevenButton.BackgroundColor= color;
            eightButton.BackgroundColor= color;
            nineButton.BackgroundColor = color;
            fourButton.BackgroundColor = color;
            fiveButton.BackgroundColor = color;
            sixButton.BackgroundColor = color;
            oneButton.BackgroundColor = color;
            twoButton.BackgroundColor = color;
            threeButton.BackgroundColor = color;
            zeroButton.BackgroundColor = color;
            dotButton.BackgroundColor = color;
            cButton.BackgroundColor = color;
            dinerStepper.BackgroundColor = color;
        }
        
        public void changeText(Color color)
        {
            oneButton.TextColor= color;
            twoButton.TextColor= color;
            threeButton.TextColor= color;
            fourButton.TextColor= color;
            fiveButton.TextColor= color;
            sixButton.TextColor= color;
            sevenButton.TextColor= color;
            eightButton.TextColor= color;
            nineButton.TextColor= color;
            zeroButton.TextColor= color;
            dotButton.TextColor= color;
            cButton.TextColor= color;

            dinerLabel.TextColor = color;
            totalAmountLabel.TextColor = color;
            tipAmountLabel.TextColor = color;
            billAmountLabel.TextColor = color;
            dinerCostLabel.TextColor = color;

            billLabel.TextColor = color;
            tipLabel.TextColor = color;
            totalLabel.TextColor= color;
            tipPercentLabel.TextColor= color;
            dinerAmountLabel.TextColor= color;
            costPerDinerLabel.TextColor= color;
            percentageLabel.TextColor= color;
        }

        public void changeBackground(Color color)
        {
            initialStack.Background = color;
        }

        public void DarkMode()
        {
            if (Application.Current.Properties.ContainsKey("darkMode"))
            {
                var darkMode = Application.Current.Properties["darkMode"];
                Debug.WriteLine(darkMode);
                if (darkMode is bool && (bool)darkMode)
                {
                    changeBackground(Color.DarkSlateGray);
                    changeButtonBackground(Color.SlateGray);
                    changeText(Color.White);
                }
                if (darkMode is bool && !(bool)darkMode)
                {
                    changeBackground(Color.White);
                    changeButtonBackground(Color.LightGray);
                    changeText(Color.Black);
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
            changeBackground(Color.PowderBlue);
            changeButtonBackground(Color.SkyBlue);
            changeText(Color.Black);
        }
        public void Red()
        {
            changeBackground(Color.Crimson);
            changeButtonBackground(Color.DarkRed);
            changeText(Color.White);
        }
        public void Neon()
        {
            changeBackground(Color.Lime);
            changeButtonBackground(Color.SeaGreen);
            changeText(Color.Black);
        }
    }
}
