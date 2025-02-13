using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Thomas_Chen_Task_Manager
{
    public class DataModelV2
    {
        public string connectionString = "Data Source=tasks.db";

        public DataModelV2()
        {
            InitializeDatabase();
            LoadDataFromOriginalModel();
        }

        private void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string createTasksTable = @"
                    CREATE TABLE IF NOT EXISTS Tasks (
                        id TEXT PRIMARY KEY,
                        description TEXT,
                        notes TEXT,
                        isCompleted INTEGER,
                        dueDate TEXT
                    )";

                string createFoldersTable = @"
                    CREATE TABLE IF NOT EXISTS Folders (
                        id TEXT PRIMARY KEY,
                        name TEXT
                    )";

                string createFolderTasksTable = @"
                    CREATE TABLE IF NOT EXISTS FolderTasks (
                        folderId TEXT,
                        taskId TEXT,
                        PRIMARY KEY (folderId, taskId),
                        FOREIGN KEY (folderId) REFERENCES Folders(id),
                        FOREIGN KEY (taskId) REFERENCES Tasks(id)
                    )";

                using (var command = new SqliteCommand(createTasksTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SqliteCommand(createFoldersTable, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SqliteCommand(createFolderTasksTable, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private void LoadDataFromOriginalModel()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                foreach (var task in Task.allTasks)
                {
                    InsertTask(task);
                }

                foreach (var folder in Folder.allFolders)
                {
                    InsertFolder(folder);
                }
            }
        }

        public void InsertTask(Task task)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string insertTask = @"
                    INSERT INTO Tasks (id, description, notes, isCompleted, dueDate)
                    VALUES (@id, @description, @notes, @isCompleted, @dueDate)";

                using (var command = new SqliteCommand(insertTask, connection))
                {
                    command.Parameters.AddWithValue("@id", task.id.ToString());
                    command.Parameters.AddWithValue("@description", task.description);
                    command.Parameters.AddWithValue("@notes", task.notes);
                    command.Parameters.AddWithValue("@isCompleted", task.isCompleted ? 1 : 0);
                    command.Parameters.AddWithValue("@dueDate", task.dueDate?.ToString("o")); // "o" for ISO 8601 format

                    command.ExecuteNonQuery();
                }
            }
        }

        public void InsertFolder(Folder folder)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string insertFolder = @"
                    INSERT INTO Folders (id, name)
                    VALUES (@id, @name)";

                using (var command = new SqliteCommand(insertFolder, connection))
                {
                    command.Parameters.AddWithValue("@id", folder.id.ToString());
                    command.Parameters.AddWithValue("@name", folder.name);

                    command.ExecuteNonQuery();
                }

                foreach (var taskId in folder.taskIDs)
                {
                    InsertFolderTask(folder.id, taskId);
                }
            }
        }

        public void InsertFolderTask(Guid folderId, Guid taskId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string insertFolderTask = @"
                    INSERT INTO FolderTasks (folderId, taskId)
                    VALUES (@folderId, @taskId)";

                using (var command = new SqliteCommand(insertFolderTask, connection))
                {
                    command.Parameters.AddWithValue("@folderId", folderId.ToString());
                    command.Parameters.AddWithValue("@taskId", taskId.ToString());

                    command.ExecuteNonQuery();
                }
            }
        }

        public void RemoveTask(Guid taskId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string deleteTask = @"
                    DELETE FROM Tasks
                    WHERE id = @id";

                using (var command = new SqliteCommand(deleteTask, connection))
                {
                    command.Parameters.AddWithValue("@id", taskId.ToString());

                    command.ExecuteNonQuery();
                }

                string deleteFolderTask = @"
                    DELETE FROM FolderTasks
                    WHERE taskId = @taskId";

                using (var command = new SqliteCommand(deleteFolderTask, connection))
                {
                    command.Parameters.AddWithValue("@taskId", taskId.ToString());

                    command.ExecuteNonQuery();
                }
            }
        }

        public void RemoveFolder(Guid folderId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string deleteFolder = @"
                    DELETE FROM Folders
                    WHERE id = @id";

                using (var command = new SqliteCommand(deleteFolder, connection))
                {
                    command.Parameters.AddWithValue("@id", folderId.ToString());

                    command.ExecuteNonQuery();
                }

                string deleteFolderTasks = @"
                    DELETE FROM FolderTasks
                    WHERE folderId = @folderId";

                using (var command = new SqliteCommand(deleteFolderTasks, connection))
                {
                    command.Parameters.AddWithValue("@folderId", folderId.ToString());

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateTask(Task task)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string updateTask = @"
                    UPDATE Tasks
                    SET description = @description,
                        notes = @notes,
                        isCompleted = @isCompleted,
                        dueDate = @dueDate
                    WHERE id = @id";

                using (var command = new SqliteCommand(updateTask, connection))
                {
                    command.Parameters.AddWithValue("@id", task.id.ToString());
                    command.Parameters.AddWithValue("@description", task.description);
                    command.Parameters.AddWithValue("@notes", task.notes);
                    command.Parameters.AddWithValue("@isCompleted", task.isCompleted ? 1 : 0);
                    command.Parameters.AddWithValue("@dueDate", task.dueDate?.ToString("o"));

                    command.ExecuteNonQuery();
                }
            }
        }

        public void UpdateFolder(Folder folder)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string updateFolder = @"
                    UPDATE Folders
                    SET name = @name
                    WHERE id = @id";

                using (var command = new SqliteCommand(updateFolder, connection))
                {
                    command.Parameters.AddWithValue("@id", folder.id.ToString());
                    command.Parameters.AddWithValue("@name", folder.name);

                    command.ExecuteNonQuery();
                }

                string deleteFolderTasks = @"
                    DELETE FROM FolderTasks
                    WHERE folderId = @folderId";

                using (var command = new SqliteCommand(deleteFolderTasks, connection))
                {
                    command.Parameters.AddWithValue("@folderId", folder.id.ToString());

                    command.ExecuteNonQuery();
                }

                foreach (var taskId in folder.taskIDs)
                {
                    InsertFolderTask(folder.id, taskId);
                }
            }
        }
        public void RemoveFolderTask(Guid folderId, Guid taskId)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string deleteFolderTask = @"
                DELETE FROM FolderTasks
                WHERE folderId = @folderId AND taskId = @taskId";

                using (var command = new SqliteCommand(deleteFolderTask, connection))
                {
                    command.Parameters.AddWithValue("@folderId", folderId.ToString());
                    command.Parameters.AddWithValue("@taskId", taskId.ToString());

                    command.ExecuteNonQuery();
                }
            }
        }
    }

    public class DataModelV2Tests
    {
        private static DataModelV2 dataModelV2;

        public void Test()
        {
            Initialise();

            TestAddTask();
            TestRemoveTask();
            TestAddFolder();
            TestRemoveFolder();
            TestUpdateTask();
            TestUpdateFolder();

            Debug.WriteLine("All tests completed.");
        }

        private static void Initialise()
        {
            // Initialize DataModelV2
            dataModelV2 = new DataModelV2();

            // Clear original model data
            Task.allTasks.Clear();
            Folder.allFolders.Clear();

            // Clear SQL data for a fresh start
            using (var connection = new SqliteConnection(dataModelV2.connectionString))
            {
                connection.Open();

                var deleteTasks = new SqliteCommand("DELETE FROM Tasks", connection);
                var deleteFolders = new SqliteCommand("DELETE FROM Folders", connection);
                var deleteFolderTasks = new SqliteCommand("DELETE FROM FolderTasks", connection);

                deleteTasks.ExecuteNonQuery();
                deleteFolders.ExecuteNonQuery();
                deleteFolderTasks.ExecuteNonQuery();
            }
        }

        private static void TestAddTask()
        {
            var task = new Task("Test Task", "Notes", DateTime.Now.AddDays(1));
            Task.AddTask(task);

            Debug.Assert(Task.allTasks.Contains(task), "Task not found in original model");

            using (var connection = new SqliteConnection(dataModelV2.connectionString))
            {
                connection.Open();

                var selectTask = new SqliteCommand($"SELECT COUNT(*) FROM Tasks WHERE id = '{task.id}'", connection);
                var count = (long)selectTask.ExecuteScalar();

                Debug.Assert(count == 1, "Task not found in SQL model");
            }

            Debug.WriteLine("TestAddTask passed.");
        }

        private static void TestRemoveTask()
        {
            var task = new Task("Test Task", "Notes", DateTime.Now.AddDays(1));
            Task.AddTask(task);
            Task.RemoveTask(task.id);

            Debug.Assert(!Task.allTasks.Contains(task), "Task not removed from original model");

            using (var connection = new SqliteConnection(dataModelV2.connectionString))
            {
                connection.Open();

                var selectTask = new SqliteCommand($"SELECT COUNT(*) FROM Tasks WHERE id = '{task.id}'", connection);
                var count = (long)selectTask.ExecuteScalar();

                Debug.Assert(count == 0, "Task not removed from SQL model");
            }

            Debug.WriteLine("TestRemoveTask passed.");
        }

        private static void TestAddFolder()
        {
            var folder = new Folder("Test Folder");
            Folder.AddFolder(folder);

            Debug.Assert(Folder.allFolders.Contains(folder), "Folder not found in original model");

            using (var connection = new SqliteConnection(dataModelV2.connectionString))
            {
                connection.Open();

                var selectFolder = new SqliteCommand($"SELECT COUNT(*) FROM Folders WHERE id = '{folder.id}'", connection);
                var count = (long)selectFolder.ExecuteScalar();

                Debug.Assert(count == 1, "Folder not found in SQL model");
            }

            Debug.WriteLine("TestAddFolder passed.");
        }

        private static void TestRemoveFolder()
        {
            var folder = new Folder("Test Folder");
            Folder.AddFolder(folder);
            Folder.RemoveFolder(folder.id);

            Debug.Assert(!Folder.allFolders.Contains(folder), "Folder not removed from original model");

            using (var connection = new SqliteConnection(dataModelV2.connectionString))
            {
                connection.Open();

                var selectFolder = new SqliteCommand($"SELECT COUNT(*) FROM Folders WHERE id = '{folder.id}'", connection);
                var count = (long)selectFolder.ExecuteScalar();

                Debug.Assert(count == 0, "Folder not removed from SQL model");
            }

            Debug.WriteLine("TestRemoveFolder passed.");
        }

        private static void TestUpdateTask()
        {
            var task = new Task("Test Task", "Notes", DateTime.Now.AddDays(1));
            Task.AddTask(task);

            task.description = "Updated Task";
            task.UpdateTask();

            using (var connection = new SqliteConnection(dataModelV2.connectionString))
            {
                connection.Open();

                var selectTask = new SqliteCommand($"SELECT description FROM Tasks WHERE id = '{task.id}'", connection);
                var description = (string)selectTask.ExecuteScalar();

                Debug.Assert(description == "Updated Task", "Task description not updated in SQL model");
            }

            Debug.WriteLine("TestUpdateTask passed.");
        }

        private static void TestUpdateFolder()
        {
            var folder = new Folder("Test Folder");
            Folder.AddFolder(folder);

            folder.name = "Updated Folder";
            folder.UpdateFolder();

            using (var connection = new SqliteConnection(dataModelV2.connectionString))
            {
                connection.Open();

                var selectFolder = new SqliteCommand($"SELECT name FROM Folders WHERE id = '{folder.id}'", connection);
                var name = (string)selectFolder.ExecuteScalar();

                Debug.Assert(name == "Updated Folder", "Folder name not updated in SQL model");
            }

            Debug.WriteLine("TestUpdateFolder passed.");
        }
    }
}

