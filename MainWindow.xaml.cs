using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DebugLogger;
using OsuRemote.Internal;

namespace OsuRemote
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OsuRmtHandler osuRmtHandler = new OsuRmtHandler();

        public MainWindow()
        {
            DLog.Instantiate();

            InitializeComponent();

            osuRmtHandler.Initialize();
        }

        private void OnClosingWindow(object sender, CancelEventArgs e)
        {
            osuRmtHandler.Close();
            DLog.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            osuRmtHandler.StopListening();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            osuRmtHandler.ListenForInput();
        }
       
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            /*
            var endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 60);
            var client = new UdpClient();

            string data = "bruh";

            client.Connect(endPoint);

            client.Send(Encoding.ASCII.GetBytes(data), data.Length); 

            client.Close();
            */
        }
        

        private void SKey(object sender, RoutedEventArgs e)
        {
        }
    }
}