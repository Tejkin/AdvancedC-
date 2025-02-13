using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;
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

namespace Thomas_Chen_Debugging
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            // Call the method to calculate probabilities for rolling 2 dice
            Dictionary<int, double> probabilities = DiceProbabilities.calculateProbabilitiesForNumberOfDice(2);

            // Display the results in the console
            foreach (var pair in probabilities)
            {
                Debug.WriteLine($"Total: {pair.Key}, Probability: {pair.Value}");
            }
        }
    }
    class DiceProbabilities
    {
        // Method to calculate probabilities for the total value of rolling a certain number of dice
        public static Dictionary<int, Double> calculateProbabilitiesForNumberOfDice(int numberOfDice)
        {
            // Dictionary to store the count of each possible total value
            Dictionary<int, int> rollCounts = new Dictionary<int, int>();

            // Minimum and maximum possible total values when rolling 'numberOfDice' dice
            int minTotal = numberOfDice;
            int maxTotal = numberOfDice * 6;

            // Initializing all possible total values with count 0
            for (int i = minTotal; i <= maxTotal; i++)
            {
                rollCounts[i] = 0;
            }

            // Array to store the result of each dice roll
            int[] diceResults = new int[numberOfDice];

            // Initializing all dice rolls to 1
            for (int i = 0; i < numberOfDice; i++)
            {
                diceResults[i] = 1;
            }

            // Flag to indicate if all possible combinations have been generated
            bool allCombinationsGenerated = false;

            // Loop until all combinations are generated
            while (!allCombinationsGenerated)
            {
                // Calculate the total value of the current combination
                int currentTotal = 0;
                foreach (int result in diceResults)
                {
                    currentTotal += result;
                }

                // Increment the count for the current total value
                rollCounts[currentTotal] += 1;

                int index = 0;
                bool rollNextDie = false;

                // Loop to handle carry-over when a die reaches 6
                while (!rollNextDie)
                {
                    diceResults[index] += 1;

                    // If the current die has not reached its maximum value (6), exit the loop
                    if (diceResults[index] <= 6)
                    {
                        rollNextDie = true;
                    }
                    else
                    {
                        // If the last die has reached its maximum value, exit both loops
                        if (index == numberOfDice - 1)
                        {
                            allCombinationsGenerated = true;
                            rollNextDie = true;
                        }
                        else
                        {
                            // Reset the current die to 1 and proceed to the next die
                            diceResults[index] = 1;
                        }
                    }
                    index++;
                }
            }

            // Dictionary to store the probabilities for each total value
            Dictionary<int, Double> rollProbabilities = new Dictionary<int, double>();

            // Total number of possible combinations
            Double totalCombinations = Math.Pow(6.0, (Double)numberOfDice);

            // Calculate probability for each total value
            for (int i = minTotal; i <= maxTotal; i++)
            {
                rollProbabilities[i] = (Double)rollCounts[i] / totalCombinations;
            }
            return rollProbabilities;
        }
    }

}
