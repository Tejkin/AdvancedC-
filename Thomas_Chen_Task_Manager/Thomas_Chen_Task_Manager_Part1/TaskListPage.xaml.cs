using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using Windows.Storage;
using Newtonsoft.Json;

namespace Thomas_Chen_Task_Manager
{
    public sealed partial class TaskListPage : Page
    {
        private Guid _currentFolderId;
        private TasksAndFoldersStaticList _tasksAndFoldersStaticList = new TasksAndFoldersStaticList();

        public TaskListPage()
        {
            this.InitializeComponent();
            PreferencesPage.EventAggregator.SortOrderChangedEvent += OnSortOrderChanged;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _currentFolderId = (Guid)e.Parameter;
            //LoadData();
            _tasksAndFoldersStaticList.LoadTasksAndFolders();
            LoadTasks();
        }

        private void OnSortOrderChanged()
        {
            LoadTasks();
        }

        private void LoadTasks()
        {
            var folder = Folder.allFolders.FirstOrDefault(f => f.id == _currentFolderId);

            if (folder != null)
            {
                var tasks = folder.taskIDs.Select(id => Task.allTasks.FirstOrDefault(t => t.id == id)).ToList();

                var localSettings = ApplicationData.Current.LocalSettings;
                var sortOrder = localSettings.Values["SortOrder"] as string;

                List<Task> overdueTasks = new List<Task>();
                List<Task> dueTodayTasks = new List<Task>();
                List<Task> otherTasks = new List<Task>();

                if (sortOrder == "Alphabetically")
                {
                    overdueTasks = tasks.Where(t => t.isOverdue).OrderBy(t => t.description).ToList();
                    dueTodayTasks = tasks.Where(t => t.dueDate.HasValue && t.dueDate.Value.Date == DateTime.Today).OrderBy(t => t.description).ToList();
                    otherTasks = tasks.Where(t => !t.isOverdue && (t.dueDate == null || t.dueDate.Value.Date > DateTime.Today)).OrderBy(t => t.description).ToList();
                }
                else
                {
                    overdueTasks = tasks.Where(t => t.isOverdue).OrderBy(t => t.dueDate).ToList();
                    dueTodayTasks = tasks.Where(t => t.dueDate.HasValue && t.dueDate.Value.Date == DateTime.Today).OrderBy(t => t.dueDate).ToList();
                    otherTasks = tasks.Where(t => !t.isOverdue && (t.dueDate == null || t.dueDate.Value.Date > DateTime.Today)).OrderBy(t => t.dueDate).ToList();
                }

                TaskListView.Items.Clear();

                foreach (var task in overdueTasks)
                {
                    TaskListView.Items.Add(task);
                }

                foreach (var task in dueTodayTasks)
                {
                    TaskListView.Items.Add(task);
                }

                foreach (var task in otherTasks)
                {
                    TaskListView.Items.Add(task);
                }
            }
        }

        private void TaskListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TaskListView.SelectedItem != null)
            {
                var selectedTask = (Task)TaskListView.SelectedItem;
                Guid taskId = selectedTask.id;
                Frame.Navigate(typeof(TaskDetailPage), new { FolderId = _currentFolderId, TaskId = (Guid?)taskId });
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
            //SaveData();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Task task = checkBox.DataContext as Task;
            if (task != null)
            {
                var originalTask = Task.allTasks.FirstOrDefault(t => t.id == task.id);
                if (originalTask != null)
                {
                    originalTask.isCompleted = true;
                }
            }
            _tasksAndFoldersStaticList.SaveTasksAndFolders();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            Task task = checkBox.DataContext as Task;
            if (task != null)
            {
                var originalTask = Task.allTasks.FirstOrDefault(t => t.id == task.id);
                if (originalTask != null)
                {
                    originalTask.isCompleted = false;
                }
            }
            _tasksAndFoldersStaticList.SaveTasksAndFolders();
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TaskDetailPage), new { FolderId = _currentFolderId, TaskId = (Guid?)null });
            _tasksAndFoldersStaticList.SaveTasksAndFolders();
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            Guid taskId = (Guid)button.CommandParameter;

            var task = Task.allTasks.FirstOrDefault(t => t.id == taskId);
            if (task != null)
            {
                Task.allTasks.Remove(task);

                var folder = Folder.allFolders.FirstOrDefault(f => f.id == _currentFolderId);
                if (folder != null)
                {
                    folder.RemoveTask(taskId);
                    RefreshTaskList();
                    _tasksAndFoldersStaticList.SaveTasksAndFolders();
                }
            }
        }


        public class DataContainer
        {
            public List<Task> Tasks { get; set; }
            public List<Folder> Folders { get; set; }
        }

        private void RefreshTaskList()
        {
            var folder = Folder.allFolders.FirstOrDefault(f => f.id == _currentFolderId);
            if (folder != null)
            {
                TaskListView.ItemsSource = folder.taskIDs.Select(taskId => Task.allTasks.FirstOrDefault(t => t.id == taskId)).ToList();
            }
        }
    }
}
