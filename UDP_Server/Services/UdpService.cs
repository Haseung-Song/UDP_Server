using System;
using System.Diagnostics;
using System.Net.Sockets;
using System.Windows;

namespace UDP_Server.Services
{
    /// <summary>
    /// [UdpService] 클래스 (분리)
    /// </summary>
    public class UdpService
    {
        private UdpClient _udpServer;
        private static int _port;
        public event Action<byte[], DateTime> MessageReceived; // 이벤트 정의

        public UdpService(int port)
        {
            _udpServer = new UdpClient(port);
            _port = port;
        }

        public async void UdpStart()
        {
            try
            {
                Console.WriteLine("UDP Server Started...");
                _ = MessageBox.Show("UDP 서버 통신을 시작합니다!", "서버 시작", MessageBoxButton.OK, MessageBoxImage.Information);

                if (_udpServer == null)
                {
                    _udpServer = new UdpClient(_port);
                }

                while (true)
                {
                    // 1. 클라이언트 메시지 [수신] 부분
                    UdpReceiveResult result = await _udpServer.ReceiveAsync();
                    byte[] messageListen = result.Buffer;

                    // 2. 서버 메시지 [송신] 부분
                    //byte[] byteMessage = Encoding.UTF8.GetBytes(_messageSend);
                    //_ = await _udpServer.SendAsync(byteMessage, byteMessage.Length, _ipAddress, _port);

                    // 이벤트 발생 (현재 시간 + 수신 메시지)
                    MessageReceived?.Invoke(messageListen, DateTime.Now);
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
            finally
            {
                _udpServer.Close();
                _udpServer.Dispose();
            }

        }

    }

}
