using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Tally_App
{
    public partial class MainPage : ContentPage
    {
       bool hasStarted = false; //boolean condition to indicate a start of an input, to avoid users starting a number with 0 or using "+" when there is no number
       List<int> tallyNumbers = new List<int>(); //List to hold numbers for total
       string numberString; //String to hold individual numbers to be combined into the intended number, eg. user presses "1" and "5", the string exists to combine those button presses into "15"
        
        public MainPage()
        {
            InitializeComponent();
            SizeChanged += MainPageSizeChanged;
        }

        void MainPageSizeChanged(object sender, EventArgs e)
        {
            bool isPortait = this.Height > this.Width;

            if (isPortait)
            {
                tallyStack.Orientation = StackOrientation.Horizontal;
            }
            else
            {
                tallyStack.Orientation = StackOrientation.Vertical;
            }
        }

        private void button_Clicked(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            if(clickedButton == clearButton) //Clears condition and displays
            {
                totalLabel.Text = "0";
                hasStarted = false;
                tallyNumbers.Clear();
                tallyNumberEditor.Text = "";
                numberString = "";
            }
            else if (clickedButton == plusButton)
            {
                if (hasStarted == true)
                {
                    hasStarted = false; //Resets boolean condition
                    tallyNumberEditor.Text = tallyNumberEditor.Text + "\n+";

                    tallyNumbers.Add(int.Parse(numberString));
                    int total = tallyNumbers.Sum(x => Convert.ToInt32(x)); //Sums all numbers in list
                    totalLabel.Text = total.ToString();
                    numberString = ""; //Resets numberString, so a new number may be created
                }
            }
            else
            {
                if (hasStarted == false && clickedButton == zeroButton)
                {
                    return;
                }
                else
                {
                    hasStarted = true;
                    numberString = numberString + clickedButton.Text;

                    tallyNumberEditor.Text = tallyNumberEditor.Text + clickedButton.Text;
                }
            }
        }
    }
}
