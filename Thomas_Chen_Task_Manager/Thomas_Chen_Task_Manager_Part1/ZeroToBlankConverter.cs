using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Thomas_Chen_Task_Manager
{
    public class ZeroToBlankConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            int count = (int)value;
            return count == 0 ? "" : count.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class NotesToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            string notes = (string)value;
            return string.IsNullOrEmpty(notes) ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
