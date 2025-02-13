using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thomas_Chen_Task_Manager_Part1
{
    public enum RepeatType
    {
        Daily,
        Weekly
    }

    public class RepeatingTask : Task
    {
        public RepeatType RepeatFrequency { get; private set; }

        public RepeatingTask(string description, RepeatType repeatFrequency, string notes = "") : base(description, notes)
        {
            RepeatFrequency = repeatFrequency;
        }

        public void CompleteTask()
        {
            if (IsCompleted)
            {
                // Update the completion date based on the repeat frequency
                switch (RepeatFrequency)
                {
                    case RepeatType.Daily:
                        CompletionDate = CompletionDate.Value.AddDays(1);
                        break;
                    case RepeatType.Weekly:
                        CompletionDate = CompletionDate.Value.AddDays(7);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                IsCompleted = true;
                CompletionDate = DateTime.Today;
            }
        }

    }

}
