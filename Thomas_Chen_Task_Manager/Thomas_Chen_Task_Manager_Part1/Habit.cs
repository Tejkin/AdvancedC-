using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thomas_Chen_Task_Manager
{
    public class Habit : RepeatingTask
    {
        public int currentStreak { get; private set; }
        public int longestStreak { get; private set; }

        public Habit(string description, RepeatType repeatFrequency, string notes = "") : base(description, repeatFrequency, notes)
        {
            currentStreak = 0;
            longestStreak = 0;
        }

        public new void CompleteTask()
        {
            if (!isCompleted)
            {
                isCompleted = true;
                dueDate = DateTime.Today;

                // Increment streaks
                currentStreak++;
                if (currentStreak > longestStreak)
                {
                    longestStreak = currentStreak;
                }
            }
            else if (DateTime.Today > dueDate) // If task is completed before completion date
            {
                currentStreak++;
                if (currentStreak > longestStreak)
                {
                    longestStreak = currentStreak;
                }
            }
            else
            {
                currentStreak = 0;
            }
        }
    }

}
