using System;

namespace UDP_Server.Models
{
    public class DisplayInfo
    {
        #region [DisplayInfo] 모델

        public string Description { get; set; }

        public string MessageListen { get; set; }

        public DateTime CurrentTime { get; set; }

        public byte MessageByte { get; set; }

        public byte[] MessageBytes { get; set; }

        #endregion
    }

}
