# RepeatingTask Class

The `RepeatingTask` class is designed to manage repeating tasks that can occur daily or weekly. It builds onto the functionality of the `Task` class to handle tasks that need to be completed on a daily or weekly basis.
```csharp
public class RepeatingTask : Task
```
Inheritance: Task → RepeatingTask


### Enums

1. **RepeatType**
   - **Description**: Enum to define how frequent task is repeated.
   - **Values**:
     - `Daily`: Task repeats daily.
     - `Weekly`: Task repeats weekly.
   - **Example**:
     ```csharp
     RepeatType daily = RepeatType.Daily;
     RepeatType weekly = RepeatType.Weekly;
     ```

### Properties

1. **RepeatFrequency (RepeatType)**
   - **Description**: Defines how often the task repeats (daily or weekly).
   - **Access**: Read-only
   - **Example**:
     ```csharp
     RepeatType frequency = repeatingTask.RepeatFrequency;
     ```

### Methods

1. **RepeatingTask(string description, RepeatType repeatFrequency, string notes = "", DateTime? = dueDate = null)**
   - **Description**: Constructor to create a new repeating task with the given description, repeat frequency and notes.
   - **Example**:
     ```csharp
     RepeatingTask dailyTask = new RepeatingTask("Daily meeting", RepeatType.Daily, "Discuss daily assignment");
     RepeatingTask weeklyTask = new RepeatingTask("Weekly report", RepeatType.Weekly, "Report weekly project progress");
     ```

2. **CompleteTask()**
   - **Description**: Marks the task as completed and reschedules it based on the repeat frequency. If the task is completed, another due date will be set depending on its repeat frequency.
   - **Returns:** void
   - **Example**:
     ```csharp
     dailyTask.CompleteTask();
     ```