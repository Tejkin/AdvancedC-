using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thomas_Chen_Task_Manager_Part1
{
    public class Folder
    {
        public Guid ID { get; }
        public string Name { get; set; }
        private List<Guid> taskIDs;

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
        public static void RemoveFolder(Guid folderID)
        {
            allFolders.RemoveAll(folder => folder.ID == folderID);
        }

        // Constructor
        public Folder(string name)
        {
            ID = Guid.NewGuid();
            Name = name;
            taskIDs = new List<Guid>();
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
