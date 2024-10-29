using System;
using System.Globalization;
using System.Windows.Data;

namespace ByteToDecimalStrConverter
{
    public class ByteToDecimalStrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte messageByte)
            {
                // D: 숫자를 10진수(Decimal)로 변환
                return $"{messageByte:D}"; // Byte => [10진수] 문자열로 변환 후 출력
            }
            return string.Empty; // 값이 없으면 출력 (X)
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

}
