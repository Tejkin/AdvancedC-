using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using Windows.Storage;
using Newtonsoft.Json;
using System.Threading;

namespace Thomas_Chen_Task_Manager
{
    class TasksAndFoldersStaticList
    {
        private static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
        public async void SaveTasksAndFolders()
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                StorageFile file;
                file = await storageFolder.CreateFileAsync("myData.json", CreationCollisionOption.ReplaceExisting);


                Debug.WriteLine($"File saved to {file.Path}");

                using (var stream = await file.OpenStreamForWriteAsync())
                {
                    using (var writer = new StreamWriter(stream))
                    {
                        var allData = new
                        {
                            Tasks = Task.allTasks,
                            Folders = Folder.allFolders
                        };

                        var settings = new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            Formatting = Formatting.Indented
                        };


                        string json = JsonConvert.SerializeObject(allData, settings);
                        await writer.WriteAsync(json);
                    }
                }
            }
            finally
            {
                semaphoreSlim.Release();
            }
            
        }

        public async void LoadTasksAndFolders()
        {
            await semaphoreSlim.WaitAsync();
            try
            {
                StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
                try
                {
                    StorageFile file = await storageFolder.GetFileAsync("myData.json");
                    using (var stream = await file.OpenStreamForReadAsync())
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            string json = await reader.ReadToEndAsync();
                            var allData = JsonConvert.DeserializeObject<dynamic>(json);

                            // Clear existing tasks and folders
                            Task.allTasks.Clear();
                            Folder.allFolders.Clear();

                            // Load tasks
                            foreach (var task in allData.Tasks)
                            {
                                Task.allTasks.Add(task.ToObject<Task>());
                            }

                            // Load folders
                            foreach (var folder in allData.Folders)
                            {
                                Folder.allFolders.Add(folder.ToObject<Folder>());
                            }
                        }
                    }
                }
                catch (FileNotFoundException)
                {
                    Debug.WriteLine("File 'myData.json' does not exist.");
                }
            }
            finally { semaphoreSlim.Release(); };
            
            
        }
    }
}
