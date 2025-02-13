using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using PCLStorage;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections;
using System.Windows;
using System.Threading;
using Xamarin.Forms.Internals;
using System.Runtime.CompilerServices;

namespace HowLongSince
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<Tasks> tasks; 
        public String folderName = "tasksFolder";
        public String fileName = "tasksFile.txt";
        List<Tasks> taskList = new List<Tasks>();

        private Timer timer;

        public class Tasks
        {
            public string name { get; set; }
            public string time { get; set; }
            public string duration { get; set; }

            public Tasks(string name, string time, string duration)
            {
                this.name = name;
                this.time = time;
                this.duration = duration;
            }
        }

        public class ObservableTasks
        {
            /*public string name { get; set; }
            public string duration { get; set; }*/
            public string name { get; set; }
            public string duration { get; set; }
            public double FontSize { get; set; }


        }

        public ObservableCollection<ObservableTasks> observableTasks;

        
        public MainPage()
        {
            InitializeComponent();

            loadTask();

            updateList();

            timer = new Timer(RefreshList, null, TimeSpan.Zero, TimeSpan.FromSeconds(1));

        }

        private void RefreshList(object state)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                // Update the list
                updateList();
            });
        }

        public async void saveTask(String task)
        {
            IFolder folder = FileSystem.Current.LocalStorage;
            folder = await folder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);

            IFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);
            await file.WriteAllTextAsync(task);
            Debug.WriteLine(folder.Path);
        }
        public async void loadTask()
        {
            IFolder folder = FileSystem.Current.LocalStorage;
            folder = await folder.CreateFolderAsync(folderName, CreationCollisionOption.OpenIfExists);

            IFile file = await folder.CreateFileAsync(fileName, CreationCollisionOption.OpenIfExists);

            string loadedTasks = await file.ReadAllTextAsync();

            var tempList = JsonConvert.DeserializeObject<List<Tasks>>(loadedTasks);
            if (tempList != null)
            {
                taskList = tempList;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DarkMode();
            FontSize();

            updateList();
        }

        private void updateList()
        {
            /*observableTasks = new ObservableCollection<ObservableTasks>();
            for (int i = 0; i < taskList.Count; i++)
            {
                string now = DateTime.Now.ToString("HH:mm:ss");


                TimeSpan duration = (Convert.ToDateTime(now) - Convert.ToDateTime(taskList[i].time)).Duration();
                taskList[i].duration = duration.ToString();
                observableTasks.Add(new ObservableTasks { name = taskList[i].name, duration = taskList[i].duration });
            }
            tasksListView.ItemsSource = observableTasks;*/
            observableTasks = new ObservableCollection<ObservableTasks>();
            for (int i = 0; i < taskList.Count; i++)
            {
                string now = DateTime.Now.ToString("HH:mm:ss");
                if (Application.Current.Properties.ContainsKey("fontSize"))
                {
                    var fontSizeObject = Application.Current.Properties["fontSize"];
                    int fontSize = (int)fontSizeObject;

                    TimeSpan duration = (Convert.ToDateTime(now) - Convert.ToDateTime(taskList[i].time)).Duration();
                    taskList[i].duration = duration.ToString();
                    observableTasks.Add(new ObservableTasks { name = taskList[i].name, duration = taskList[i].duration, FontSize = fontSize });
                }
                else
                {
                    TimeSpan duration = (Convert.ToDateTime(now) - Convert.ToDateTime(taskList[i].time)).Duration();
                    taskList[i].duration = duration.ToString();
                    observableTasks.Add(new ObservableTasks { name = taskList[i].name, duration = taskList[i].duration, FontSize = 14 });
                }
                
            }
            tasksListView.ItemsSource = observableTasks;

        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(taskEntry.Text) || string.IsNullOrWhiteSpace(taskEntry.Text))
            {
                Debug.WriteLine("Entry cannot be empty");
            }
            else
            {
                taskList.Add(new Tasks(taskEntry.Text, DateTime.Now.ToString("HH:mm:ss"), DateTime.Now.ToString("HH:mm:ss")));

                String jsonString = JsonConvert.SerializeObject(taskList);
                Debug.WriteLine(jsonString);
                saveTask(jsonString);

                taskEntry.Text = "";
            }
            Debug.WriteLine(taskList);
            updateList();
        }

        private async void preferences_Clicked(object sender, EventArgs e)
        {
            Page1 settings = new Page1();
            await Navigation.PushModalAsync(settings);
        }

        public void DarkMode()
        {

            if (Application.Current.Properties.ContainsKey("darkMode"))
            {
                var darkMode = Application.Current.Properties["darkMode"];
                Debug.WriteLine(darkMode);
                if (darkMode is bool && (bool)darkMode)
                {
                    ChangeBackground(Color.DarkSlateGray);
                    ChangeButtonColor(Color.SlateGray);
                    ChangeTextColor(Color.White);
                }
                if (darkMode is bool && !(bool)darkMode)
                {
                    ChangeBackground(Color.White);
                    ChangeButtonColor(Color.LightGray);
                    ChangeTextColor(Color.Black);
                }
            }
        }

        public void ChangeBackground(Color color)
        {
            initialStack.BackgroundColor = color;
            preferences.BackgroundColor = color;
            add.BackgroundColor = color;
        }

        public void ChangeButtonColor(Color color)
        {

            tasksListView.BackgroundColor = color;
        }

        public void ChangeTextColor(Color color)
        {
            taskLabel.TextColor = color;
            taskEntry.TextColor = color;
        }

        public void FontSize()
        {
            if (Application.Current.Properties.ContainsKey("fontSize"))
            {
                var fontSizeObject = Application.Current.Properties["fontSize"];
                int fontSize = (int)fontSizeObject;
                Debug.WriteLine(fontSize.ToString());
                ChangeFontSize(fontSize);
            }
        }

        public void ChangeFontSize(int fontSize)
        {
            /*foreach (var item in tasksListView.ItemsSource)
            {
                if (item is Label label)
                {
                    label.FontSize = fontSize;
                }
            }*/
            foreach (var item in observableTasks)
            {
                item.FontSize = 100; // Set the desired font size
            }

            tasksListView.ItemsSource = null;
            tasksListView.ItemsSource = observableTasks;
        }

        private void tasksListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ObservableTasks task = observableTasks[e.ItemIndex];
            observableTasks.Remove(task);
            taskList.RemoveAt(e.ItemIndex);

            String jsonString = JsonConvert.SerializeObject(taskList); //saves changes made to favourite
            saveTask(jsonString);
        }
    }
}
