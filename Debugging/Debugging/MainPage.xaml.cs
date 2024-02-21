using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Debugging
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
    }
    class DiceProbabilities
    {
        public static Dictionary<int, Double> calculateProbabilitiesForNumberOfDice(int numberOfDice)
        {
            Dictionary<int, int> rollTotalCount = new Dictionary<int, int>();
            // Creates new dictionary to hold dice record

            int min = numberOfDice; // Minimum possible total
            int max = numberOfDice * 6; // Maximum possible total

            for (int i = min; i <= max; i++) // Populate dictionary, marking possible dice rolls as keys and populating with value 0
            {
                rollTotalCount[i] = 0;
            }

            int[] rolls = new int[numberOfDice]; // Create list of size numberOfDice

            for (int i = 0; i < numberOfDice; i++) // Populating rolls with the smallest possible roll
            {
                rolls[i] = 1; 
            }

            bool allDiceAccountedFor = false; // Boolean value to check if all dice has been included in its calculation

            while (!allDiceAccountedFor)
            {
                int rollTotal = 0; // Total from rolls
                foreach (int roll in rolls) // Tallies up all rolls from the rollRecord
                {
                    rollTotal += roll;
                }

                rollTotalCount[rollTotal] += 1; // Adding a count to the rollTotalCount dictionary with the roll total as key

                int currentDice = 0; // Set current dice to the first dice of the list
                bool currentDiceIsMaxed = false; // Set boolean value to while the current dice isnt at its max roll (6)

                while (!currentDiceIsMaxed)
                {
                    rolls[currentDice] += 1; // Add 1 to the value of the current dice

                    if (rolls[currentDice] <= 6) // Check if the current dice exceeds or equals to the max dice value of 6
                    {
                        currentDiceIsMaxed = true; // Set boolean value to true so that the code can move on to the next dice
                    }

                    else
                    {
                        if (currentDice == numberOfDice - 1) // If the count of the current dice equals to the value of the total number of dice.
                        {
                            allDiceAccountedFor = true;
                            currentDiceIsMaxed = true;
                        }
                        else
                        {
                            rolls[currentDice] = 1;
                        }
                    }
                    currentDice++; // Set next dice as current dice
                }
            }

            Dictionary<int, Double> rollPossibilities = new Dictionary<int, double>(); // Creates dictionary to hold the all roll possibilities and the probability of it occuring
            Double totalPossibleDiceRolls = Math.Pow(6.0, (Double)numberOfDice); // Calculate the total for all possible dice rolls
            for (int i = min; i <= max; i++)
            {
                rollPossibilities[i] = (Double)rollTotalCount[i] / totalPossibleDiceRolls; // Calculate the chances of a certail roll occuring
            }
            return rollPossibilities;
        }
    }
}
