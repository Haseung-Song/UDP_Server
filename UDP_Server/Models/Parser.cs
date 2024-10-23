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
                if (data.Length < 5 || data.Length > 31)
                {
                    throw new ArgumentException("Invalid data length");
                }

                FlightControlField field = new FlightControlField
                {
                    // 1. [연산 계산 추가 방식]
                    //ModeOverride = BitOperatorConverter.GetBitFrom7(data, 4, 7),
                    //FlightMode = BitOperatorConverter.GetBitFrom6To5(data, 4),

                    // 2. [기존 솔탑 프레임워크 연산 계산 방식]
                    // [GetBits] 메서드: 바이트 스트림의 특정 위치에서 비트를 추출하는 역할
                    // 첫 번째 인자: 시작 위치. (0 바이트부터 시작)
                    // 두 번째 인자: 추출할 바이트 수.
                    // 세 번째 인자: 비트 시작 위치.
                    // 네 번째 인자: 추출할 비트 수.

                    // Byte #5.
                    ModeOverride = (byte)stream.GetBits(4, 1, 7, 1), // 7번째 비트를 추출 (ModeOverride)
                    FlightMode = (byte)stream.GetBits(4, 1, 5, 2), // 6~5번째 비트를 추출 (FlightMode)
                    ModeEngage = (byte)stream.GetBits(4, 1, 1, 4), // 4~1번째 비트를 추출 (ModeEngage)

                    // Byte #6.
                    FlapOverride = (byte)stream.GetBits(5, 1, 7, 1), // 7번째 비트를 추출 (FlapOverride)
                    FlapAngle = (byte)stream.GetBits(5, 1, 1, 6), // 6~1번째 비트를 추출 (FlapAngle)

                    // Byte #7.
                    WingTiltOverride = (byte)stream.GetBits(6, 1, 7, 1), // 7번째 비트를 추출 (WingTiltOverride)
                    TiltAngle = (byte)stream.GetBits(6, 1, 0, 7), // 6~0번째 비트를 추출 (TiltAngle)
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
            public byte LonOfLP { get; set; }

            /// <summary>
            /// [Latitude of Landing point]
            /// [Byte #19. ~ Byte #22.] = [4 Byte]
            /// </summary>
            public byte LatOfLP { get; set; }

            /// <summary>
            /// [Altitude of Landing point]
            /// [Byte #23. ~ Byte #24.] = [2 Byte]
            /// </summary>
            public byte AltOfLP { get; set; }

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
            return modeOverrideByte == 1 ? "ON(default)" : "OFF";
        }

        public static string FlightModeParser(this byte flightModeByte)
        {
            switch (flightModeByte)
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
            switch (modeEngageByte)
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
            return flapOverrideByte == 1 ? "ON(default)" : "OFF";
        }

        public static string FlapAngleParser(this byte flapAngleByte)
        {
            return null;
        }

        public static string WingTiltOverrideParser(this byte wingTiltOverrideByte)
        {
            return wingTiltOverrideByte == 1 ? "ON(default)" : "OFF";
        }

        public static string TiltAngleParser(this byte tiltAngleByte)
        {
            return null;
        }

        public static string KnobSpeedParser(this byte knobSpeedByte)
        {
            return null;
        }

        public static string KnobAltitudeParser(this byte knobAltitudeByte)
        {
            return null;
        }

        public static string KnobHeadingParser(this byte knobHeadingByte)
        {
            return null;
        }

        public static string StickThrottleParser(this byte stickThrottleByte)
        {
            return null;
        }

        public static string StickRollParser(this byte stickRollByte)
        {
            return null;
        }

        public static string StickPitchParser(this byte stickPitchByte)
        {
            return null;
        }

        public static string StickYawParser(this byte stickYawByte)
        {
            return null;
        }

        public static string LonOfLPParser(this byte lonOfLPByte)
        {
            return null;
        }

        public static string LatOfLPParser(this byte latOfLPByte)
        {
            return null;
        }

        public static string AltOfLPParser(this byte altOfLPByte)
        {
            return null;
        }

        public static string EngineStartStopParser(this byte engineStartStopByte)
        {
            return null;
        }

        public static string RaftDropParser(this byte raftDropByte)
        {
            return null;
        }

    }

}
