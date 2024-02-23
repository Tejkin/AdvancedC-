using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace TaskManagerPt1
{
    public class Task
    {
        private Guid _id = Guid.NewGuid();
        public string description;
        public string notes;
        public bool completed = false;
        public DateTime completionDate;
        private bool _overdue;

        public Guid id
        {
            get { return _id; }
        }
        public bool overdue
        { 
            set
            {
                if (completionDate >= DateTime.Now)
                {
                    _overdue = true;
                }
                else
                {
                    _overdue = false;
                }
            }
            get { return _overdue; }
        }

        
        public Task(string description, string notes, DateTime completionDate)
        {
            this.description = description;
            this.notes = notes;
            this.completionDate = completionDate;
        }

    }

    public class RepeatingTask : Task
    {
        public int repeatInDays;
        public int 
    }
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Task myTask = new Task("Clean", "Clean the kitchen", new DateTime(2024, 12, 25));

            Debug.WriteLine(myTask.id.ToString());
            Debug.WriteLine(myTask.overdue.ToString());
        }
    }
}
