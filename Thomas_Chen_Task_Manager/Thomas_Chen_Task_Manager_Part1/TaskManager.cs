using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
namespace Thomas_Chen_Task_Manager
{
    public class TaskManager
    {
        // perform binary search on tasks based on completion date
        public static List<int> SearchTasksByDueDate(DateTime targetDate)
        {
            // Sort the tasks
            Task.allTasks.Sort();

            // Create a list to store the indices of tasks with the target due date
            List<int> indices = new List<int>();

            // Iterate over the tasks
            for (int i = 0; i < Task.allTasks.Count; i++)
            {
                // If the task's due date is the target date, add its index to the list
                if (Task.allTasks[i].dueDate.HasValue && Task.allTasks[i].dueDate.Value.Date == targetDate.Date)
                {
                    indices.Add(i);
                }
            }

            // Return the list of indices
            return indices;
        }

        // perform arbitrary search on tasks based on description (name)
        public static void SortTaskByIndex()
        {
            Task.allTasks.Sort(Task.CompareTasksByDescription);
        }

        public static Task FindTaskByDescription(string description)
        {
            return Task.allTasksByDescription.Find(task => task.description == description);
        }

        public static void SearchTaskTest()
        {
            DateTime targetDate = new DateTime(2024, 5, 17);

            // create new task to be due on May 16th 2024
            Task searchTask = new Task("Search Task", "Test", DateTime.Now.AddDays(3));
            Task.AddTask(searchTask);

            // create new task to have same dates but different description
            Task sortTask1 = new Task("Sort Task 1", "Test", DateTime.Now.AddDays(-2));
            Task sortTask2 = new Task("Sort Task 2", "Test", targetDate);
            Task sortTask3 = new Task("A Sort Task", "Test", targetDate);
            Task.AddTask(sortTask1);
            Task.AddTask(sortTask2);
            Task.AddTask(sortTask3);

            // search for tasks due on May 16th 2024
            var indicies = SearchTasksByDueDate(targetDate);
            bool taskFound = false;

            foreach (int index in indicies)
            {
                if (index >= 0)
                {
                    Debug.WriteLine($"Tasks due on {targetDate.ToShortDateString()} found at index {index}.");

                    Task foundTask = Task.allTasks[index];
                    Debug.WriteLine($"Task: {foundTask.description}");

                }
                taskFound = true;
            }
            
            if (!taskFound)
            {
                Debug.WriteLine($"No tasks due on {targetDate.ToShortDateString()} found.");
            }

            // search for tasks via description

            Debug.WriteLine($"Find task by description: Search Task... {FindTaskByDescription("Search Task").id}");
        }
    }
}
