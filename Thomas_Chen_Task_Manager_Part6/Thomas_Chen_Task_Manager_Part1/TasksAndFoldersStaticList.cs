using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Windows.Storage;

namespace Thomas_Chen_Task_Manager
{
    class TasksAndFoldersStaticList
    {
        public async void SaveTasksAndFolders()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile file = await storageFolder.CreateFileAsync ("myFile.bin", CreationCollisionOption.ReplaceExisting);

            Debug.WriteLine($"File saved to {file.Path}");

            using (var stream = File.Open(file.Path, FileMode.Create))
            {
                using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
                {
                    // Save number of tasks
                    writer.Write(Task.allTasks.Count);
                    // Save tasks
                    foreach (Task task in Task.allTasks)
                    {
                        writer.Write(task.id.ToString());
                        writer.Write(task.description);
                        writer.Write(task.notes);
                        writer.Write(task.isCompleted);
                        writer.Write(task.dueDate.HasValue);
                        if (task.dueDate.HasValue)
                            writer.Write(task.dueDate.Value.Ticks);
                    }

                    // Save number of folders
                    writer.Write(Folder.allFolders.Count);
                    // Save folders
                    foreach (Folder folder in Folder.allFolders)
                    {
                        writer.Write(folder.id.ToString());
                        writer.Write(folder.name);
                        writer.Write(folder.taskIDs.Count);
                        foreach (Guid taskID in folder.taskIDs)
                        {
                            writer.Write(taskID.ToString());

                        }
                    }
                }
            }
        }

        public async void LoadTasksAndFolders()
        {
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            try
            {
                StorageFile file = await storageFolder.GetFileAsync("myFile.bin");
                using (var stream = await file.OpenStreamForReadAsync())
                {
                    using (var reader = new BinaryReader(stream, Encoding.UTF8, true))
                    {
                        // Clear existing tasks and folders
                        Task.allTasks.Clear();
                        Folder.allFolders.Clear();

                        // Load tasks
                        int taskCount = reader.ReadInt32();
                        for (int i = 0; i < taskCount; i++)
                        {
                            Guid taskId = Guid.Parse(reader.ReadString());
                            string description = reader.ReadString();
                            string notes = reader.ReadString();
                            bool isCompleted = reader.ReadBoolean();
                            bool hasCompletionDate = reader.ReadBoolean();
                            DateTime? completionDate = null;
                            if (hasCompletionDate)
                            {
                                long ticks = reader.ReadInt64();
                                completionDate = new DateTime(ticks);
                            }
                            Task task = new Task(description, notes);
                            task.SetID(taskId);
                            task.isCompleted = isCompleted;
                            task.dueDate = completionDate;
                            Task.allTasks.Add(task);
                        }

                        // Load folders
                        int folderCount = reader.ReadInt32();
                        for (int i = 0; i < folderCount; i++)
                        {
                            Guid folderId = Guid.Parse(reader.ReadString());
                            string name = reader.ReadString();
                            int taskCountInFolder = reader.ReadInt32();
                            List<Guid> taskIds = new List<Guid>();
                            for (int j = 0; j < taskCountInFolder; j++)
                            {
                                taskIds.Add(Guid.Parse(reader.ReadString()));
                            }
                            Folder folder = new Folder(name);
                            folder.SetID(folderId);
                            folder.taskIDs = taskIds;
                            Folder.allFolders.Add(folder);
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("File 'myFile.bin' does not exist.");
            }
        }

        public static void Test()
        {
            var taskAndFolderList = new TasksAndFoldersStaticList();
            
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

            // Save tasks and folders
            taskAndFolderList.SaveTasksAndFolders();

            // Load tasks and folders
            taskAndFolderList.LoadTasksAndFolders();

            // Print loaded tasks
            Debug.WriteLine("Loaded Tasks:");
            foreach (Task task in Task.allTasks)
            {
                Debug.WriteLine($"ID: {task.id}, Description: {task.description}");
            }

            // Print loaded folders
            Debug.WriteLine("Loaded Folders:");
            foreach (Folder folder in Folder.allFolders)
            {
                Debug.WriteLine($"ID: {folder.id}, Name: {folder.name}");
            }
        }
    }
}
