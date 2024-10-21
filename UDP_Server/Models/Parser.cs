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
            /// <summary>
            /// [Byte #5.] 7번째 비트
            /// </summary>
            public byte ModeOverride { get; set; }

            /// <summary>
            /// [Byte #5.] 6~5번째 비트
            /// </summary>
            public byte FlightMode { get; set; }

            /// <summary>
            /// [Byte #5.] 4~1번째 비트
            /// </summary>
            public byte ModeEngage { get; set; }

            /// <summary>
            /// [Byte #6.] 7번째 비트
            /// </summary>
            public byte FlapOverride { get; set; }

            /// <summary>
            /// [Byte #6.] 6~1번째 비트
            /// </summary>
            public byte FlapAngle { get; set; }
        }

    }

}
