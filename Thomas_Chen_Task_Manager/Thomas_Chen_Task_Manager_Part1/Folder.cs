using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thomas_Chen_Task_Manager
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Folder
    {
        [JsonProperty]
        public Guid id { get; private set; }
        [JsonProperty]
        public string name { get; set; }
        [JsonProperty]
        public List<Guid> taskIDs;


        // Static list to store all folders
        public static List<Folder> allFolders = new List<Folder>();

        public int IncompleteTaskCount
        {
            get
            {
                return taskIDs.Count;
            }
        }

        // add a folder to the allfolder list
        public static void AddFolder(Folder folder)
        {
            allFolders.Add(folder);
        }

        // remove a folder from the sstatic list, given its GUID
        public static void RemoveFolder(Guid folderId)
        {
            Folder folderToRemove = allFolders.Find(folder => folder.id == folderId);
            if (folderToRemove != null)
            {
                // Remove tasks that are only in this folder
                foreach (var taskId in folderToRemove.taskIDs)
                {
                    bool isTaskInOtherFolders = allFolders.Any(folder => folder.taskIDs.Contains(taskId));
                    if (!isTaskInOtherFolders)
                    {
                        Task.RemoveTask(taskId);
                    }
                }

                // Remove the folder
                allFolders.Remove(folderToRemove);
            }
        }


        // Constructor
        public Folder(string name)
        {
            id = Guid.NewGuid();
            this.name = name;
            taskIDs = new List<Guid>();
        }

        public void SetID (Guid id)
        {
            this.id = id;
        }

        // add a task to the folder
        public void AddTask(Guid taskID)
        {
            taskIDs.Add(taskID);
        }

        // remove a task from the folder
        public void RemoveTask(Guid taskID)
        {
            taskIDs.Remove(taskID);
        }

        
    }

}
