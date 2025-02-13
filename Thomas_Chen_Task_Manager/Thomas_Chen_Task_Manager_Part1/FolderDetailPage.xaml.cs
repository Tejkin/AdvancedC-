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
    public sealed partial class FolderDetailPage : Page
    {
        private TasksAndFoldersStaticList _tasksAndFoldersStaticList = new TasksAndFoldersStaticList();
        public FolderDetailPage()
        {
            this.InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Save the folder name
            string folderName = FolderNameTextBox.Text;

            Folder newFolder = new Folder(folderName);
            Folder.AddFolder(newFolder);

            _tasksAndFoldersStaticList.SaveTasksAndFolders();
            // Navigate back to MainPage
            Frame.GoBack();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to MainPage without saving
            Frame.GoBack();
        }
    }
}
