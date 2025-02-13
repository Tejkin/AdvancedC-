using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Thomas_Chen_Victorian_Money_Tracker
{
    public sealed partial class MainPage : Page
    {
        private int pounds = 0;
        private int crowns = 0;
        private int shillings = 0;
        private int pence = 0;
        private int farthings = 0;

        public MainPage()
        {
            InitializeComponent();
            UpdateTotalWorth();
        }

        private void UpdateTotalWorth()
        {
            int totalPence = (pounds * 240) + (crowns * 60) + (shillings * 12) + pence + (farthings / 4);
            double totalPounds = totalPence / 240.0;
            TotalWorthTextBlock.Text = $"Total Worth: £{totalPounds:F2}";


            // Display individual coin amounts
            PoundsAmountTextBlock.Text = GetMoneyString(pounds, "pound", "pounds");
            CrownsAmountTextBlock.Text = GetMoneyString(crowns, "crown", "crowns");
            ShillingsAmountTextBlock.Text = GetMoneyString(shillings, "shilling", "shillings");
            PenceAmountTextBlock.Text = GetMoneyString(pence, "penny", "pence");
            FarthingsAmountTextBlock.Text = GetMoneyString(farthings, "farthing", "farthings");

            UpdateButtons();
        }



        private void IncreaseAmount(ref int amount)
        {
            amount++;
            UpdateTotalWorth();
        }

        private void DecreaseAmount(ref int amount)
        {
            if (amount > 0)
            {
                amount--;
                UpdateTotalWorth();
            }
        }

        private void ConvertToHigher(ref int lowerAmount, ref int higherAmount, int conversionRate)
        {
            if (lowerAmount >= conversionRate)
            {
                lowerAmount -= conversionRate;
                higherAmount++;
                UpdateTotalWorth();
            }
        }

        private void ConvertToLower(ref int higherAmount, ref int lowerAmount, int conversionRate)
        {
            if (higherAmount > 0)
            {
                higherAmount--;
                lowerAmount += conversionRate;
                UpdateTotalWorth();
            }
        }


        public void PoundsIncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            IncreaseAmount(ref pounds);
        }

        public void PoundsDecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            DecreaseAmount(ref pounds);
        }

        public void CrownsIncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            IncreaseAmount(ref crowns);
        }

        public void CrownsDecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            DecreaseAmount(ref crowns);
        }

        public void ShillingsIncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            IncreaseAmount(ref shillings);
        }

        public void ShillingsDecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            DecreaseAmount(ref shillings);
        }

        public void PenceIncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            IncreaseAmount(ref pence);
        }

        public void PenceDecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            DecreaseAmount(ref pence);
        }

        public void FarthingsIncreaseButton_Click(object sender, RoutedEventArgs e)
        {
            IncreaseAmount(ref farthings);
        }

        public void FarthingsDecreaseButton_Click(object sender, RoutedEventArgs e)
        {
            DecreaseAmount(ref farthings);
        }
        public void PoundsUpgradeButton_Click(object sender, RoutedEventArgs e)
        {
            return;
        }

        public void PoundsDowngradeButton_Click(object sender, RoutedEventArgs e)
        {
            ConvertToLower(ref pounds, ref crowns, 4);
        }

        public void CrownsUpgradeButton_Click(object sender, RoutedEventArgs e)
        {
            ConvertToHigher(ref crowns, ref pounds, 4);
        }

        public void CrownsDowngradeButton_Click(object sender, RoutedEventArgs e)
        {
            ConvertToLower(ref crowns, ref shillings, 5);
        }

        public void ShillingsUpgradeButton_Click(object sender, RoutedEventArgs e)
        {
            ConvertToHigher(ref shillings, ref crowns, 5);
        }

        public void ShillingsDowngradeButton_Click(object sender, RoutedEventArgs e)
        {
            ConvertToLower(ref shillings, ref pence, 12);
        }

        public void PenceUpgradeButton_Click(object sender, RoutedEventArgs e)
        {
            ConvertToHigher(ref pence, ref shillings, 12);
        }

        public void PenceDowngradeButton_Click(object sender, RoutedEventArgs e)
        {
            ConvertToLower(ref pence, ref farthings, 4);
        }

        public void FarthingsUpgradeButton_Click(object sender, RoutedEventArgs e)
        {
            ConvertToHigher(ref farthings, ref pence, 4);
        }

        public void FarthingsDowngradeButton_Click(object sender, RoutedEventArgs e)
        {
            return;
        }

        private void ButtonDown_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Button button = sender as Button;
            Image image = button.Content as Image;

            if (image != null)
            {
                string imageName = button.IsEnabled ? "Assets/downArrowEnabled.png" : "Assets/downArrowDisabled.png";
                image.Source = new BitmapImage(new Uri("ms-appx:///" + imageName));
            }
        }

        private void ButtonUp_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Button button = sender as Button;
            Image image = button.Content as Image;

            if (image != null)
            {
                string imageName = button.IsEnabled ? "Assets/upArrowEnabled.png" : "Assets/upArrowDisabled.png";
                image.Source = new BitmapImage(new Uri("ms-appx:///" + imageName));
            }
        }


        private void UpdateButtons()
        {
            // Pounds
            PoundsIncreaseButton.IsEnabled = true;
            PoundsDecreaseButton.IsEnabled = pounds > 0;
            PoundsDowngradeButton.IsEnabled = pounds > 0;
            PoundsUpgradeButton.IsEnabled = false; // No conversion up from pounds

            // Crowns
            CrownsIncreaseButton.IsEnabled = true;
            CrownsDecreaseButton.IsEnabled = crowns > 0;
            CrownsDowngradeButton.IsEnabled = crowns > 0;
            CrownsUpgradeButton.IsEnabled = crowns >= 4;

            // Shillings
            ShillingsIncreaseButton.IsEnabled = true;
            ShillingsDecreaseButton.IsEnabled = shillings > 0;
            ShillingsDowngradeButton.IsEnabled = shillings > 0;
            ShillingsUpgradeButton.IsEnabled = shillings >= 5;

            // Pence
            PenceIncreaseButton.IsEnabled = true;
            PenceDecreaseButton.IsEnabled = pence > 0;
            PenceDowngradeButton.IsEnabled = pence > 0;
            PenceUpgradeButton.IsEnabled = pence >= 12;

            // Farthings
            FarthingsIncreaseButton.IsEnabled = true;
            FarthingsDecreaseButton.IsEnabled = farthings > 0;
            FarthingsDowngradeButton.IsEnabled = false; // No conversion down from farthings
            FarthingsUpgradeButton.IsEnabled = farthings >= 4;
        }

        private string GetMoneyString(int amount, string singularLabel, string pluralLabel)
        {
            if (amount == 1)
                return $"1{Environment.NewLine}{singularLabel}";
            else
                return $"{amount}{Environment.NewLine}{pluralLabel}";
        }

    }
}
