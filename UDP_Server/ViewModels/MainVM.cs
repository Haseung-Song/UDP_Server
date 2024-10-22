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
            get => _currentTime;
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
            string messageAsHex = BitConverter.ToString(messageListen).Replace("-", " "); // byte[]를 16진수 문자열로 변환
            Console.WriteLine($"Received: {messageAsHex}");

            DisplayInfo?.Clear();
            DisplayInfo.Add(new DisplayInfo { Description = "Mode override", MessageListen = parserData.ModeOverride.ModeOverrideParser(), CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Flight mode", MessageListen = parserData.FlightMode.FlightModeParser(), CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Mode engage", MessageListen = parserData.ModeEngage.ModeEngageParser(), CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Flap Override", MessageListen = parserData.FlapOverride.FlapOverrideParser(), CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "플랩각 조종 명령", MessageListen = parserData.FlapAngle.FlapAngleParser(), CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Wing Tilt Override", MessageListen = parserData.FlapAngle.WingTiltOverrideParser(), CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "틸트각 조종 명령", MessageListen = parserData.TiltAngle.TiltAngleParser(), CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "노브 속도 조종명령", MessageListen = parserData.KnobSpeed.KnobSpeedParser(), CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "노브 고도 조종명령", MessageListen = parserData.KnobAltitude.KnobAltitudeParser(), CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "노브 방위 조종명령", MessageListen = parserData.KnobHeading.KnobHeadingParser(), CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "스틱 고도 조종명령", MessageListen = parserData.StickThrottle.StickThrottleParser(), CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "스틱 횡방향 속도 조종명령", MessageListen = parserData.StickRoll.StickRollParser(), CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "스틱 종방향 속도 조종명령", MessageListen = parserData.StickPitch.StickPitchParser(), CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "스틱 방위 조종명령", MessageListen = parserData.StickYaw.StickYawParser(), CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Longitude of Landing point", MessageListen = parserData.LonOfLP.LonOfLPParser(), CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Latitude of Landing point", MessageListen = parserData.LatOfLP.LatOfLPParser(), CurrentTime = currentTime }); ;
            DisplayInfo.Add(new DisplayInfo { Description = "Altitude of Landing point", MessageListen = parserData.AltOfLP.AltOfLPParser(), CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "Engine Start / Stop", MessageListen = parserData.EngineStartStop.EngineStartStopParser(), CurrentTime = currentTime });
            DisplayInfo.Add(new DisplayInfo { Description = "구조장비 투하 전 개폐명령", MessageListen = parserData.RaftDrop.RaftDropParser(), CurrentTime = currentTime });
        }
        #endregion
    }

}
