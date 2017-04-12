using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

namespace Appl
{
    public class DebugConverter : IValueConverter
    {
        public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            return value;
        }

        public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            return value;
        }
    }

    public class TimeSpanToStringConverter : IValueConverter
    {
        public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            TimeSpan val = (TimeSpan)value;

            if ( (string)parameter == "m:s" )
            {
                if(val.Minutes > 0)
                    return val.Minutes + "min : " + val.Seconds + "sec";
                else
                    return val.Seconds + "sec";
            }

            return value;
        }

        public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }


    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            string param = (string)parameter;

            if ( param == null )
                param = "";

            var val = (bool)value;
            if (param.ToLower().Contains('i'))
                val = !val;

            System.Windows.Visibility vis;

            if(param.ToLower().Contains('h'))
                vis = System.Windows.Visibility.Hidden;
            else
                vis = System.Windows.Visibility.Collapsed;

            
            if ( val )
                return System.Windows.Visibility.Visible;
            else
                return System.Windows.Visibility.Hidden;
        }

        public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }

    public class DoublerDiv2Converter : IValueConverter
    {
        public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            var val = (double)value;
            return val/2;
        }

        public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }

    public class InvertBoolConverter : IValueConverter
    {
        public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            var val = (bool)value;
            return !val;
        }

        public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }

    
    public class BoolToForegroundColorConverter : IValueConverter
    {
        public object Convert ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            if ( value == null )
                return value;

            if ( value is bool )
            {
                var val = value as bool?;
                if ( val == true )
                    return new SolidColorBrush( Colors.Green );
                else
                    return new SolidColorBrush( Colors.Red );
            }

            return new SolidColorBrush( Colors.Red );
        }

        public object ConvertBack ( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }

    public class ConcatValueUnitConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values[0] != null)
            {
                double val;
                string tmp;
                if ( double.TryParse( values[0].ToString(), out val ) )
                {
                    tmp = val.ToString( "0.0" );
                }
                else
                    return null;

                string units = "";

                if (values.Length > 1)
                    units = values[1] as string;

                return tmp + " " + units;
            }
            else
                return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class StringToBitmapConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            if ( value == null )
                return value;

            string path = value as string;

            if ( value is string )
            {
                if ( path != "" )
                {
                    try
                    {
                        string p = (string)parameter;
                        return new BitmapImage( new Uri( path, UriKind.Absolute ) );
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                    return value;
            }

            return value;
        }

        public object ConvertBack( object value, Type targetType, object parameter, System.Globalization.CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }


}
