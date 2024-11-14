using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
        private ObservableCollection<DisplayInfo> _displayInfo;
        private bool _IsStartBtnEnabled;

        #endregion

        #region [OnPropertyChanged]

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// [InitStop_BtnText]
        /// </summary>
        public string InitStop_BtnText => IsStartBtnEnabled ? "Init Server" : "Stop Server";

        /// <summary>
        /// [IsStartBtnEnabled]
        /// </summary>
        public bool IsStartBtnEnabled
        {
            get => _IsStartBtnEnabled;
            private set
            {
                if (_IsStartBtnEnabled != value)
                {
                    _IsStartBtnEnabled = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(InitStop_BtnText));
                }

            }

        }

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

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region [ICommand]

        public ICommand InitAndStopServerCommand { get; set; }

        #endregion

        #region 생성자 (Initialize)

        public MainVM()
        {
            IsStartBtnEnabled = true;
            _ipAddress = IPAddress.Loopback.ToString();
            _port = 20000;
            _displayInfo = new ObservableCollection<DisplayInfo>();
            InitAndStopServerCommand = new RelayCommand(InitAndStop);
        }

        #endregion

        #region [버튼 및 기능]

        private void InitAndStop()
        {
            if (_udpService == null || IsStartBtnEnabled)
            {
                IsStartBtnEnabled = false;
                _udpService = new UdpService(Port);
                _udpService.MessageReceived += OnMessageReceived; // 이벤트 구독
                _udpService.UdpStart();
                Console.WriteLine("UDP Server Started...");
            }
            else
            {
                IsStartBtnEnabled = true;
                _udpService.MessageReceived -= OnMessageReceived; // 이벤트 해제
                _udpService.UdpStop();
                Console.WriteLine("UDP Server Stopped...");
            }

        }

        private void OnMessageReceived(byte[] messageListen, DateTime currentTime)
        {
            try
            {
                Parser parser = new Parser();
                FlightControlField parserData = parser.Parse(messageListen);
                // [UI 초기화] [작업]
                DisplayInfo?.Clear();
                if (parserData != null)
                {
                    DisplayInfo.Add(new DisplayInfo { Description = "Mode override", MessageListen = parserData.ModeOverride.ModeOverrideParser(), CurrentTime = currentTime, MessageByte = parserData.ModeOverride });
                    DisplayInfo.Add(new DisplayInfo { Description = "Flight mode", MessageListen = parserData.FlightMode.FlightModeParser(), CurrentTime = currentTime, MessageByte = parserData.FlightMode });
                    DisplayInfo.Add(new DisplayInfo { Description = "Mode engage", MessageListen = parserData.ModeEngage.ModeEngageParser(), CurrentTime = currentTime, MessageByte = parserData.ModeEngage });
                    DisplayInfo.Add(new DisplayInfo { Description = "Flap Override", MessageListen = parserData.FlapOverride.FlapOverrideParser(), CurrentTime = currentTime, MessageByte = parserData.FlapOverride });
                    DisplayInfo.Add(new DisplayInfo { Description = "플랩각 조종 명령", MessageListen = parserData.FlapAngle.FlapAngleParser(), CurrentTime = currentTime, MessageByte = parserData.FlapAngle });
                    DisplayInfo.Add(new DisplayInfo { Description = "Wing Tilt Override", MessageListen = parserData.WingTiltOverride.WingTiltOverrideParser(), CurrentTime = currentTime, MessageByte = parserData.WingTiltOverride });
                    DisplayInfo.Add(new DisplayInfo { Description = "틸트각 조종 명령", MessageListen = parserData.TiltAngle.TiltAngleParser(), CurrentTime = currentTime, MessageByte = parserData.TiltAngle });
                    DisplayInfo.Add(new DisplayInfo { Description = "노브 속도 조종명령", MessageListen = parserData.KnobSpeed.KnobSpeedParser(), CurrentTime = currentTime, MessageByte = parserData.KnobSpeed });
                    DisplayInfo.Add(new DisplayInfo { Description = "노브 고도 조종명령", MessageListen = parserData.KnobAltitude.KnobAltitudeParser(), CurrentTime = currentTime, MessageByte = parserData.KnobAltitude });
                    DisplayInfo.Add(new DisplayInfo { Description = "노브 방위 조종명령", MessageListen = parserData.KnobHeading.KnobHeadingParser(), CurrentTime = currentTime, MessageByte = parserData.KnobHeading });
                    DisplayInfo.Add(new DisplayInfo { Description = "스틱 고도 조종명령", MessageListen = parserData.StickThrottle.StickThrottleParser(), CurrentTime = currentTime, MessageByte = parserData.StickThrottle });
                    DisplayInfo.Add(new DisplayInfo { Description = "스틱 횡방향 속도 조종명령", MessageListen = parserData.StickRoll.StickRollParser(), CurrentTime = currentTime, MessageByte = parserData.StickRoll });
                    DisplayInfo.Add(new DisplayInfo { Description = "스틱 종방향 속도 조종명령", MessageListen = parserData.StickPitch.StickPitchParser(), CurrentTime = currentTime, MessageByte = parserData.StickPitch });
                    DisplayInfo.Add(new DisplayInfo { Description = "스틱 방위 조종명령", MessageListen = parserData.StickYaw.StickYawParser(), CurrentTime = currentTime, MessageByte = parserData.StickYaw });
                    DisplayInfo.Add(new DisplayInfo { Description = "Longitude of Landing point", MessageListen = parserData.LonOfLP.LonOfLPParser(), CurrentTime = currentTime, MessageBytes = parserData.LonOfLP });
                    DisplayInfo.Add(new DisplayInfo { Description = "Latitude of Landing point", MessageListen = parserData.LatOfLP.LatOfLPParser(), CurrentTime = currentTime, MessageBytes = parserData.LatOfLP });
                    DisplayInfo.Add(new DisplayInfo { Description = "Altitude of Landing point", MessageListen = parserData.AltOfLP.AltOfLPParser(), CurrentTime = currentTime, MessageBytes = parserData.AltOfLP });
                    DisplayInfo.Add(new DisplayInfo { Description = "Engine Start / Stop", MessageListen = parserData.EngineStartStop.EngineStartStopParser(), CurrentTime = currentTime, MessageByte = parserData.EngineStartStop });
                    DisplayInfo.Add(new DisplayInfo { Description = "구조장비 투하 전 개폐명령", MessageListen = parserData.RaftDrop.RaftDropParser(), CurrentTime = currentTime, MessageByte = parserData.RaftDrop });
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

        }
        #endregion
    }

}
