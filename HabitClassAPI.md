# Habit Manager API Documentation

## Part 1 - Overview

The `Habit` class is designed to manage habits, which are repeating tasks but with an extra goal of maintaing its streak. It builds upon the functionality of the `RepeatingTask` class to track the current and longest streaks of habit completions.
```csharp
public class Habit : RepeatingTask
```
Inheritance: RepeatingTask → Habit


### Properties

1. **currentStreak (int)**
   - **Description**: The current streak of consecutive habit completions.
   - **Access**: Read-only
   - **Example**:
     ```csharp
     int current = habit.currentStreak;
     ```

2. **longestStreak (int)**
   - **Description**: The longest streak of consecutive habit completions.
   - **Access**: Read-only
   - **Example**:
     ```csharp
     int longest = habit.longestStreak;
     ```

### Methods

1. **Habit(string description, RepeatType repeatFrequency, string notes = "")**
   - **Description**: The constructor used to create a new habit with the given description, repeat frequency, and notes.
   - **Example**:
     ```csharp
     Habit exerciseHabit = new Habit("Exercise daily", RepeatType.Daily, "30 minutes of exercise");
     ```

2. **CompleteTask()**
   - **Description**: Sets the habit as completed and updates the streaks. If the task is completed before the due date, the streak is incremented. If not, the current streak is reset. Additionally, if the current streak exceeds the longest streak, the current streak value replaces the longest streak value.
   - **Returns:** void
   - **Example**:
     ```csharp
     exerciseHabit.CompleteTask();
     ```