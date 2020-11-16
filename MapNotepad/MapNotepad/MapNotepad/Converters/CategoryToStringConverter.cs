using MapNotepad.Enums;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace MapNotepad.Converters
{
    public class CategoryToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((SearchCategories)(int)value).ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (int)(SearchCategories)value;
        }

    }
}
