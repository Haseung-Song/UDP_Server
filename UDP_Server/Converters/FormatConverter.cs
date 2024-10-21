using System;
using System.Globalization;
using System.Windows.Data;

namespace FormatConverter
{
    public class FormatConverter : IValueConverter
    {
        // byte -> string 변환 (예: 0 -> "OFF", 1 -> "ON")
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte byteValue)
            {
                return byteValue == 1 ? "ON" : "OFF";
            }
            return "OFF";  // 기본 값으로 OFF를 반환
        }

        // string -> byte 변환 (예: "ON" -> 1, "OFF" -> 0)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string stringValue)
            {
                return stringValue == "ON" ? (byte)1 : (byte)0;
            }
            return (byte)0;  // 기본 값으로 0 반환
        }

    }

}