namespace UDP_Server.Models
{
    public class Parser
    {
        public FlightControlField Parse(byte[] data)
        {
            if (data.Length < 5 && data.Length > 31)
            {
                return null;
            }

            FlightControlField field = new FlightControlField
            {
                FlightMode = data[0],
                ModeEngage = data[1],
            };
            return field;
        }

        public class FlightControlField
        {
            public byte FlightMode { get; set; }

            public byte ModeEngage { get; set; }

            public byte FlapAngle { get; set; }

            public byte TiltAngle { get; set; }

            public byte KnobSpeed { get; set; }

            public byte KnobAltitude { get; set; }

            public byte KnobHeading { get; set; }

            public byte StickThrottle { get; set; }

            public byte StickRoll { get; set; }

            public byte StickPitch { get; set; }

            public byte StickYaw { get; set; }

            public byte LonOfLP { get; set; }

            public byte LatOfLP { get; set; }

            public byte AltOfLP { get; set; }

            public byte EngineStartStop { get; set; }

            public byte RaftDrop { get; set; }
        }

    }

}
