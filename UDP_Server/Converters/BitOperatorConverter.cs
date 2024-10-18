namespace UDP_Server.Converters
{
    public class BitOperatorConverter
    {
        /// <summary>
        /// [GetBit(): 특정 바이트에서 원하는 비트를 추출하는 메서드]
        /// </summary>
        /// <param name="data"></param>
        /// <param name="byteIndex"></param>
        /// <param name="bitPosition"></param>
        /// <returns></returns>
        public static byte GetBit(byte[] data, int byteIndex, int bitPosition)
        {
            return (byte)((data[byteIndex] >> bitPosition) & 0x01);
        }

    }

}
