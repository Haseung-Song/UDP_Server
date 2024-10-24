using System;
using System.Globalization;
using System.Windows.Data;

namespace ByteToHexStringConverter
{
    public class ByteToHexStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte messageByte)
            {
                // X: 숫자를 16진수(헥사)로 변환
                // 2: 출력되는 16진수 값의 자릿수를 두 자리로 세팅
                return $"Byte: 0x{messageByte:X2}"; // 바이트를 16진수 문자열로 변환
            }
            return string.Empty; // 값이 없으면 출력 (X)
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

}
