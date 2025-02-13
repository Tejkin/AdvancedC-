using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thomas_Chen_Task_Manager
{
    public class NaturalLanguageParser
    {
        private readonly NaturalLanguageDateParser dateParser = new NaturalLanguageDateParser();
        private readonly NaturalLanguageTimeParser timeParser = new NaturalLanguageTimeParser();

        public (string Task, DateTime? Date, TimeSpan? Time) ParseTask(string taskDescription)
        {
            string task = taskDescription;

            DateTime? date = null;
            TimeSpan? time = null;

            // new list to hold possible patterns for date and time
            string[] datePatterns = new[] { "today", "tomorrow", "next", "this", "on" };
            string[] timeIndicators = new[] { "am", "pm", "in the morning", "in the afternoon", "at night" };

            // Remove all commas
            int commaIndex = 1;
            while (commaIndex > 0)
            {
                commaIndex = taskDescription.IndexOf(",");
                if(commaIndex != -1) 
                {
                    taskDescription = taskDescription.Remove(commaIndex, 1);
                }
            }

            foreach (var datePattern in datePatterns)
            {
                int dateStartIndex = taskDescription.ToLower().IndexOf(datePattern.ToLower());
                int dateEndIndex;
                if (dateStartIndex != -1)
                {
                    string potentialDate = taskDescription.Substring(dateStartIndex);
                    if (datePattern == "this" || datePattern == "next" || datePattern == "on")
                    {
                        dateEndIndex = potentialDate.IndexOf("y"); // The end of the days would usually end with day if mentioning week days
                        potentialDate = potentialDate.Substring(0, dateEndIndex + 1);
                    }
                    else if (datePattern == "tomorrow")
                    {
                        dateEndIndex = potentialDate.IndexOf("w"); // Adding tomorrow to the case
                        potentialDate = potentialDate.Substring(0, dateEndIndex + 1);
                    }
                    
                    try
                    {
                        date = dateParser.ParseDate(potentialDate);
                        taskDescription = taskDescription.Remove(dateStartIndex, potentialDate.Length).Trim();
                        break;
                    }
                    catch { }
                }
            }
            NaturalLanguageDateParser daysOfWeek = new NaturalLanguageDateParser();
            foreach (var weekDay in daysOfWeek.daysOfWeekMap) // Catch dates where they dont confine to the natural date patterns
            {
                if (taskDescription.ToLower().Contains(weekDay.Key.ToLower()))
                {
                    int dateStartIndex = taskDescription.ToLower().IndexOf(weekDay.Key.ToLower());
                    string potentialDate = taskDescription.Substring(dateStartIndex);
                    int dateEndIndex = potentialDate.IndexOf("y");
                    potentialDate = potentialDate.Substring(0, dateEndIndex + 1);

                    try
                    {
                        date = dateParser.ParseDate(potentialDate);
                        taskDescription = taskDescription.Remove(dateStartIndex, potentialDate.Length).Trim();
                        break;
                    }
                    catch { }
                }
            }
            foreach (var timeIndicator in timeIndicators)
            {
                
                int timeIndex = taskDescription.ToLower().IndexOf(timeIndicator.ToLower());
               
                
                if (timeIndex != -1)
                {
                    // Remove common "at" string 
                    int atIndex = taskDescription.LastIndexOf("at", timeIndex - timeIndicator.Length);
                    if (atIndex != -1)
                    {
                        taskDescription = taskDescription.Remove(atIndex, 2).Trim();
                        timeIndex = taskDescription.ToLower().IndexOf(timeIndicator.ToLower()); // Reset time index
                    }


                    int startIndex = taskDescription.LastIndexOf(' ', timeIndex - timeIndicator.Length); // to ignore the first space that is next to the time indicator
                    
                    
                    if (startIndex == -1) startIndex = 0; // if no space found, start from the beginning

                    string potentialTime = taskDescription.Substring(startIndex, timeIndex + timeIndicator.Length - startIndex).Trim();
                    try
                    {   
                        time = timeParser.ParseTime(potentialTime);
                        taskDescription = taskDescription.Remove(startIndex, potentialTime.Length + 1).Trim();
                        break;
                    }
                    catch
                    {
                        Debug.WriteLine("Inocrrect time format");
                    }

                }
            }

            return (taskDescription, date, time);
        }
    }

    

}
