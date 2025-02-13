using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Thomas_Chen_Task_Manager
{
    public class Folder
    {
        public Guid id { get; private set; }
        public string name { get; set; }
        public List<Guid> taskIDs;

        // Static list to store all folders
        public static List<Folder> allFolders = new List<Folder>();

        public static DataModelV2 dataModelV2 = new DataModelV2();

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
            dataModelV2.InsertFolder(folder);
        }

        // remove a folder from the sstatic list, given its GUID
        public static void RemoveFolder(Guid folderID)
        {
            allFolders.RemoveAll(folder => folder.id == folderID);
            dataModelV2.RemoveFolder(folderID);
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
            dataModelV2.InsertFolderTask(id, taskID);
        }

        // remove a task from the folder
        public void RemoveTask(Guid taskID)
        {
            taskIDs.Remove(taskID);
            dataModelV2.RemoveFolderTask(id, taskID);
        }
        public void UpdateFolder()
        {
            dataModelV2.UpdateFolder(this);
        }

    }

}
