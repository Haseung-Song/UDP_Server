using System;
using System.Globalization;
using System.Windows.Data;

namespace ByteArrayToHexStrConverter
{
    public class ByteArrayToHexStrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] messageBytes)
            {
                if (messageBytes.Length >= 2)
                {
                    return $"Bytes: {BitConverter.ToString(messageBytes).Replace("-", " ")}"; // Bytes => [16진수] 문자열로 변환 후 출력
                }

            }
            return string.Empty; // 값이 없으면 출력 (X)
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

}
