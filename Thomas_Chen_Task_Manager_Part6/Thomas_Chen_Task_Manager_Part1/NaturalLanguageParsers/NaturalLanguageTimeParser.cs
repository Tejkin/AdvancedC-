using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Lights;
using System.Globalization;

namespace Thomas_Chen_Task_Manager
{
    public class NaturalLanguageTimeParser
    {
        private readonly Dictionary<string, int> NumericLanguageMap = new Dictionary<string, int>()
        {
            {"one", 1},
            {"two", 2}, 
            {"three", 3},
            {"four", 4},
            {"five", 5},
            {"six", 6},
            {"seven", 7},
            {"eight", 8},
            {"nine", 9},
            {"ten", 10},
            {"eleven", 11},
            {"twelve", 12}
        };
        public TimeSpan ParseTime(string naturalLanguageTime)
        {
            naturalLanguageTime = naturalLanguageTime.ToLower().Trim();

            int hour = 0;
            int minute = 0;
            bool isPm = false;
            bool isAm = false;

            // Check if the string ends with "am" or "pm"
            if (naturalLanguageTime.EndsWith("pm"))
            {
                isPm = true;
                naturalLanguageTime = naturalLanguageTime.Replace("pm", "").Trim();
            }
            else if (naturalLanguageTime.EndsWith("am"))
            {
                isAm = true;
                naturalLanguageTime = naturalLanguageTime.Replace("am", "").Trim();
            }
            else if (naturalLanguageTime.Contains("in the afternoon") || naturalLanguageTime.Contains("in the evening"))
            {
                isPm = true;
                naturalLanguageTime = naturalLanguageTime.Replace("in the afternoon", "").Replace("in the evening", "").Trim();
            }
            else if (naturalLanguageTime.Contains("in the morning"))
            {
                isAm = true;
                naturalLanguageTime = naturalLanguageTime.Replace("in the morning", "").Trim();
            }


            // Split the input string by spaces, : and , into a list
            string[] parts = naturalLanguageTime.Split(new[] { ' ', ':', '.' }, StringSplitOptions.RemoveEmptyEntries);

            // Check for hour and minute from input text
            if (parts.Length > 0 && int.TryParse(parts[0], out int parsedHour))
            {
                hour = parsedHour;
            }

            if (parts.Length > 1 && int.TryParse(parts[1], out int parsedMinute))
            {
                minute = parsedMinute;
            }

            else
            {
                foreach (string numericLanguage in NumericLanguageMap.Keys)
                {
                    if (naturalLanguageTime.Contains(numericLanguage))
                    {
                        hour = NumericLanguageMap[numericLanguage];
                        break;
                    }
                }
            }

            // If am or pm is not specified attempt to make a reasonable guess. If time given is outside of 1 - 6, expecting the user to not have tasks between 1-6am
            if (!isPm && !isAm)
            {
                if (hour >= 1 && hour <= 6)
                {
                    isAm = true;
                }
                else
                {
                    isPm = true;
                }
            }

            // Adjust hour for PM times
            if (isPm && hour < 12)
            {
                hour += 12;
            }
            else if (isAm && hour == 12)
            {
                hour = 0;
            }

            return new TimeSpan(hour, minute, 0);
        }
    }
}
