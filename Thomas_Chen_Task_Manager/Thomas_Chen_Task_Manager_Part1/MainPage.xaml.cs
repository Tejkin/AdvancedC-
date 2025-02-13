using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Diagnostics;
using Windows.UI.Core;
using Newtonsoft.Json;
using static Thomas_Chen_Task_Manager.TaskListPage;
using Windows.Storage;
using System.Threading.Tasks;

namespace Thomas_Chen_Task_Manager
{
    public sealed partial class MainPage : Page
    {
        private TasksAndFoldersStaticList _tasksAndFoldersStaticList = new TasksAndFoldersStaticList();
        public MainPage()
        {
            this.InitializeComponent();
            LoadAndInitializeData();
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;
            Debug.WriteLine($"Local folder path: {localFolder.Path}");
        }

        private void LoadAndInitializeData()
        {
            //LoadData();
            
            InitialiseData();
            _tasksAndFoldersStaticList.LoadTasksAndFolders();
        }


        private void InitialiseData()
        {
            if (Folder.allFolders.Count == 0)
            {
                var workFolder = new Folder("Work");
                var personalFolder = new Folder("Personal");

                Folder.AddFolder(workFolder);
                Folder.AddFolder(personalFolder);

                var task1 = new Task("Complete project report", "Report needs to be submitted by end of the week", DateTime.Now.AddDays(2));
                var task2 = new Task("Buy groceries", "Milk, Bread, Cheese", DateTime.Now.AddDays(1));
                var task3 = new Task("Call plumber", "Fix the kitchen sink", DateTime.Now.AddDays(-1));

                Task.AddTask(task1);
                Task.AddTask(task2);
                Task.AddTask(task3);

                workFolder.AddTask(task1.id);
                personalFolder.AddTask(task2.id);
                personalFolder.AddTask(task3.id);

                _tasksAndFoldersStaticList.SaveTasksAndFolders();
            }
            FolderListView.ItemsSource = Folder.allFolders;
            //SaveData();
        }

        private void FolderListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FolderListView.SelectedItem != null)
            {
                var selectedFolder = (Folder)FolderListView.SelectedItem;
                Frame.Navigate(typeof(TaskListPage), selectedFolder.id);
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            LoadAndInitializeData();
            UpdateBackButtonVisibility();
        }

        private void DeleteFolderButton_Click(object sender, RoutedEventArgs e)
        {
            
            Button button = sender as Button;
            Guid folderId = (Guid)button.CommandParameter;

            // Retrieve the folder to be deleted
            var folderToRemove = Folder.allFolders.FirstOrDefault(f => f.id == folderId);
            if (folderToRemove != null)
            {
                // Loop through each task in the folder and remove them
                foreach (var taskId in folderToRemove.taskIDs)
                {
                    Task.RemoveTask(taskId);
                }

                // Remove the folder itself
                Folder.RemoveFolder(folderId);

                // Update the FolderListView
                FolderListView.ItemsSource = null;
                FolderListView.ItemsSource = Folder.allFolders;

                // Save the updated data
                _tasksAndFoldersStaticList.SaveTasksAndFolders();
            }
        }


        private void UpdateBackButtonVisibility()
        {
            var visibility = Frame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility = visibility;
        }

        private void PreferencesButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PreferencesPage));
        }

        private void AddFolderButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FolderDetailPage));
        }
    }
}
