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
        private ObservableCollection<string> _description;

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
        /// [DisplayInfo]
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

        /// <summary>
        /// [DeleteFolderInfo]
        /// </summary>
        public ObservableCollection<string> Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
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
            DisplayInfo?.Clear();
            DisplayInfo.Add(new DisplayInfo { Description = "Mode override", MessageListen = parserData.ModeOverride, CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Flight mode", MessageListen = parserData.FlightMode, CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Mode engage", MessageListen = parserData.ModeEngage, CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Flap Override", MessageListen = parserData.FlapOverride, CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Flap angle", MessageListen = parserData.FlapAngle, CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Tilt angle", MessageListen = parserData.ModeEngage, CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Knob speed", MessageListen = parserData.ModeEngage, CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "knob altitude", MessageListen = parserData.ModeEngage, CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Knob heading", MessageListen = parserData.ModeEngage, CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Stick throttle", MessageListen = parserData.ModeEngage, CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Stick roll", MessageListen = parserData.ModeEngage, CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Stick pitch", MessageListen = parserData.ModeEngage, CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Stick yaw", MessageListen = parserData.ModeEngage, CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Lon. of LP", MessageListen = parserData.ModeEngage, CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Lat. of LP", MessageListen = parserData.ModeEngage, CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Alt. of LP", MessageListen = parserData.ModeEngage, CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Engine start/stop", MessageListen = parserData.ModeEngage, CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Raft drop", MessageListen = parserData.ModeEngage, CurrentTime = currentTime });
        }
        #endregion
    }

}
