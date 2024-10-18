using System;
using UDP_Server.Converters;

namespace UDP_Server.Models
{
    public class Parser
    {
        public FlightControlField Parse(byte[] data)
        {
            if (data.Length < 5 || data.Length > 31)
            {
                throw new ArgumentException("Invalid data length");
            }

            FlightControlField field = new FlightControlField
            {
                ModeOverride = BitOperatorConverter.GetBit(data, 4, 7),
            };
            return field;
        }

        public class FlightControlField
        {
            // 7번째 비트
            public byte ModeOverride { get; set; }

            // 6~5번째 비트
            public byte FlightMode { get; set; }

            // 4~1번째 비트
            public byte ModeEngage { get; set; }
        }

    }

}
