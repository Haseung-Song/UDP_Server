using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using UDP_Server.Common;
using UDP_Server.Models;
using UDP_Server.Services;
using static UDP_Server.Models.Parser;

namespace UDP_Server.ViewModels
{
    public class MainVM : INotifyPropertyChanged
    {
        #region [프로퍼티]

        private UdpService _udpService;
        private string _ipAddress;
        private int _port;
        private DateTime _currentTime;
        private string _messageListen;
        private ObservableCollection<DisplayInfo> _displayInfo;

        #endregion

        #region [OnPropertyChanged]

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// [IpAddress]
        /// </summary>
        public string IpAddress
        {
            get => _ipAddress;
            set
            {
                if (_ipAddress != value)
                {
                    _ipAddress = value;
                    OnPropertyChanged();
                }

            }

        }

        /// <summary>
        /// [Port]
        /// </summary>
        public int Port
        {
            get => _port;
            set
            {
                if (_port != value)
                {
                    _port = value;
                    OnPropertyChanged();
                }

            }

        }

        /// <summary>
        /// [CurrentTime]
        /// </summary>
        public DateTime CurrentTime
        {
            get => _currentTime = DateTime.Now;
            set
            {
                if (_currentTime != value)
                {
                    _currentTime = value;
                    OnPropertyChanged();
                }

            }

        }

        /// <summary>
        /// [MessageReceive]
        /// </summary>
        public string MessageListen
        {
            get => _messageListen;
            set
            {
                if (_messageListen != value)
                {
                    _messageListen = value;
                    OnPropertyChanged();
                }

            }

        }

        /// <summary>
        /// [DeleteFolderInfo]
        /// </summary>
        public ObservableCollection<DisplayInfo> DisplayInfo
        {
            get => _displayInfo;
            set
            {
                if (_displayInfo != value)
                {
                    _displayInfo = value;
                    OnPropertyChanged();
                }

            }

        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region [ICommand]

        public ICommand StartServerCommand { get; set; }

        #endregion

        #region 생성자 (Initialize)

        public MainVM()
        {
            _ipAddress = IPAddress.Loopback.ToString();
            _port = 2000;
            _displayInfo = new ObservableCollection<DisplayInfo>();
            StartServerCommand = new RelayCommand(StartServer);
        }

        #endregion

        #region [버튼 및 기능]

        private void StartServer()
        {
            _udpService = new UdpService(Port);
            _udpService.MessageReceived += OnMessageReceived; // 이벤트 구독
            _udpService.UdpStart();
        }

        private void OnMessageReceived(byte[] messageListen, DateTime currentTime)
        {
            Parser parser = new Parser();
            FlightControlField parserData = parser.Parse(messageListen);
            // byte[]를 16진수 문자열로 변환
            string messageAsHex = BitConverter.ToString(messageListen).Replace("-", " ");
            Console.WriteLine($"Received: {messageAsHex}");
            DisplayInfo.Add(new DisplayInfo
            {
                MessageListen = parserData.ModeOverride,
                CurrentTime = currentTime
            });

        }
        #endregion
    }

}
