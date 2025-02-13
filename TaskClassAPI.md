# Task Class API
The `Task` class is designed to allow users manage their tasks in an organised application. It allows for the creation, updating, retrieval, and deletion of tasks. 
```csharp
public class Task : IComparable<Task>
```
Inheritance: IComparable → Task


### Properties

1. **id (Guid)**
   - **Description**: A unique identifier for the task.
   - **Access**: Read-only
   - **Example**:
     ```csharp
     Guid taskId = task.id;
     ```

2. **description (string)**
   - **Description**: The description of the task.
   - **Access**: Read/Write
   - **Example**:
     ```csharp
     task.description = "Complete the project documentation.";
     ```

3. **notes (string)**
   - **Description**: Additional notes/category for the task.
   - **Access**: Read/Write
   - **Example**:
     ```csharp
     task.notes = "Refer to the latest project guidelines.";
     ```

4. **isCompleted (bool)**
   - **Description**: Indicates whether the task is completed.
   - **Access**: Read/Write
   - **Example**:
     ```csharp
     task.isCompleted = true;
     ```

5. **dueDate (DateTime?)**
   - **Description**: The due date of the task. This property is nullable.
   - **Access**: Read/Write
   - **Example**:
     ```csharp
     task.dueDate = DateTime.Parse("2024-05-30");
     ```

6. **isOverdue (bool)**
   - **Description**: Indicates whether the task is overdue.
   - **Access**: Read-only
   - **Example**:
     ```csharp
     bool overdueStatus = task.isOverdue;
     ```

### Methods

1. **Task(string description, string notes = "", DateTime? dueDate = null)**
   - **Description**: Constructor used to create Task, given its description, notes and optional due date.
   - **Returns:** void
   - **Example 1**:
     ```csharp
     Task newTask = new Task("Show Example 1", "Task without a due date");
     ```
   - **Example 2**:
     ```csharp
     Task newTask = new Task("Show Example 2", "Task with a due date", DateTime.Parse("2024-05-20"));
     ```

2. **AddTask(Task task)**
   - **Description**: Adds a new task to the list of all tasks.
   - **Returns:** void
   - **Example**:
     ```csharp
     Task.AddTask(newTask);
     ```

3. **RemoveTask(Guid taskID)**
   - **Description**: Removes a task from the task list given its ID.
   - **Returns:** void
   - **Example**:
     ```csharp
     Task.RemoveTask(taskId);
     ```

4. **CompareTo(Task other)**
   - **Description**: Inherited from the IComparable class. Compares the current task with another task based on their due dates and then their descriptions.
   - **Returns:** int
   - **Example**:
     ```csharp
     int comparisonResult = task1.CompareTo(task2);
     ```

5. **CompareTasksByDescription(Task targetTask, Task searchTask)**
   - **Description**: Static method to compare tasks by their descriptions.
   - **Returns:** int
   - **Example**:
     ```csharp
     int descriptionComparison = Task.CompareTasksByDescription(task1, task2);
     ```
