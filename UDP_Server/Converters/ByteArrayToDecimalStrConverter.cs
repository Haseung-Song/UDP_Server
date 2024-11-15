using System;
using System.Globalization;
using System.Windows.Data;

namespace ByteArrayToDecimalStrConverter
{
    public class ByteArrayToDecimalStrConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is byte[] messageBytes)
            {
                // [리틀 엔디안]으로 들어온 경우에, [바이트 배열]을 리버스 후, [빅 엔디안] 변환
                // 즉, 클라이언트 측에서 데이터를 보낼 때, [빅 엔디안]으로 보내준다는 의미
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(messageBytes);
                }
                // Ex. [OO Bytes] => [OO진수] 문자열로 변환 후 출력
                string result;
                if (messageBytes.Length == 2)
                {
                    result = ((decimal)BitConverter.ToInt16(messageBytes, 0)).ToString("N0", CultureInfo.InvariantCulture); // [2 Bytes] => [10진수] 문자열로 변환 후 출력
                }
                else if (messageBytes.Length == 4)
                {
                    result = ((decimal)BitConverter.ToInt32(messageBytes, 0)).ToString("N0", CultureInfo.InvariantCulture); // [4 Bytes] => [10진수] 문자열로 변환 후 출력
                }
                else
                {
                    return null; // 다른 [OO Bytes]는 [null] 출력
                }
                return $"Decimal: {result}"; // [쉼표]가 포함된 [형식]으로 [반환]
            }
            return string.Empty; // 값이 없으면 출력 (X)
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

}