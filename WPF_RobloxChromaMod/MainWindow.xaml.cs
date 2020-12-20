using ChromaSDK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using static ChromaSDK.ChromaAnimationAPI;

namespace WPF_RobloxChromaMod
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool _mWaitForExit = true;

        StringBuilder _mStringBuilder = null;

        DispatcherTimer _mTimer = null;

        public MainWindow()
        {
            InitializeComponent();

            _mStringBuilder = new StringBuilder();

            int _mResult = ChromaAnimationAPI.Init();
            switch (_mResult)
            {
                case RazerErrors.RZRESULT_DLL_NOT_FOUND:
                    _mTextStatus.Text = string.Format("Chroma DLL is not found! {0}", RazerErrors.GetResultString(_mResult));
                    return;
                case RazerErrors.RZRESULT_DLL_INVALID_SIGNATURE:
                    _mTextStatus.Text = string.Format("Chroma DLL has an invalid signature! {0}", RazerErrors.GetResultString(_mResult));
                    return;
                case RazerErrors.RZRESULT_SUCCESS:
                    _mStringBuilder.AppendLine("Chroma RGB Initialized.");
                    Thread.Sleep(100);
                    SetupIdleAnimations();
                    break;
                default:
                    _mTextStatus.Text = string.Format("Failed to initialize Chroma! {0}", RazerErrors.GetResultString(_mResult));
                    return;
            }

            _mTextStatus.Text = _mStringBuilder.ToString();

            ThreadStart ts = new ThreadStart(LogWorker);
            Thread thread = new Thread(ts);
            thread.Start();

            _mTimer = new DispatcherTimer();
            _mTimer.Interval = TimeSpan.FromSeconds(1);
            _mTimer.Tick += Timer_Tick;
            _mTimer.Start();


            Closed += MainWindow_Closed;
        }


        void SetupHotkeys(string layer)
        {
            int[] keys = {
                (int)Keyboard.RZKEY.RZKEY_W,
                (int)Keyboard.RZKEY.RZKEY_A,
                (int)Keyboard.RZKEY.RZKEY_S,
                (int)Keyboard.RZKEY.RZKEY_D,
                (int)Keyboard.RZKEY.RZKEY_SPACE,
                };
            int color = ChromaAnimationAPI.GetRGB(0, 255, 0);
            ChromaAnimationAPI.SetKeysColorAllFramesName(layer, keys, keys.Length, color);
        }


        private void SetupIdleAnimation(string layer)
        {
            SetupHotkeys(layer);
            ChromaAnimationAPI.SetIdleAnimationName(layer);
        }

        private void SetupIdleAnimations()
        {
            // Enable Idle Animations
            ChromaAnimationAPI.UseIdleAnimation((int)Devices.ChromaLink, true);
            ChromaAnimationAPI.UseIdleAnimation((int)Devices.Headset, true);
            ChromaAnimationAPI.UseIdleAnimation((int)Devices.Keyboard, true);
            ChromaAnimationAPI.UseIdleAnimation((int)Devices.Keypad, true);
            ChromaAnimationAPI.UseIdleAnimation((int)Devices.Mouse, true);
            ChromaAnimationAPI.UseIdleAnimation((int)Devices.Mousepad, true);

            SetupIdleAnimation("Animations/Idle_ChromaLink.chroma");
            SetupIdleAnimation("Animations/Idle_Headset.chroma");
            SetupIdleAnimation("Animations/Idle_Keyboard.chroma");
            SetupIdleAnimation("Animations/Idle_Keypad.chroma");
            SetupIdleAnimation("Animations/Idle_Mouse.chroma");
            SetupIdleAnimation("Animations/Idle_Mousepad.chroma");
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (_mStringBuilder.Length > 512)
            {
                _mStringBuilder.Remove(0, 512);
            }
            _mTextStatus.Text = _mStringBuilder.ToString();
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            _mWaitForExit = false;
            _mTimer.Tick -= Timer_Tick;
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
                        if (fi.Length >= oldLength)
                        {
                            ReadContent(fi, oldLength);
                        }
                        sizeMap[fi.FullName] = fi.Length;
                    }
                }
                Thread.Sleep(100);
            }

            // Disable Idle Animations
            ChromaAnimationAPI.UseIdleAnimation((int)Devices.ChromaLink, false);
            ChromaAnimationAPI.UseIdleAnimation((int)Devices.Headset, false);
            ChromaAnimationAPI.UseIdleAnimation((int)Devices.Keyboard, false);
            ChromaAnimationAPI.UseIdleAnimation((int)Devices.Keypad, false);
            ChromaAnimationAPI.UseIdleAnimation((int)Devices.Mouse, false);
            ChromaAnimationAPI.UseIdleAnimation((int)Devices.Mousepad, false);

            ChromaAnimationAPI.StopAll();
            ChromaAnimationAPI.CloseAll();
            ChromaAnimationAPI.Uninit();
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


        #region Player State

        private Dictionary<string, bool> _mPlayerState = new Dictionary<string, bool>();

        private bool GetPlayerState(string state)
        {
            if (!_mPlayerState.ContainsKey(state))
            {
                return false;
            }
            return _mPlayerState[state];
        }

        private void SetPlayerState(string state, bool flag)
        {
            _mPlayerState[state] = flag;
        }

        #endregion


        void ProcessEffect(string effect)
        {
            if (string.IsNullOrEmpty(effect))
            {
                return;
            }
            _mStringBuilder.AppendLine(effect);
            switch (effect)
            {
                case "BtnEffect1":
                    ShowEffect1ChromaLink();
                    ShowEffect1Headset();
                    ShowEffect1Keyboard();
                    ShowEffect1Keypad();
                    ShowEffect1Mousepad();
                    ShowEffect1Mouse();
                    break;
                case "BtnEffect2":
                    ShowEffect2ChromaLink();
                    ShowEffect2Headset();
                    ShowEffect2Keyboard();
                    ShowEffect2Keypad();
                    ShowEffect2Mousepad();
                    ShowEffect2Mouse();
                    break;
                case "BtnEffect3":
                    ShowEffect3ChromaLink();
                    ShowEffect3Headset();
                    ShowEffect3Keyboard();
                    ShowEffect3Keypad();
                    ShowEffect3Mousepad();
                    ShowEffect3Mouse();
                    break;
                case "BtnEffect4":
                    ShowEffect4ChromaLink();
                    ShowEffect4Headset();
                    ShowEffect4Keyboard();
                    ShowEffect4Keypad();
                    ShowEffect4Mousepad();
                    ShowEffect4Mouse();
                    break;
                case "BtnEffect5":
                    ShowEffect5ChromaLink();
                    ShowEffect5Headset();
                    ShowEffect5Keyboard();
                    ShowEffect5Keypad();
                    ShowEffect5Mousepad();
                    ShowEffect5Mouse();
                    break;
                case "BtnEffect6":
                    ShowEffect6ChromaLink();
                    ShowEffect6Headset();
                    ShowEffect6Keyboard();
                    ShowEffect6Keypad();
                    ShowEffect6Mousepad();
                    ShowEffect6Mouse();
                    break;
                case "BtnEffect7":
                    ShowEffect7ChromaLink();
                    ShowEffect7Headset();
                    ShowEffect7Keyboard();
                    ShowEffect7Keypad();
                    ShowEffect7Mousepad();
                    ShowEffect7Mouse();
                    break;
                case "BtnEffect8":
                    ShowEffect8ChromaLink();
                    ShowEffect8Headset();
                    ShowEffect8Keyboard();
                    ShowEffect8Keypad();
                    ShowEffect8Mousepad();
                    ShowEffect8Mouse();
                    break;
                case "BtnEffect9":
                    ShowEffect9ChromaLink();
                    ShowEffect9Headset();
                    ShowEffect9Keyboard();
                    ShowEffect9Keypad();
                    ShowEffect9Mousepad();
                    ShowEffect9Mouse();
                    break;
                case "BtnEffect10":
                    ShowEffect10ChromaLink();
                    ShowEffect10Headset();
                    ShowEffect10Keyboard();
                    ShowEffect10Keypad();
                    ShowEffect10Mousepad();
                    ShowEffect10Mouse();
                    break;
                case "BtnEffect11":
                    ShowEffect11ChromaLink();
                    ShowEffect11Headset();
                    ShowEffect11Keyboard();
                    ShowEffect11Keypad();
                    ShowEffect11Mousepad();
                    ShowEffect11Mouse();
                    break;
                case "BtnEffect12":
                    ShowEffect12ChromaLink();
                    ShowEffect12Headset();
                    ShowEffect12Keyboard();
                    ShowEffect12Keypad();
                    ShowEffect12Mousepad();
                    ShowEffect12Mouse();
                    break;
                case "BtnEffect13":
                    ShowEffect13ChromaLink();
                    ShowEffect13Headset();
                    ShowEffect13Keyboard();
                    ShowEffect13Keypad();
                    ShowEffect13Mousepad();
                    ShowEffect13Mouse();
                    break;
                case "BtnEffect14":
                    ShowEffect14ChromaLink();
                    ShowEffect14Headset();
                    ShowEffect14Keyboard();
                    ShowEffect14Keypad();
                    ShowEffect14Mousepad();
                    ShowEffect14Mouse();
                    break;
                case "BtnEffect15":
                    ShowEffect15ChromaLink();
                    ShowEffect15Headset();
                    ShowEffect15Keyboard();
                    ShowEffect15Keypad();
                    ShowEffect15Mousepad();
                    ShowEffect15Mouse();
                    break;

                //events
                case "Player_Climbing":
                    ShowClimbingChromaLink();
                    ShowClimbingHeadset();
                    ShowClimbingKeyboard();
                    ShowClimbingKeypad();
                    ShowClimbingMousepad();
                    ShowClimbingMouse();
                    break;
                case "Player_Dead":
                    ShowEffect5ChromaLink();
                    ShowEffect5Headset();
                    ShowEffect5Keyboard();
                    ShowEffect5Keypad();
                    ShowEffect5Mouse();
                    ShowEffect5Mousepad();
                    break;
                case "Player_Flying":
                    ShowEffect7ChromaLink();
                    ShowEffect7Headset();
                    ShowEffect7Keyboard();
                    ShowEffect7Keypad();
                    ShowEffect7Mouse();
                    ShowEffect7Mousepad();
                    break;
                case "Player_Jumping":
                    if (!GetPlayerState("Jumping"))
                    {
                        SetPlayerState("Jumping", true);
                        ShowJumpingChromaLink();
                        ShowJumpingHeadset();
                        ShowJumpingKeyboard();
                        ShowJumpingKeypad();
                        ShowJumpingMousepad();
                        ShowJumpingMouse();
                    }
                    break;
                case "Player_Landed":
                    SetPlayerState("Jumping", false);
                    break;
                case "Player_Running":
                    /*
                    ShowEffect11ChromaLink();
                    ShowEffect11Headset();
                    ShowEffect11Keyboard();
                    ShowEffect11Keypad();
                    ShowEffect11Mouse();
                    ShowEffect11Mousepad();
                    */
                    break;
                case "Player_Seated":
                    ShowEffect3ChromaLink();
                    ShowEffect3Headset();
                    ShowEffect3Keyboard();
                    ShowEffect3Keypad();
                    ShowEffect3Mouse();
                    ShowEffect3Mousepad();
                    break;
                case "Player_Swimming":
                    ShowEffect4ChromaLink();
                    ShowEffect4Headset();
                    ShowEffect4Keyboard();
                    ShowEffect4Keypad();
                    ShowEffect4Mouse();
                    ShowEffect4Mousepad();
                    break;
            }
        }

        void SetupEvent(string layer, int index)
        {
            ChromaAnimationAPI.PlayAnimationName(layer, false);
        }

        #region Autogenerated
        void ShowEffect1Keyboard()
        {
            string baseLayer = "Animations/Blank_Keyboard.chroma";
            string layer2 = "Animations/Title_Keyboard.chroma";
            string layer3 = "Animations/BlackAndWhiteRainbow_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.CloseAnimationName(layer2);
            ChromaAnimationAPI.CloseAnimationName(layer3);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.GetAnimation(layer2);
            ChromaAnimationAPI.GetAnimation(layer3);
            ChromaAnimationAPI.ReduceFramesName(layer2, 2);
            int frameCount = ChromaAnimationAPI.GetFrameCountName(layer2);
            ChromaAnimationAPI.MakeBlankFramesName(baseLayer, frameCount, 0.1f, 0);
            int color1 = ChromaAnimationAPI.GetRGB(0, 255, 255);
            int color2 = ChromaAnimationAPI.GetRGB(255, 255, 255);
            ChromaAnimationAPI.MultiplyTargetColorLerpAllFramesName(layer3, color1, color2);
            ChromaAnimationAPI.CopyNonZeroTargetAllKeysAllFramesName(layer3, layer2);
            ChromaAnimationAPI.CopyNonZeroAllKeysAllFramesName(layer2, baseLayer);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.FillZeroColorAllFramesRGBName(baseLayer, 32, 0, 32);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.SetChromaCustomColorAllFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 1);
        }
        void ShowEffect1ChromaLink()
        {
            string baseLayer = "Animations/BlackAndWhiteRainbow_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            int color1 = ChromaAnimationAPI.GetRGB(0, 255, 255);
            int color2 = ChromaAnimationAPI.GetRGB(255, 255, 255);
            ChromaAnimationAPI.MultiplyTargetColorLerpAllFramesName(baseLayer, color1, color2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 1);
        }
        void ShowEffect1Headset()
        {
            string baseLayer = "Animations/BlackAndWhiteRainbow_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            int color1 = ChromaAnimationAPI.GetRGB(0, 255, 255);
            int color2 = ChromaAnimationAPI.GetRGB(255, 255, 255);
            ChromaAnimationAPI.MultiplyTargetColorLerpAllFramesName(baseLayer, color1, color2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 1);
        }
        void ShowEffect1Mousepad()
        {
            string baseLayer = "Animations/BlackAndWhiteRainbow_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            int color1 = ChromaAnimationAPI.GetRGB(0, 255, 255);
            int color2 = ChromaAnimationAPI.GetRGB(255, 255, 255);
            ChromaAnimationAPI.MultiplyTargetColorLerpAllFramesName(baseLayer, color1, color2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 1);
        }
        void ShowEffect1Mouse()
        {
            string baseLayer = "Animations/BlackAndWhiteRainbow_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            int color1 = ChromaAnimationAPI.GetRGB(0, 255, 255);
            int color2 = ChromaAnimationAPI.GetRGB(255, 255, 255);
            ChromaAnimationAPI.MultiplyTargetColorLerpAllFramesName(baseLayer, color1, color2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 1);
        }
        void ShowEffect1Keypad()
        {
            string baseLayer = "Animations/BlackAndWhiteRainbow_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            int color1 = ChromaAnimationAPI.GetRGB(0, 255, 255);
            int color2 = ChromaAnimationAPI.GetRGB(255, 255, 255);
            ChromaAnimationAPI.MultiplyTargetColorLerpAllFramesName(baseLayer, color1, color2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 1);
        }
        void ShowEffect2Keyboard()
        {
            string baseLayer = "Animations/Effect2_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.SetChromaCustomColorAllFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 2);
        }
        void ShowEffect2ChromaLink()
        {
            string baseLayer = "Animations/Effect2_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 2);
        }
        void ShowEffect2Headset()
        {
            string baseLayer = "Animations/Effect2_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 2);
        }
        void ShowEffect2Mousepad()
        {
            string baseLayer = "Animations/Effect2_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 2);
        }
        void ShowEffect2Mouse()
        {
            string baseLayer = "Animations/Effect2_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 2);
        }
        void ShowEffect2Keypad()
        {
            string baseLayer = "Animations/Effect2_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 2);
        }
        void ShowEffect3Keyboard()
        {
            string baseLayer = "Animations/Effect3_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.SetChromaCustomColorAllFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 3);
        }
        void ShowEffect3ChromaLink()
        {
            string baseLayer = "Animations/Effect3_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 3);
        }
        void ShowEffect3Headset()
        {
            string baseLayer = "Animations/Effect3_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 3);
        }
        void ShowEffect3Mousepad()
        {
            string baseLayer = "Animations/Effect3_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 3);
        }
        void ShowEffect3Mouse()
        {
            string baseLayer = "Animations/Effect3_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 3);
        }
        void ShowEffect3Keypad()
        {
            string baseLayer = "Animations/Effect3_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 3);
        }
        void ShowEffect4Keyboard()
        {
            string baseLayer = "Animations/Effect4_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.SetChromaCustomColorAllFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 4);
        }
        void ShowEffect4ChromaLink()
        {
            string baseLayer = "Animations/Effect4_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 4);
        }
        void ShowEffect4Headset()
        {
            string baseLayer = "Animations/Effect4_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 4);
        }
        void ShowEffect4Mousepad()
        {
            string baseLayer = "Animations/Effect4_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 4);
        }
        void ShowEffect4Mouse()
        {
            string baseLayer = "Animations/Effect4_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 4);
        }
        void ShowEffect4Keypad()
        {
            string baseLayer = "Animations/Effect4_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 4);
        }
        void ShowEffect5Keyboard()
        {
            string baseLayer = "Animations/Effect5_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.SetChromaCustomColorAllFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 5);
        }
        void ShowEffect5ChromaLink()
        {
            string baseLayer = "Animations/Effect5_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 5);
        }
        void ShowEffect5Headset()
        {
            string baseLayer = "Animations/Effect5_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 5);
        }
        void ShowEffect5Mousepad()
        {
            string baseLayer = "Animations/Effect5_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 5);
        }
        void ShowEffect5Mouse()
        {
            string baseLayer = "Animations/Effect5_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 5);
        }
        void ShowEffect5Keypad()
        {
            string baseLayer = "Animations/Effect5_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 5);
        }
        void ShowEffect6Keyboard()
        {
            string baseLayer = "Animations/Effect6_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            int color = ChromaAnimationAPI.GetRGB(182, 133, 255);
            ChromaAnimationAPI.MultiplyIntensityColorAllFramesName(baseLayer, color);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.SetChromaCustomColorAllFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 6);
        }
        void ShowEffect6ChromaLink()
        {
            string baseLayer = "Animations/Effect6_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            int color = ChromaAnimationAPI.GetRGB(182, 133, 255);
            ChromaAnimationAPI.MultiplyIntensityColorAllFramesName(baseLayer, color);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 6);
        }
        void ShowEffect6Headset()
        {
            string baseLayer = "Animations/Effect6_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            int color = ChromaAnimationAPI.GetRGB(182, 133, 255);
            ChromaAnimationAPI.MultiplyIntensityColorAllFramesName(baseLayer, color);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 6);
        }
        void ShowEffect6Mousepad()
        {
            string baseLayer = "Animations/Effect6_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            int color = ChromaAnimationAPI.GetRGB(182, 133, 255);
            ChromaAnimationAPI.MultiplyIntensityColorAllFramesName(baseLayer, color);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 6);
        }
        void ShowEffect6Mouse()
        {
            string baseLayer = "Animations/Effect6_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            int color = ChromaAnimationAPI.GetRGB(182, 133, 255);
            ChromaAnimationAPI.MultiplyIntensityColorAllFramesName(baseLayer, color);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 6);
        }
        void ShowEffect6Keypad()
        {
            string baseLayer = "Animations/Effect6_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            int color = ChromaAnimationAPI.GetRGB(182, 133, 255);
            ChromaAnimationAPI.MultiplyIntensityColorAllFramesName(baseLayer, color);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 6);
        }
        void ShowEffect7Keyboard()
        {
            string baseLayer = "Animations/Effect7_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.SetChromaCustomColorAllFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 7);
        }
        void ShowEffect7ChromaLink()
        {
            string baseLayer = "Animations/Effect7_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 7);
        }
        void ShowEffect7Headset()
        {
            string baseLayer = "Animations/Effect7_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 7);
        }
        void ShowEffect7Mousepad()
        {
            string baseLayer = "Animations/Effect7_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 7);
        }
        void ShowEffect7Mouse()
        {
            string baseLayer = "Animations/Effect7_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 7);
        }
        void ShowEffect7Keypad()
        {
            string baseLayer = "Animations/Effect7_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 7);
        }
        void ShowEffect8Keyboard()
        {
            string baseLayer = "Animations/Effect8_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.SetChromaCustomColorAllFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 8);
        }
        void ShowEffect8ChromaLink()
        {
            string baseLayer = "Animations/Effect8_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 8);
        }
        void ShowEffect8Headset()
        {
            string baseLayer = "Animations/Effect8_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            SetupEvent(baseLayer, 8);
        }
        void ShowEffect8Mousepad()
        {
            string baseLayer = "Animations/Effect8_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 8);
        }
        void ShowEffect8Mouse()
        {
            string baseLayer = "Animations/Effect8_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 8);
        }
        void ShowEffect8Keypad()
        {
            string baseLayer = "Animations/Effect8_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 8);
        }
        void ShowEffect9Keyboard()
        {
            string baseLayer = "Animations/Effect9_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.SetChromaCustomColorAllFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 9);
        }
        void ShowEffect9ChromaLink()
        {
            string baseLayer = "Animations/Effect9_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 9);
        }
        void ShowEffect9Headset()
        {
            string baseLayer = "Animations/Effect9_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 9);
        }
        void ShowEffect9Mousepad()
        {
            string baseLayer = "Animations/Effect9_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 9);
        }
        void ShowEffect9Mouse()
        {
            string baseLayer = "Animations/Effect9_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 9);
        }
        void ShowEffect9Keypad()
        {
            string baseLayer = "Animations/Effect9_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 9);
        }
        void ShowEffect10Keyboard()
        {
            string baseLayer = "Animations/Effect10_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.SetChromaCustomColorAllFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 10);
        }
        void ShowEffect10ChromaLink()
        {
            string baseLayer = "Animations/Effect10_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 10);
        }
        void ShowEffect10Headset()
        {
            string baseLayer = "Animations/Effect10_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 10);
        }
        void ShowEffect10Mousepad()
        {
            string baseLayer = "Animations/Effect10_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 10);
        }
        void ShowEffect10Mouse()
        {
            string baseLayer = "Animations/Effect10_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 10);
        }
        void ShowEffect10Keypad()
        {
            string baseLayer = "Animations/Effect10_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 10);
        }
        void ShowEffect11Keyboard()
        {
            string baseLayer = "Animations/Effect11_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            int color1 = ChromaAnimationAPI.GetRGB(69, 12, 69);
            int color2 = ChromaAnimationAPI.GetRGB(255, 255, 0);
            ChromaAnimationAPI.MultiplyTargetColorLerpAllFramesName(baseLayer, color1, color2);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.SetChromaCustomColorAllFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 11);
        }
        void ShowEffect11ChromaLink()
        {
            string baseLayer = "Animations/Effect11_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            int color1 = ChromaAnimationAPI.GetRGB(69, 12, 69);
            int color2 = ChromaAnimationAPI.GetRGB(255, 255, 0);
            ChromaAnimationAPI.MultiplyTargetColorLerpAllFramesName(baseLayer, color1, color2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 11);
        }
        void ShowEffect11Headset()
        {
            string baseLayer = "Animations/Effect11_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            int color1 = ChromaAnimationAPI.GetRGB(69, 12, 69);
            int color2 = ChromaAnimationAPI.GetRGB(255, 255, 0);
            ChromaAnimationAPI.MultiplyTargetColorLerpAllFramesName(baseLayer, color1, color2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 11);
        }
        void ShowEffect11Mousepad()
        {
            string baseLayer = "Animations/Effect11_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            int color1 = ChromaAnimationAPI.GetRGB(69, 12, 69);
            int color2 = ChromaAnimationAPI.GetRGB(255, 255, 0);
            ChromaAnimationAPI.MultiplyTargetColorLerpAllFramesName(baseLayer, color1, color2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 11);
        }
        void ShowEffect11Mouse()
        {
            string baseLayer = "Animations/Effect11_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            int color1 = ChromaAnimationAPI.GetRGB(69, 12, 69);
            int color2 = ChromaAnimationAPI.GetRGB(255, 255, 0);
            ChromaAnimationAPI.MultiplyTargetColorLerpAllFramesName(baseLayer, color1, color2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 11);
        }
        void ShowEffect11Keypad()
        {
            string baseLayer = "Animations/Effect11_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            int color1 = ChromaAnimationAPI.GetRGB(69, 12, 69);
            int color2 = ChromaAnimationAPI.GetRGB(255, 255, 0);
            ChromaAnimationAPI.MultiplyTargetColorLerpAllFramesName(baseLayer, color1, color2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 11);
        }
        void ShowEffect12Keyboard()
        {
            string baseLayer = "Animations/Effect12_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.SetChromaCustomColorAllFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 12);
        }
        void ShowEffect12ChromaLink()
        {
            string baseLayer = "Animations/Effect12_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 12);
        }
        void ShowEffect12Headset()
        {
            string baseLayer = "Animations/Effect12_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 12);
        }
        void ShowEffect12Mousepad()
        {
            string baseLayer = "Animations/Effect12_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 12);
        }
        void ShowEffect12Mouse()
        {
            string baseLayer = "Animations/Effect12_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 12);
        }
        void ShowEffect12Keypad()
        {
            string baseLayer = "Animations/Effect12_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 12);
        }
        void ShowEffect13Keyboard()
        {
            string baseLayer = "Animations/Effect13_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.DuplicateFramesName(baseLayer);
            int color1 = ChromaAnimationAPI.GetRGB(0, 0, 0);
            int color2 = ChromaAnimationAPI.GetRGB(204, 204, 0);
            ChromaAnimationAPI.MultiplyTargetColorLerpAllFramesName(baseLayer, color1, color2);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.SetChromaCustomColorAllFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 13);
        }
        void ShowEffect13ChromaLink()
        {
            string baseLayer = "Animations/Effect13_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.DuplicateFramesName(baseLayer);
            int color1 = ChromaAnimationAPI.GetRGB(0, 0, 0);
            int color2 = ChromaAnimationAPI.GetRGB(204, 204, 0);
            ChromaAnimationAPI.MultiplyTargetColorLerpAllFramesName(baseLayer, color1, color2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 13);
        }
        void ShowEffect13Headset()
        {
            string baseLayer = "Animations/Effect13_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.DuplicateFramesName(baseLayer);
            int color1 = ChromaAnimationAPI.GetRGB(0, 0, 0);
            int color2 = ChromaAnimationAPI.GetRGB(204, 204, 0);
            ChromaAnimationAPI.MultiplyTargetColorLerpAllFramesName(baseLayer, color1, color2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 13);
        }
        void ShowEffect13Mousepad()
        {
            string baseLayer = "Animations/Effect13_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.DuplicateFramesName(baseLayer);
            int color1 = ChromaAnimationAPI.GetRGB(0, 0, 0);
            int color2 = ChromaAnimationAPI.GetRGB(204, 204, 0);
            ChromaAnimationAPI.MultiplyTargetColorLerpAllFramesName(baseLayer, color1, color2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 13);
        }
        void ShowEffect13Mouse()
        {
            string baseLayer = "Animations/Effect13_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.DuplicateFramesName(baseLayer);
            int color1 = ChromaAnimationAPI.GetRGB(0, 0, 0);
            int color2 = ChromaAnimationAPI.GetRGB(204, 204, 0);
            ChromaAnimationAPI.MultiplyTargetColorLerpAllFramesName(baseLayer, color1, color2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 13);
        }
        void ShowEffect13Keypad()
        {
            string baseLayer = "Animations/Effect13_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.DuplicateFramesName(baseLayer);
            int color1 = ChromaAnimationAPI.GetRGB(0, 0, 0);
            int color2 = ChromaAnimationAPI.GetRGB(204, 204, 0);
            ChromaAnimationAPI.MultiplyTargetColorLerpAllFramesName(baseLayer, color1, color2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 13);
        }
        void ShowEffect14Keyboard()
        {
            string baseLayer = "Animations/Effect14_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.SetChromaCustomColorAllFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 14);
        }
        void ShowEffect14ChromaLink()
        {
            string baseLayer = "Animations/Effect14_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 14);
        }
        void ShowEffect14Headset()
        {
            string baseLayer = "Animations/Effect14_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 14);
        }
        void ShowEffect14Mousepad()
        {
            string baseLayer = "Animations/Effect14_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 14);
        }
        void ShowEffect14Mouse()
        {
            string baseLayer = "Animations/Effect14_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 14);
        }
        void ShowEffect14Keypad()
        {
            string baseLayer = "Animations/Effect14_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 14);
        }
        void ShowEffect15Keyboard()
        {
            string baseLayer = "Animations/Effect15_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.SetChromaCustomColorAllFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 15);
        }
        void ShowEffect15ChromaLink()
        {
            string baseLayer = "Animations/Effect15_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 15);
        }
        void ShowEffect15Headset()
        {
            string baseLayer = "Animations/Effect15_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 15);
        }
        void ShowEffect15Mousepad()
        {
            string baseLayer = "Animations/Effect15_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 15);
        }
        void ShowEffect15Mouse()
        {
            string baseLayer = "Animations/Effect15_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 15);
        }
        void ShowEffect15Keypad()
        {
            string baseLayer = "Animations/Effect15_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 15);
        }
        #endregion


        #region Climbing

        void ShowClimbingKeyboard()
        {
            string baseLayer = "Animations/Climbing_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.SetChromaCustomColorAllFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 9);
        }
        void ShowClimbingChromaLink()
        {
            string baseLayer = "Animations/Climbing_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 9);
        }
        void ShowClimbingHeadset()
        {
            string baseLayer = "Animations/Climbing_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 9);
        }
        void ShowClimbingMousepad()
        {
            string baseLayer = "Animations/Climbing_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 9);
        }
        void ShowClimbingMouse()
        {
            string baseLayer = "Animations/Climbing_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 9);
        }
        void ShowClimbingKeypad()
        {
            string baseLayer = "Animations/Climbing_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 9);
        }

        #endregion


        #region Jumping

        void ShowJumpingKeyboard()
        {
            string baseLayer = "Animations/Jumping_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.TrimStartFramesName(baseLayer, 50);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.SetChromaCustomColorAllFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 9);
        }
        void ShowJumpingChromaLink()
        {
            string baseLayer = "Animations/Jumping_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.TrimStartFramesName(baseLayer, 50);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 9);
        }
        void ShowJumpingHeadset()
        {
            string baseLayer = "Animations/Jumping_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.TrimStartFramesName(baseLayer, 50);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 9);
        }
        void ShowJumpingMousepad()
        {
            string baseLayer = "Animations/Jumping_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.TrimStartFramesName(baseLayer, 50);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 9);
        }
        void ShowJumpingMouse()
        {
            string baseLayer = "Animations/Jumping_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.TrimStartFramesName(baseLayer, 50);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 9);
        }
        void ShowJumpingKeypad()
        {
            string baseLayer = "Animations/Jumping_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.TrimStartFramesName(baseLayer, 50);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            SetupEvent(baseLayer, 9);
        }

        #endregion
    }
}
