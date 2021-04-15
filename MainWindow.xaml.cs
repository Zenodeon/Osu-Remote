using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
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

namespace Osu_Remote
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Progress<HttpListenerContext> httpCallback = new Progress<HttpListenerContext>();

        public MainWindow()
        {
            DLog.Instantiate();

            InitializeComponent();

            httpCallback.ProgressChanged += HttpResponse;

            Listener(httpCallback);
        }

        private void OnClosingWindow(object sender, CancelEventArgs e)
        {
            DLog.Close();
        }

        private void Listener(IProgress<HttpListenerContext> progress)
        {
            Task.Run(() =>
            {
                HttpListener listener = new HttpListener();

                string prefixes = "http://+:8080/osu_test/";

                listener.Prefixes.Add(prefixes);
                listener.Start();

                HttpListenerContext context = listener.GetContext();

                progress.Report(context);
            });
        }

        private void HttpResponse(object sender, HttpListenerContext e)
        {
            DLog.Log("working");
        }
    }
}
