using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thomas_Chen_Task_Manager_Part1
{
    public class Habit : RepeatingTask
    {
        public int CurrentStreak { get; private set; }
        public int LongestStreak { get; private set; }

        public Habit(string description, RepeatType repeatFrequency, string notes = "") : base(description, repeatFrequency, notes)
        {
            CurrentStreak = 0;
            LongestStreak = 0;
        }

        public new void CompleteTask()
        {
            if (!IsCompleted)
            {
                IsCompleted = true;
                CompletionDate = DateTime.Today;

                // Increment streaks
                CurrentStreak++;
                if (CurrentStreak > LongestStreak)
                {
                    LongestStreak = CurrentStreak;
                }
            }
            else if (DateTime.Today > CompletionDate) // If task is completed before completion date
            {
                CurrentStreak++;
                if (CurrentStreak > LongestStreak)
                {
                    LongestStreak = CurrentStreak;
                }
            }
            else
            {
                CurrentStreak = 0;
            }
        }
    }

}
