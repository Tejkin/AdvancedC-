using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        public static Dictionary<int, Double> calculateProbabilitiesForNumberOfDice(int n)
        {
            Dictionary<int, int> diceRecord = new Dictionary<int, int>();
            // Creates new dictionary to hold record of dice throws

            int min = n;
            int max = n * 6;

            for (int i = min; i <= max; i++)
            {
                diceRecord[i] = 0;
            }

            int[] d = new int[n];

            for (int i = 0; i < n; i++)
            {
                d[i] = 1;
            }

            bool finished1 = false;

            while (!finished1)
            {
                int total = 0;
                foreach (int r in d)
                {
                    total += r;
                }

                diceRecord[total] += 1;

                int i = 0;
                bool finished2 = false;

                while (!finished2)
                {
                    d[i] += 1;

                    if (d[i] <= 6)
                    {
                        finished2 = true;
                    }

                    else
                    {
                        if (i == n - 1)
                        {
                            finished1 = true;
                            finished2 = true;
                        }
                        else
                        {
                            d[i] = 1;
                        }
                    }
                    i++;
                }
            }

            Dictionary<int, Double> rp = new Dictionary<int, double>();
            Double total2 = Math.Pow(6.0, (Double)n);
            for (int i = min; i <= max; i++)
            {
                rp[i] = (Double)diceRecord[i] / total2;
            }
            return rp;
        }
    }
}
