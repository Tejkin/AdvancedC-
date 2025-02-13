using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Thomas_Chen_Task_Manager_Part1
{
    class StaticListTest
    {
        public static void Test()
        {
            // Create tasks
            Task task1 = new Task("Task 1");
            Task task2 = new Task("Task 2");
            Task task3 = new Task("Task 3");

            // Add tasks to the static list
            Task.AddTask(task1);
            Task.AddTask(task2);
            Task.AddTask(task3);

            // Create folders
            Folder folder1 = new Folder("Folder 1");
            Folder folder2 = new Folder("Folder 2");

            // Add folders to the static list
            Folder.AddFolder(folder1);
            Folder.AddFolder(folder2);

            // Test output of tasks and folders
            Debug.WriteLine("Tasks:");
            foreach (Task task in Task.allTasks)
            {
                Debug.WriteLine($"ID: {task.ID}, Description: {task.Description}");
            }

            Debug.WriteLine("\nFolders:");
            foreach (Folder folder in Folder.allFolders)
            {
                Debug.WriteLine($"ID: {folder.ID}, Name: {folder.Name}");
            }

            // Remove a task
            Task.RemoveTask(task2.ID);

            // Remove a folder
            Folder.RemoveFolder(folder1.ID);

            // Test after removal
            Debug.WriteLine("\nAfter removal:");
            Debug.WriteLine("Tasks:");
            foreach (Task task in Task.allTasks)
            {
                Debug.WriteLine($"ID: {task.ID}, Description: {task.Description}");
            }

            Debug.WriteLine("\nFolders:");
            foreach (Folder folder in Folder.allFolders)
            {
                Debug.WriteLine($"ID: {folder.ID}, Name: {folder.Name}");
            }
        }
    }
}
