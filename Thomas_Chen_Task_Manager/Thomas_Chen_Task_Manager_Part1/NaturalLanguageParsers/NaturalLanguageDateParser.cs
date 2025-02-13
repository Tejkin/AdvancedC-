using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Lights;
using System.Globalization;

namespace Thomas_Chen_Task_Manager
{
    public class NaturalLanguageDateParser
    {
        public readonly Dictionary<string, DayOfWeek> daysOfWeekMap = new Dictionary<string, DayOfWeek>()
        {
            { "sunday", DayOfWeek.Sunday },
            { "monday", DayOfWeek.Monday },
            { "tuesday", DayOfWeek.Tuesday },
            { "wednesday", DayOfWeek.Wednesday },
            { "thursday", DayOfWeek.Thursday },
            { "friday", DayOfWeek.Friday },
            { "saturday", DayOfWeek.Saturday }
        };
        public DateTime ParseDate(string naturalLanguageDate)
        {
            naturalLanguageDate = naturalLanguageDate.ToLower();
            DateTime now = DateTime.Now;

            if (naturalLanguageDate.Contains("tomorrow")) // If code detects the input contains "tomorrow", the program automatically expects the task to be set one day from now
            {
                return now.AddDays(1).Date;
            }
            else if (naturalLanguageDate.Contains("today")) // For the british: if fortnight is detected, the program automaticaly expects the task to be set 2 weeks from now
            {
                return now;
            }
            else if (naturalLanguageDate.Contains("fortnight")) // For the british: if fortnight is detected, the program automaticaly expects the task to be set 2 weeks from now
            {
                return now.AddDays(14).Date;
            }

            foreach (var day in daysOfWeekMap)
            {
                if (naturalLanguageDate.Contains(day.Key))
                {
                    bool containsNext = naturalLanguageDate.Contains("next");
                    bool containsThis = naturalLanguageDate.Contains("this");

                    int currentDayOfWeek = (int)now.DayOfWeek;
                    int targetDayOfWeek = (int)day.Value;

                    int daysDifference = targetDayOfWeek - currentDayOfWeek;

                    if (containsNext || (!containsThis && daysDifference == 0)) // If input contains "next", the date is expected to be the week after.
                    {
                        daysDifference += 7;
                    }

                    if (daysDifference < 0) // If input day of the week and target day of the week is the same, program expects the user intends next week to have the task.
                    {
                        daysDifference += 7;
                    }

                    return now.AddDays(daysDifference).Date;
                }
            }
            return now.Date;
            throw new ArgumentException("Date format or no input error... Applied today's date");
        }
    }
}
