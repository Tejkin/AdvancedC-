﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;

namespace Thomas_Chen_Task_Manager
{
    [JsonObject(MemberSerialization.OptIn)]
    public class Task : IComparable<Task>
    {
        [JsonProperty]
        public Guid id { get; private set; } // A GUID that uniquely identifies the task
        [JsonProperty]
        public string description { get; set; }
        [JsonProperty]
        public string notes { get; set; }
        [JsonProperty]
        public bool isCompleted { get; set; }
        [JsonProperty]
        public DateTime? dueDate { get; set; } //DateTime storable as a nullable 
        public bool isOverdue
        {
            get
            {
                if (dueDate.HasValue)
                {
                    return DateTime.Today > dueDate.Value;
                }
                return false;
            }
        }

        // Static list to store all tasks
        public static List<Task> allTasks = new List<Task>();

        // Static index list to store all tasks sorted by description
        public static List<Task> allTasksByDescription = new List<Task>();

        public Task(string description, string notes = "", DateTime? dueDate = null)
        {
            id = Guid.NewGuid();
            this.description = description;
            this.notes = notes;
            isCompleted = false;
            this.dueDate = dueDate;
        }

        public void SetID (Guid id)
        {
            this.id = id;
        }

        //add a new task to the static list
        public static void AddTask(Task task)
        {
            allTasks.Add(task);

            int index = allTasksByDescription.BinarySearch(task, new TaskDescriptionComparer());
            if (index < 0) index = ~index;
            allTasksByDescription.Insert(index, task);
        }

        // remove a task from the static list, given its ID
        public static void RemoveTask(Guid taskID)
        {
            Task taskToRemove = allTasks.Find(task => task.id == taskID);

            allTasks.Remove(taskToRemove);
            allTasksByDescription.Remove(taskToRemove);
        }

        public int CompareTo(Task other)
        {
            if (this.dueDate != null && other.dueDate != null && dueDate.Value.CompareTo(other.dueDate.Value) != 0)
            {
                return this.dueDate.Value.CompareTo(other.dueDate.Value);
            }
            else if (this.dueDate != null && other.dueDate == null)
            {
                return 1;
            }
            else if (this.dueDate == null && other.dueDate != null)
            {
                return -1;
            }
            else if (this.dueDate == null && other.dueDate == null)
            {
                return 0;
            }
            // If both task has the same completion date, then sort by description
            else if (this.dueDate.Value == other.dueDate.Value)
            {
                return this.description.CompareTo(other.description);
            }
            return 0;

        }

        // Compare by name with Arbitrary searches
        public static int CompareTasksByDescription(Task targetTask, Task searchTask)
        {
            return targetTask.description.CompareTo(searchTask.description);
        }

    }

}
