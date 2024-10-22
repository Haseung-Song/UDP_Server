namespace UDP_Server.Converters
{
    public class BitOperatorConverter
    {
        /// <summary>
        /// [GetBit(): 특정 바이트에서 원하는 비트를 추출하는 메서드]
        /// </summary>
        /// <param name="data"></param> // 바이트 배열
        /// <param name="byteIndex"></param> // 바이트 인덱스
        /// <param name="bitPosition"></param> // 추출할 비트 위치 (0~7)
        /// <returns></returns>
        public static byte GetBitFrom7(byte[] data, int byteIndex, int bitPosition)
        {
            return (byte)((data[byteIndex] >> bitPosition) & 0x01);
        }

        public static byte GetBitFrom6To5(byte[] data, int byteIndex)
        {
            return (byte)((data[byteIndex] >> 5) & 0x03);
        }

    }

}
