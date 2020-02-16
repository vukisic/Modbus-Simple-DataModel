using Common.Devices;
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace Server.Converters
{
    public class ValueToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is AnalogInput)
            {
                var item = value as AnalogInput;
                if (item.Value <= item.MaxValue && item.Value >= item.MinValue)
                    return Brushes.Green;
                else
                    return Brushes.Red;
            }
            else if(value is AnalogOutput)
            {
                var item = value as AnalogOutput;
                if (item.Value <= item.MaxValue && item.Value >= item.MinValue)
                    return Brushes.Green;
                else
                    return Brushes.Red;
            }
            else if (value is DigitalInput)
            {
                var item = value as DigitalInput;
                if (item.Value != item.MaxValue)
                    return Brushes.Green;
                else
                    return Brushes.Red;
            }
            else if (value is DigitalOutput)
            {

                var item = value as DigitalOutput;
                if (item.Value != item.MaxValue)
                    return Brushes.Green;
                else
                    return Brushes.Red;
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
