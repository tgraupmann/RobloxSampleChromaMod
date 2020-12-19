using System;
using System.Collections.Generic;
using System.IO;
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
            Dictionary<string, long> sizeMap = new Dictionary<string, long>();

            while (_mWaitForExit)
            {
                DirectoryInfo di = new DirectoryInfo(Environment.GetEnvironmentVariable("LocalAppData") + @"\Roblox\logs");
                if (!di.Exists)
                {
                    Thread.Sleep(5000); //wait for folder to exist
                    continue;
                }
                foreach (FileInfo fi in di.GetFiles("*.txt"))
                {
                    if (!sizeMap.ContainsKey(fi.FullName))
                    {
                        //new log
                        sizeMap[fi.FullName] = fi.Length;
                    }
                    else
                    {
                        long oldLength = sizeMap[fi.FullName];
                        if (fi.Length > oldLength)
                        {
                            ReadContent(fi, oldLength);
                        }
                        sizeMap[fi.FullName] = fi.Length;
                    }
                }
                Thread.Sleep(100);
            }
        }

        void ReadContent(FileInfo fi, long oldLength)
        {
            try
            {
                using (FileStream fs = File.Open(fi.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        fs.Seek(oldLength, SeekOrigin.Begin);
                        do
                        {
                            string content = sr.ReadLine();
                            if (content == null)
                            {
                                break;
                            }
                            const string TOKEN = "ChromaRGB: ";
                            int index = content.IndexOf(TOKEN);
                            if (index >= 0)
                            {
                                string effect = content.Substring(index + TOKEN.Length);
                                ProcessEffect(effect);
                            }
                        }
                        while (true);
                    }
                }
            }
            catch
            {
                Thread.Sleep(3000);
            }
        }

        void ProcessEffect(string effect)
        {
            if (string.IsNullOrEmpty(effect))
            {
                return;
            }
            switch (effect)
            {
                case "BtnEffect1":
                    break;
                case "BtnEffect2":
                    break;
                case "BtnEffect3":
                    break;
                case "BtnEffect4":
                    break;
                case "BtnEffect5":
                    break;
                case "BtnEffect6":
                    break;
                case "BtnEffect7":
                    break;
                case "BtnEffect8":
                    break;
                case "BtnEffect9":
                    break;
                case "BtnEffect10":
                    break;
                case "BtnEffect11":
                    break;
                case "BtnEffect12":
                    break;
                case "BtnEffect13":
                    break;
                case "BtnEffect14":
                    break;
                case "BtnEffect15":
                    break;
            }
        }
    }
}
