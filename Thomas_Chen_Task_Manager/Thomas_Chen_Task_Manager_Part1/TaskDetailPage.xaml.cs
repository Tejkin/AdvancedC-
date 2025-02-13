using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Thomas_Chen_Task_Manager
{
    public sealed partial class TaskDetailPage : Page
    {
        private Guid? _taskId;
        private Guid _folderId;
        private TasksAndFoldersStaticList _tasksAndFoldersStaticList = new TasksAndFoldersStaticList();

        public TaskDetailPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter != null)
            {
                var parameters = (dynamic)e.Parameter;
                _folderId = parameters.FolderId;
                _taskId = parameters.TaskId as Guid?;

                if (_taskId.HasValue)
                {
                    LoadTaskDetails();
                }
            }
            UpdateBackButtonVisibility();
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                e.Handled = true;
                Frame.GoBack();
                
            }
        }

        private void UpdateBackButtonVisibility()
        {
            var visibility = Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = visibility;
        }

        private void LoadTaskDetails()
        {
            var task = Task.allTasks.FirstOrDefault(t => t.id == _taskId);

            if (task != null)
            {
                DescriptionTextBox.Text = task.description;
                NotesTextBox.Text = task.notes;
                DueDatePicker.Date = task.dueDate.HasValue ? task.dueDate.Value : DateTime.Now;
                IsCompletedCheckBox.IsChecked = task.isCompleted;
            }

        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

            if (_taskId.HasValue)
            {
                var task = Task.allTasks.FirstOrDefault(t => t.id == _taskId.Value);
                if (task != null)
                {
                    task.description = DescriptionTextBox.Text;
                    task.notes = NotesTextBox.Text;
                    task.dueDate = DueDatePicker.Date.DateTime;
                    task.isCompleted = IsCompletedCheckBox.IsChecked.Value;
                }
            }
            else
            {
                // Adding a new task
                var newTask = new Task(DescriptionTextBox.Text, NotesTextBox.Text, DueDatePicker.Date.DateTime)
                {
                    isCompleted = IsCompletedCheckBox.IsChecked.Value
                };
                Task.AddTask(newTask);

                // Add the task to the folder
                var folder = Folder.allFolders.FirstOrDefault(f => f.id == _folderId);
                if (folder != null)
                {
                    folder.AddTask(newTask.id);
                }
            }
            _tasksAndFoldersStaticList.SaveTasksAndFolders();
            Frame.GoBack();
        }
    }
}
