using System;
using System.Collections.Generic;
using System.Text;

namespace Tip_Calculator
{
    public class TipCalculatorDataModel
    {
        public static string CalculateTip(string str_billAmount, string str_tipPercent)
        {
            //Converts string into double, removes dollar sign bill and returns amount tipped in a string
            double dbl_tipPercent, dbl_tipAmount, dbl_billAmount;
            dbl_billAmount = double.Parse(str_billAmount.Substring(1, str_billAmount.Length-1));
            dbl_tipPercent = double.Parse(str_tipPercent.Substring(0, str_tipPercent.Length-1))/100;
            dbl_tipAmount = dbl_billAmount * dbl_tipPercent;
            string str_tipAmount = dbl_tipAmount.ToString("F2");
            return str_tipAmount;
        }
        public static string CalculateTotal(string str_billAmount, string str_tipAmount)
        {
            //Converts string into double, removes dollar sign bill and returns total amount in a string
            double dbl_totalAmount, dbl_tipAmount, dbl_billAmount;
            dbl_tipAmount = double.Parse(str_tipAmount.Substring(1, str_tipAmount.Length - 1));
            dbl_billAmount = double.Parse(str_billAmount.Substring(1, str_billAmount.Length - 1));
            dbl_totalAmount = dbl_tipAmount + dbl_billAmount;
            string str_totalAmount = dbl_totalAmount.ToString("F2");
            return str_totalAmount;
        }
        public static double CalculateCostPerDiner(double dbl_dinerAmount, string str_totalAmount)
        {
            double dbl_totalAmount;
            dbl_totalAmount = double.Parse(str_totalAmount.Substring(1, str_totalAmount.Length - 1));
            double dbl_costPerDiner = dbl_totalAmount / dbl_dinerAmount;
            return dbl_costPerDiner;
        }
    }
}

