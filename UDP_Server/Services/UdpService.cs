using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace UDP_Server.Services
{
    /// <summary>
    /// [UdpService] 클래스 (분리)
    /// </summary>
    public class UdpService
    {
        public UdpClient _udpServer;
        public event Action<byte[], DateTime> MessageReceived; // 이벤트 정의

        public UdpService(int port)
        {
            _udpServer = new UdpClient(port);
        }

        /// <summary>
        /// [UdpStart()]
        /// [Udp 서버 시작]
        /// </summary>
        public async void UdpStart()
        {
            try
            {
                while (true)
                {
                    UdpReceiveResult result = await _udpServer.ReceiveAsync(); // 클라이언트 메시지 [수신] 부분
                    byte[] messageListen = result.Buffer;
                    MessageReceived?.Invoke(messageListen, DateTime.Now); // 이벤트 호출 (수신 Msg + 현재 Time)
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

        }

        /// <summary>
        /// [UdpStop()]
        /// [Udp 서버 종료]
        /// </summary>
        public void UdpStop()
        {
            if (_udpServer != null)
            {
                _udpServer.Close();
                _udpServer.Dispose();
            }

        }

    }

}
