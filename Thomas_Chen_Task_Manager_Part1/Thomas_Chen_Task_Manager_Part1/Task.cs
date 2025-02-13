using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thomas_Chen_Task_Manager_Part1
{
    public class Task
    {
        public Guid ID { get; } // A GUID that uniquely identifies the task
        public string Description { get; set; }
        public string Notes { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletionDate { get; set; } //DateTime storable as a nullable type

        public bool IsOverdue
        {
            get
            {
                if (CompletionDate.HasValue)
                {
                    return DateTime.Today > CompletionDate.Value;
                }
                return false;
            }
        }

        // Static list to store all tasks
        public static List<Task> allTasks = new List<Task>();

        public Task(string description, string notes = "")
        {
            ID = Guid.NewGuid();
            Description = description;
            Notes = notes;
            IsCompleted = false;
            CompletionDate = null;
        }

        //add a new task to the static list
        public static void AddTask(Task task)
        {
            allTasks.Add(task);
        }

        // remove a task from the static list, given its ID
        public static void RemoveTask(Guid taskID)
        {
            allTasks.RemoveAll(task => task.ID == taskID);
        }
    }

}
