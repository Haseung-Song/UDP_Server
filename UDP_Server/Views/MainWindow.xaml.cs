using System.Windows;
using UDP_Server.ViewModels;

namespace UDP_Server
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainVM vm = new MainVM();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = vm;
        }

    }

}
