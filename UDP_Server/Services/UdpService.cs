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
            catch (OperationCanceledException)
            {
                Debug.WriteLine("UDP 서버 데이터 수신 작업이 취소되었습니다.");
            }
            catch (ObjectDisposedException)
            {
                Debug.WriteLine("UDP 소켓이 닫혔습니다.");
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
                // [UDP 서버] 닫기!
                _udpServer.Close();
                // [서버 리소스] 해제
                _udpServer.Dispose();
            }

        }

    }

}
