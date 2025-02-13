# Folder Manager API Documentation

## Part 1 - Overview

The `Folder` class e is designed to manage a collection of tasks grouped into folders.


### Properties

1. **id (Guid)**
   - **Description**: A unique identifier for the folder.
   - **Access**: Read-only
   - **Example**:
     ```csharp
     Guid folderId = folder.id;
     ```

2. **name (string)**
   - **Description**: The name of the folder.
   - **Access**: Read/Write
   - **Example**:
     ```csharp
     folder.name = "Work Related Tasks";
     ```

3. **taskIDs (List<Guid>)**
   - **Description**: A list of task IDs within the folder.
   - **Access**: Read/Write
   - **Example**:
     ```csharp
     List<Guid> tasks = folder.taskIDs;
     ```

4. **IncompleteTaskCount (int)**
   - **Description**: The total count of incomplete tasks in the folder.
   - **Access**: Read-only
   - **Example**:
     ```csharp
     int incompleteCount = folder.IncompleteTaskCount;
     ```

### Methods

1. **Folder(string name)**
   - **Description**: Constructor to create a new folder with the given name.
   - **Example**:
     ```csharp
     Folder projectFolder = new Folder("Home Chores");
     ```

2. **AddFolder(Folder folder)**
   - **Description**: Adds a new folder to the static list of all folders.
   - **Example**:
     ```csharp
     Folder.AddFolder(projectFolder);
     ```

3. **RemoveFolder(Guid folderID)**
   - **Description**: Removes a folder from the static list given its ID.
   - **Example**:
     ```csharp
     Folder.RemoveFolder(folderId);
     ```

4. **AddTask(Guid taskID)**
   - **Description**: Adds a task to the folder.
   - **Example**:
     ```csharp
     projectFolder.AddTask(taskId);
     ```

5. **RemoveTask(Guid taskID)**
   - **Description**: Removes a task from the folder.
   - **Example**:
     ```csharp
     projectFolder.RemoveTask(taskId);
     ```