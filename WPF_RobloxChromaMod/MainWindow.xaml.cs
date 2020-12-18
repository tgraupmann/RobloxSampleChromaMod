using System;
using System.Threading;
using System.Windows;


namespace WPF_RobloxChromaMod
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool _mWaitForExit = true;

        public MainWindow()
        {
            InitializeComponent();

            ThreadStart ts = new ThreadStart(LogWorker);
            Thread thread = new Thread(ts);
            thread.Start();

            Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            _mWaitForExit = false;
        }

        private void LogWorker()
        {
            while (_mWaitForExit)
            {
                Thread.Sleep(100);
            }
        }
    }
}
