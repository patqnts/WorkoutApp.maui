using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace WorkoutApp.MVVM.Converter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public class BoolToButtonTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool isRunning = (bool)value;
            return isRunning ? "Pause" : "Play";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
