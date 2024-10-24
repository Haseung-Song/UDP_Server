using System;
using System.Globalization;
using System.Windows.Data;

namespace ByteArrayToHexStringConverter
{
    public class ByteArrayToHexStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] messageBytes)
            {
                return $"Bytes: {BitConverter.ToString(messageBytes).Replace("-", " ")}"; // MessageBytes가 있을 때만 16진수 문자열로 출력
            }
            return string.Empty; // 값이 없으면 출력 (X)
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

}
