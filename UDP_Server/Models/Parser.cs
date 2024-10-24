using System;
using Soletop.IO;

namespace UDP_Server.Models
{
    public class Parser
    {
        public FlightControlField Parse(byte[] data)
        {
            using (ByteStream stream = new ByteStream(data, 0, data.Length))
            {
                if (data.Length > 32)
                {
                    throw new ArgumentException("Invalid data length");
                }

                FlightControlField field = new FlightControlField
                {
                    // 1. [New 연산 계산 방식] => 직접 비트식연산 사용
                    //ModeOverride = BitOperatorConverter.GetBitFrom7(data, 4, 7),
                    //FlightMode = BitOperatorConverter.GetBitFrom6To5(data, 4),

                    // 2. [Old 연산 계산 방식] => 솔탑 프레임워크 사용
                    // # [GetBits] 메서드 #
                    // 바이트 스트림의 특정 위치에서 비트 추출 역할
                    // 첫 번째 인자: 시작 위치. (0 바이트부터 시작)
                    // 두 번째 인자: 추출할 바이트 수.
                    // 세 번째 인자: 비트 시작 위치.
                    // 네 번째 인자: 추출할 비트 수.

                    // [Byte #5.]
                    // 7    번째 비트를 추출
                    ModeOverride = (byte)stream.GetBits(4, 1, 7, 1),
                    // 6 ~ 5번째 비트를 추출
                    FlightMode = (byte)stream.GetBits(4, 1, 5, 2),
                    // 4 ~ 1번째 비트를 추출
                    ModeEngage = (byte)stream.GetBits(4, 1, 1, 4),

                    // [Byte #6.]
                    // 7    번째 비트를 추출
                    FlapOverride = (byte)stream.GetBits(5, 1, 7, 1),
                    // 6 ~ 1번째 비트를 추출
                    FlapAngle = (byte)stream.GetBits(5, 1, 1, 6),

                    // [Byte #7.]
                    // 7    번째 비트를 추출
                    WingTiltOverride = (byte)stream.GetBits(6, 1, 7, 1),
                    // 6 ~ 0번째 비트를 추출
                    TiltAngle = (byte)stream.GetBits(6, 1, 0, 7),

                    // [Byte #8.]
                    KnobSpeed = (byte)stream.Get(7, 1),
                    // [Byte #9.]
                    KnobAltitude = (byte)stream.Get(8, 1),
                    // [Byte #10.]
                    KnobHeading = (byte)stream.Get(9, 1),

                    // [Byte #11.]
                    StickThrottle = (byte)stream.Get(10, 1),
                    // [Byte #12.]
                    StickRoll = (byte)stream.Get(11, 1),
                    // [Byte #13.]
                    StickPitch = (byte)stream.Get(12, 1),
                    // [Byte #14.]
                    StickYaw = (byte)stream.Get(13, 1),

                    // [Byte #15. ~ Byte #18.]
                    LonOfLP = stream.GetBytes(14, 4),
                    // [Byte # 19. ~ Byte #22.]
                    LatOfLP = stream.GetBytes(18, 4),
                    // [Byte # 23. ~ Byte #24.]
                    AltOfLP = stream.GetBytes(22, 2),
                };
                return field;
            }

        }

        public class FlightControlField
        {
            /// <summary>
            /// [Mode override]
            /// [Byte #5.] 7번째 비트
            /// </summary>
            public byte ModeOverride { get; set; }

            /// <summary>
            /// [Flight mode]
            /// [Byte #5.] 6~5번째 비트
            /// </summary>
            public byte FlightMode { get; set; }

            /// <summary>
            /// [Mode engage]
            /// [Byte #5.] 4~1번째 비트
            /// </summary>
            public byte ModeEngage { get; set; }

            /// <summary>
            /// [Flap Override]
            /// [Byte #6.] 7번째 비트
            /// </summary>
            public byte FlapOverride { get; set; }

            /// <summary>
            /// [플랩각 조종 명령]
            /// [Byte #6.] 6~1번째 비트
            /// </summary>
            public byte FlapAngle { get; set; }

            /// <summary>
            /// [Wing Tilt Override]
            /// [Byte #7.] 7번째 비트
            /// </summary>
            public byte WingTiltOverride { get; set; }

            /// <summary>
            /// [틸트각 조종 명령]
            /// [Byte #7.] 6~0번째 비트
            /// </summary>
            public byte TiltAngle { get; set; }

            /// <summary>
            /// [노브 속도 조종명령]
            /// [Byte #8.]
            /// </summary>
            public byte KnobSpeed { get; set; }

            /// <summary>
            /// [노브 고도 조종명령]
            /// [Byte #9.]
            /// </summary>
            public byte KnobAltitude { get; set; }

            /// <summary>
            /// [노브 방위 조종명령]
            /// [Byte #10.]
            /// </summary>
            public byte KnobHeading { get; set; }

            /// <summary>
            /// [스틱 고도 조종명령]
            /// [Byte #11.]
            /// </summary>
            public byte StickThrottle { get; set; }

            /// <summary>
            /// [스틱 횡방향 속도 조종명령]
            /// [Byte #12.]
            /// </summary>
            public byte StickRoll { get; set; }

            /// <summary>
            /// [스틱 종방향 속도 조종명령]
            /// [Byte #13.]
            /// </summary>
            public byte StickPitch { get; set; }

            /// <summary>
            /// [스틱 방위 조종명령]
            /// [Byte #14.]
            /// </summary>
            public byte StickYaw { get; set; }

            /// <summary>
            /// [Longitude of Landing point]
            /// [Byte #15. ~ Byte #18.] = [4 Byte]
            /// </summary>
            public byte[] LonOfLP { get; set; }

            /// <summary>
            /// [Latitude of Landing point]
            /// [Byte #19. ~ Byte #22.] = [4 Byte]
            /// </summary>
            public byte[] LatOfLP { get; set; }

            /// <summary>
            /// [Altitude of Landing point]
            /// [Byte #23. ~ Byte #24.] = [2 Byte]
            /// </summary>
            public byte[] AltOfLP { get; set; }

            /// <summary>
            /// [Engine Start / Stop]
            /// [Byte #25.] 7번째 비트
            /// </summary>
            public byte EngineStartStop { get; set; }

            /// <summary>
            /// [구조장비 투하 전 개폐명령]
            /// [Byte #25.] 0번째 비트
            /// </summary>
            public byte RaftDrop { get; set; }
        }

    }

    public static class FlightControlFieldExtention
    {
        public static string ModeOverrideParser(this byte modeOverrideByte)
        {
            int modeOverrideByteToInt = modeOverrideByte;
            // [0x01, 0x00] => [ON, OFF] 변환 공식
            return modeOverrideByteToInt == 1 ? "ON(default)" : "OFF";
        }

        public static string FlightModeParser(this byte flightModeByte)
        {
            int flightModeByteToInt = flightModeByte;
            // [0x00, ~ 0x03] => [Preprogram(Default) ~ Manual(CAS)] 변환 공식
            switch (flightModeByteToInt)
            {
                case 0:
                    return "Preprogram(Default)";
                case 1:
                    return "WPT Navigation";
                case 2:
                    return "Knob";
                case 3:
                    return "Manual(CAS)";
                default:
                    return "Unknown";
            }

        }

        public static string ModeEngageParser(this byte modeEngageByte)
        {
            int modeEngageByteToInt = modeEngageByte;
            // [0x00 ~ 0x08] => [No action(default) ~ Mission reschedule] 변환 공식
            switch (modeEngageByteToInt)
            {
                case 0:
                    return "No action(default)";
                case 1:
                    return "Auto take off(Start preprogrammed mission)";
                case 2:
                    return "Transition_F2M(Fixed to multi)";
                case 3:
                    return "Transition_M2F(Multi to fixed)";
                case 4:
                    return "Jump to waypoint";
                case 5:
                    return "Return to base(=Auto landing)";
                case 6:
                    return "Start mission";
                case 7:
                    return "Hold current position";
                case 8:
                    return "Mission reschedule";
                default:
                    return "Unknown";
            }

        }

        public static string FlapOverrideParser(this byte flapOverrideByte)
        {
            int flapOverrideByteToInt = flapOverrideByte;
            // [0x01, 0x00] => [ON, OFF] 변환 공식
            return flapOverrideByteToInt == 1 ? "ON(default)" : "OFF";
        }

        public static string FlapAngleParser(this byte flapAngleByte)
        {
            int flapAngleByteToInt = flapAngleByte;
            // [0x00, 0x28] => [0, 40] 변환 공식, res = 2
            int flapAngleToInt = (flapAngleByteToInt * 2) - 40;
            return (flapAngleByteToInt <= 40) ? $"{flapAngleToInt}°(도)" : "Unknown";
        }

        public static string WingTiltOverrideParser(this byte wingTiltOverrideByte)
        {
            int wingTiltOverrideByteToInt = wingTiltOverrideByte;
            // [0x01, 0x00] => [ON, OFF] 변환 공식
            return wingTiltOverrideByteToInt == 1 ? "ON" : "OFF";
        }

        public static string TiltAngleParser(this byte tiltAngleByte)
        {
            int tiltAngleByteToInt = tiltAngleByte;
            // [0x00, 0x5A] => [0, 90] 변환 공식, res = 1
            return (tiltAngleByteToInt <= 90) ? $"{tiltAngleByte}°(도)" : "Unknown";
        }

        public static string KnobSpeedParser(this byte knobSpeedByte)
        {
            // 속도값 [1 Byte] To [uint]로 변환
            uint knobSpeedByteToUInt = knobSpeedByte;
            // [0x00, 0xFA] => [0, 250] 변환 공식, res = 1
            uint knobSpeedToUint = knobSpeedByteToUInt * 1;
            return (knobSpeedByteToUInt <= 250) ? $"{knobSpeedToUint} (km/h)" : "Unknown";
        }

        public static string KnobAltitudeParser(this byte knobAltitudeByte)
        {
            // 고도값 [1 Byte] To [uint]로 변환
            uint knobAltitudeByteToUInt = knobAltitudeByte;
            // [0x00, 0xC8] => [0, 200] 변환 공식, res = 15
            uint knobAltitudeToUint = knobAltitudeByteToUInt * 15;
            return (knobAltitudeByteToUInt <= 200) ? $"{knobAltitudeToUint} (m)" : "Unknown";
        }

        public static string KnobHeadingParser(this byte knobHeadingByte)
        {
            // 방위값 [1 Byte] To [uint]로 변환
            uint knobHeadingParserToUInt = knobHeadingByte;
            // [0x00, 0xB4] => [0, 360] 변환 공식, res = 2
            uint knobHeadingToUInt = knobHeadingParserToUInt * 2;
            return (knobHeadingParserToUInt <= 358) ? $"{knobHeadingToUInt}°(도)" : "Unknown";
        }

        public static string StickThrottleParser(this byte stickThrottleByte)
        {
            // 고도값 [1 Byte] To [uint]로 변환
            uint stickThrottleByteToUInt = stickThrottleByte;
            // [0x00, 0xC8] => [0, 1] 변환 공식, res = 0.005
            double stickThrottleToDouble = stickThrottleByteToUInt * 0.005;
            return (stickThrottleByteToUInt <= 200) ? $"{stickThrottleToDouble:F3}" : "Unknown";
        }

        public static string StickRollParser(this byte stickRollByte)
        {
            // 속도값 [1 Byte] To [uint]로 변환!
            uint stickRollByteToUInt = stickRollByte;
            // [0x00, 0xC8] => [-1, 1] 변환 공식, res = 0.01
            double stickRollToDouble = (stickRollByteToUInt * 0.01) - 1;
            return (stickRollByteToUInt <= 200) ? $"{stickRollToDouble:F2}" : "Unknown";
        }

        public static string StickPitchParser(this byte stickPitchByte)
        {
            // 속도값 [1 Byte] To [uint]로 변환!
            uint stickPitchByteToUInt = stickPitchByte;
            // [0x00, 0xC8] => [-1, 1] 변환 공식, res = 0.01
            double stickPitchToDouble = (stickPitchByteToUInt * 0.01) - 1;
            return (stickPitchByteToUInt <= 200) ? $"{stickPitchToDouble:F2}" : "Unknown";
        }

        public static string StickYawParser(this byte stickYawByte)
        {
            // 방위값 [1 Byte] To [uint]로 변환!
            uint stickYawByteToUInt = stickYawByte;
            // [0x00, 0xC8] => [-1, 1] 변환 공식, res = 0.01
            double stickYawToDouble = (stickYawByteToUInt * 0.01) - 1;
            return (stickYawByteToUInt <= 200) ? $"{stickYawToDouble:F2}" : "Unknown";
        }

        public static string LonOfLPParser(this byte[] lonOfLPByte)
        {
            if (lonOfLPByte.Length != 4)
            {
                return "Invalid Bytes!";
            }
            // 경도값 [4 Byte] To [uint]로 변환
            uint lonOfLPToInt = BitConverter.ToUInt32(lonOfLPByte, 0);
            // [0x00000000, 0xD693A400] => [-180, 180] 변환 공식, res = 0.0000001
            double lonOfLPInDegree = (lonOfLPToInt * 0.0000001) - 180.0;
            return (lonOfLPToInt <= 3600000000) ? $"{lonOfLPInDegree:F7}°(도)" : "Unknown";
        }

        public static string LatOfLPParser(this byte[] latOfLPByte)
        {
            if (latOfLPByte.Length != 4)
            {
                return "Invalid Bytes!";
            }
            // 위도값 [4 Byte] To [uInt]로 변환
            uint latOfLPToInt = BitConverter.ToUInt32(latOfLPByte, 0);
            // [0x00000000, 0x6B49D200] => [-90, 90] 변환 공식, res = 0.0000001
            double latOfLPInDegree = (latOfLPToInt * 0.0000001) - 90.0;
            return (latOfLPToInt <= 1800000000) ? $"{latOfLPInDegree:F7}°(도)" : "Unknown";
        }

        public static string AltOfLPParser(this byte[] altOfLPByte)
        {
            if (altOfLPByte.Length != 2)
            {
                return "Invalid Bytes!";
            }
            // 고도값 [2 Byte] To [ushort]로 변환
            ushort altOfLPToShort = BitConverter.ToUInt16(altOfLPByte, 0);
            // [0x0000, 0xEA60] => [-500, 1000] 변환 공식, res = 0.025
            double altOfLPInMeters = (altOfLPToShort * 0.025) - 500.0;
            return (altOfLPToShort <= 65535) ? $"{altOfLPInMeters:F2}°(m)" : "Unknown";
        }

        public static string EngineStartStopParser(this byte engineStartStopByte)
        {
            int engineStartStopByteToInt = engineStartStopByte;
            // [0x01, 0x00] => [ON, OFF] 변환 공식
            return engineStartStopByteToInt == 1 ? "ON" : "OFF";
        }

        public static string RaftDropParser(this byte raftDropByte)
        {
            int raftDropByteToInt = raftDropByte;
            // [0x01, 0x00] => [ON, OFF] 변환 공식
            return raftDropByteToInt == 1 ? "ON" : "OFF";
        }

    }

}
