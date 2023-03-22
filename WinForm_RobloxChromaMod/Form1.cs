// Ref: https://www.youtube.com/watch?v=6iv7y5v1e0A - C# Capture
// Ref: https://www.youtube.com/watch?v=Q63GV5tnXN0 - Crop Bitmap

using ChromaSDK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinForm_RobloxChromaMod
{
    public partial class Form1 : Form
    {
        bool _mMouseDown = false;
        bool _mMouseOver = false;
        public static Point _sMouseMoveStart = Point.Empty;
        public static Point _sMouseMoveEnd = Point.Empty;
        public static Point _sMouseMoveOffset = Point.Empty;

        Image _mCaptureImage = null;

        string _mPreviousEffect = string.Empty;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateDebugLabels();
        }

        private void UpdateDebugLabels()
        {
            _mDebugLabel1.Text = string.Format("Start {0},{1}", _sMouseMoveStart.X, _sMouseMoveStart.Y);
            _mDebugLabel2.Text = string.Format("End {0},{1}", _sMouseMoveEnd.X, _sMouseMoveEnd.Y);
            _mDebugLabel3.Text = string.Format("Offset {0},{1}", _sMouseMoveOffset.X, _sMouseMoveOffset.Y);
        }

        #region Input Events

        private void PictureMouseDown(object sender, MouseEventArgs e)
        {
            if (!_mMouseDown && _mMouseOver)
            {
                _mMouseDown = true;
                _sMouseMoveStart = new Point(e.X + _sMouseMoveOffset.X, e.Y + _sMouseMoveOffset.Y);
                _sMouseMoveOffset = Point.Empty;
                UpdateDebugLabels();
            }
        }

        private void PictureMouseUp(object sender, MouseEventArgs e)
        {
            if (_mMouseDown)
            {
                _sMouseMoveEnd = new Point(e.X + _sMouseMoveOffset.X, e.Y + _sMouseMoveOffset.Y);
                _sMouseMoveOffset = new Point(_sMouseMoveStart.X - _sMouseMoveEnd.X, _sMouseMoveStart.Y - _sMouseMoveEnd.Y);
                UpdateDebugLabels();
            }
            _mMouseDown = false;
        }

        private void PictureMouseMove(object sender, MouseEventArgs e)
        {
            if (_mMouseDown)
            {
                _sMouseMoveEnd = new Point(e.X + _sMouseMoveOffset.X, e.Y + _sMouseMoveOffset.Y);
                UpdateDebugLabels();
            }
        }

        private void PictureMouseEnter(object sender, EventArgs e)
        {
            _mMouseOver = true;
        }

        private void PictureMouseLeave(object sender, EventArgs e)
        {
            _mMouseOver = false;
        }

        #endregion Input Events

        private void SetEffect(string effectName)
        {
            if (effectName != _mPreviousEffect)
            {
                _mPreviousEffect = effectName;
                _mLabelButtonEffect.Text = effectName;
                /*
                if (effectName != "None")
                {
                    string animationName = "Animations/" + effectName;
                    ChromaAnimationAPI.PlayComposite(animationName, false);
                }
                */
                switch (effectName)
                {
                    case "Effect1":
                        ShowEffect1ChromaLink();
                        ShowEffect1Headset();
                        ShowEffect1Keyboard();
                        ShowEffect1Keypad();
                        ShowEffect1Mousepad();
                        ShowEffect1Mouse();
                        break;
                    case "Effect2":
                        ShowEffect2ChromaLink();
                        ShowEffect2Headset();
                        ShowEffect2Keyboard();
                        ShowEffect2Keypad();
                        ShowEffect2Mousepad();
                        ShowEffect2Mouse();
                        break;
                    case "Effect3":
                        ShowEffect3ChromaLink();
                        ShowEffect3Headset();
                        ShowEffect3Keyboard();
                        ShowEffect3Keypad();
                        ShowEffect3Mousepad();
                        ShowEffect3Mouse();
                        break;
                    case "Effect4":
                        ShowEffect4ChromaLink();
                        ShowEffect4Headset();
                        ShowEffect4Keyboard();
                        ShowEffect4Keypad();
                        ShowEffect4Mousepad();
                        ShowEffect4Mouse();
                        break;
                    case "Effect5":
                        ShowEffect5ChromaLink();
                        ShowEffect5Headset();
                        ShowEffect5Keyboard();
                        ShowEffect5Keypad();
                        ShowEffect5Mousepad();
                        ShowEffect5Mouse();
                        break;
                    case "Effect6":
                        ShowEffect6ChromaLink();
                        ShowEffect6Headset();
                        ShowEffect6Keyboard();
                        ShowEffect6Keypad();
                        ShowEffect6Mousepad();
                        ShowEffect6Mouse();
                        break;
                    case "Effect7":
                        ShowEffect7ChromaLink();
                        ShowEffect7Headset();
                        ShowEffect7Keyboard();
                        ShowEffect7Keypad();
                        ShowEffect7Mousepad();
                        ShowEffect7Mouse();
                        break;
                    case "Effect8":
                        ShowEffect8ChromaLink();
                        ShowEffect8Headset();
                        ShowEffect8Keyboard();
                        ShowEffect8Keypad();
                        ShowEffect8Mousepad();
                        ShowEffect8Mouse();
                        break;
                    case "Effect9":
                        ShowEffect9ChromaLink();
                        ShowEffect9Headset();
                        ShowEffect9Keyboard();
                        ShowEffect9Keypad();
                        ShowEffect9Mousepad();
                        ShowEffect9Mouse();
                        break;
                    case "Effect10":
                        ShowEffect10ChromaLink();
                        ShowEffect10Headset();
                        ShowEffect10Keyboard();
                        ShowEffect10Keypad();
                        ShowEffect10Mousepad();
                        ShowEffect10Mouse();
                        break;
                    case "Effect11":
                        ShowEffect11ChromaLink();
                        ShowEffect11Headset();
                        ShowEffect11Keyboard();
                        ShowEffect11Keypad();
                        ShowEffect11Mousepad();
                        ShowEffect11Mouse();
                        break;
                    case "Effect12":
                        ShowEffect12ChromaLink();
                        ShowEffect12Headset();
                        ShowEffect12Keyboard();
                        ShowEffect12Keypad();
                        ShowEffect12Mousepad();
                        ShowEffect12Mouse();
                        break;
                    case "Effect13":
                        ShowEffect13ChromaLink();
                        ShowEffect13Headset();
                        ShowEffect13Keyboard();
                        ShowEffect13Keypad();
                        ShowEffect13Mousepad();
                        ShowEffect13Mouse();
                        break;
                    case "Effect14":
                        ShowEffect14ChromaLink();
                        ShowEffect14Headset();
                        ShowEffect14Keyboard();
                        ShowEffect14Keypad();
                        ShowEffect14Mousepad();
                        ShowEffect14Mouse();
                        break;
                    case "Effect15":
                        ShowEffect15ChromaLink();
                        ShowEffect15Headset();
                        ShowEffect15Keyboard();
                        ShowEffect15Keypad();
                        ShowEffect15Mousepad();
                        ShowEffect15Mouse();
                        break;
                }
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

        private void _mCaptureTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // replace clipboard operation later
                SendKeys.Send("{PRTSC}");

                // clipboard operation can fail
                _mCaptureImage = Clipboard.GetImage();
            }
            catch
            {

            }

            try
            {
                // do some cropping
                Graphics g = _mPictureBox.CreateGraphics();
                Rectangle rectCropArea = new Rectangle(
                    _sMouseMoveStart.X - _sMouseMoveEnd.X,
                    _sMouseMoveStart.Y - _sMouseMoveEnd.Y,
                    _mPictureBox.Width,
                    _mPictureBox.Height);
                g.DrawImage(_mCaptureImage, new Rectangle(0, 0, _mPictureBox.Width, _mPictureBox.Height), rectCropArea, GraphicsUnit.Pixel);

                // read the center pixel
                Bitmap bmp = new Bitmap(_mCaptureImage);
                Color color = bmp.GetPixel(rectCropArea.X + _mPictureBox.Width / 2,
                    rectCropArea.Y + _mPictureBox.Height / 2);
                _mButtonColor.Text = string.Format("{0},{1},{2}", color.R, color.G, color.B);

                // cleanup
                bmp.Dispose();
                _mCaptureImage.Dispose();
                g.Dispose();

                if (MatchColorRed(color, 1))
                {
                    SetEffect("Effect1");
                }
                else if (MatchColorRed(color, 2))
                {
                    SetEffect("Effect2");
                }
                else if (MatchColorRed(color, 3))
                {
                    SetEffect("Effect3");
                }
                else if (MatchColorRed(color, 4))
                {
                    SetEffect("Effect4");
                }
                else if (MatchColorRed(color, 5))
                {
                    SetEffect("Effect5");
                }
                else if (MatchColorRed(color, 6))
                {
                    SetEffect("Effect6");
                }
                else if (MatchColorRed(color, 7))
                {
                    SetEffect("Effect7");
                }
                else if (MatchColorRed(color, 8))
                {
                    SetEffect("Effect8");
                }
                else if (MatchColorRed(color, 9))
                {
                    SetEffect("Effect9");
                }
                else if (MatchColorRed(color, 10))
                {
                    SetEffect("Effect10");
                }
                else if (MatchColorRed(color, 11))
                {
                    SetEffect("Effect11");
                }
                else if (MatchColorRed(color, 12))
                {
                    SetEffect("Effect12");
                }
                else if (MatchColorRed(color, 13))
                {
                    SetEffect("Effect13");
                }
                else if (MatchColorRed(color, 14))
                {
                    SetEffect("Effect14");
                }
                else if (MatchColorRed(color, 15))
                {
                    SetEffect("Effect15");
                }
                else
                {
                    SetEffect("None");
                }

                const byte MASK_DEAD = 1;
                const byte MASK_CLIMBING = 3;
                const byte MASK_JUMPING = 7;
                const byte MASK_FLYING = 15;
                const byte MASK_RUNNING = 37;
                const byte MASK_SWIMMING = 63;
                const byte MASK_SEATED = 127;

                if (MatchColorGeeenMask(color, MASK_DEAD))
                {
                    if (!GetPlayerState("Dead"))
                    {
                        SetPlayerState("Dead", true);

                        ShowEffect5ChromaLink();
                        ShowEffect5Headset();
                        ShowEffect5Keyboard();
                        ShowEffect5Keypad();
                        ShowEffect5Mouse();
                        ShowEffect5Mousepad();
                    }
                }
                else
                {
                    SetPlayerState("Dead", false);
                }

                if (MatchColorGeeenMask(color, MASK_JUMPING))
                {
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
                }
                else
                {
                    SetPlayerState("Jumping", false);
                }

                if (MatchColorGeeenMask(color, MASK_CLIMBING))
                {
                    if (!GetPlayerState("Climbing"))
                    {
                        SetPlayerState("Climbing", true);
                        ShowClimbingChromaLink();
                        ShowClimbingHeadset();
                        ShowClimbingKeyboard();
                        ShowClimbingKeypad();
                        ShowClimbingMousepad();
                        ShowClimbingMouse();
                    }
                }
                else
                {
                    SetPlayerState("Climbing", false);
                }

                if (MatchColorGeeenMask(color, MASK_FLYING))
                {
                    if (!GetPlayerState("Flying"))
                    {
                        SetPlayerState("Flying", true);

                        ShowEffect7ChromaLink();
                        ShowEffect7Headset();
                        ShowEffect7Keyboard();
                        ShowEffect7Keypad();
                        ShowEffect7Mouse();
                        ShowEffect7Mousepad();
                    }
                }
                else
                {
                    SetPlayerState("Flying", true);
                }

                if (MatchColorGeeenMask(color, MASK_RUNNING))
                {
                    if (!GetPlayerState("Running"))
                    {
                        SetPlayerState("Running", true);

                        ShowEffect7ChromaLink();
                        ShowEffect7Headset();
                        ShowEffect7Keyboard();
                        ShowEffect7Keypad();
                        ShowEffect7Mouse();
                        ShowEffect7Mousepad();
                    }
                }
                else
                {
                    SetPlayerState("Running", false);
                }

                if (MatchColorGeeenMask(color, MASK_SWIMMING))
                {
                    if (!GetPlayerState("Swimming"))
                    {
                        SetPlayerState("Swimming", true);

                        ShowEffect4ChromaLink();
                        ShowEffect4Headset();
                        ShowEffect4Keyboard();
                        ShowEffect4Keypad();
                        ShowEffect4Mouse();
                        ShowEffect4Mousepad();
                    }
                }
                else
                {
                    SetPlayerState("Swimming", false);
                }

                if (MatchColorGeeenMask(color, MASK_SEATED))
                {
                    if (!GetPlayerState("Seated"))
                    {
                        SetPlayerState("Seated", true);

                        ShowEffect3ChromaLink();
                        ShowEffect3Headset();
                        ShowEffect3Keyboard();
                        ShowEffect3Keypad();
                        ShowEffect3Mouse();
                        ShowEffect3Mousepad();
                    }
                }
                else
                {
                    SetPlayerState("Seated", false);
                }

            }
            catch
            {
            }
        }

        #region Helper methods

        private bool MatchColor(Color color, byte red, byte green, byte blue)
        {
            return (color.R == red && color.G == green && color.B == blue);
        }

        private bool MatchColorRed(Color color, byte red)
        {
            return (color.R == red);
        }

        private bool MatchColorGeeenMask(Color color, byte mask)
        {
            return ((color.G | mask) == mask);
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

        #endregion Helper methods


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
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect2Keyboard()
        {
            string baseLayer = "Animations/Effect2_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect2ChromaLink()
        {
            string baseLayer = "Animations/Effect2_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect2Headset()
        {
            string baseLayer = "Animations/Effect2_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect2Mousepad()
        {
            string baseLayer = "Animations/Effect2_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect2Mouse()
        {
            string baseLayer = "Animations/Effect2_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect2Keypad()
        {
            string baseLayer = "Animations/Effect2_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect3Keyboard()
        {
            string baseLayer = "Animations/Effect3_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect3ChromaLink()
        {
            string baseLayer = "Animations/Effect3_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect3Headset()
        {
            string baseLayer = "Animations/Effect3_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect3Mousepad()
        {
            string baseLayer = "Animations/Effect3_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect3Mouse()
        {
            string baseLayer = "Animations/Effect3_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect3Keypad()
        {
            string baseLayer = "Animations/Effect3_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect4Keyboard()
        {
            string baseLayer = "Animations/Effect4_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect4ChromaLink()
        {
            string baseLayer = "Animations/Effect4_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect4Headset()
        {
            string baseLayer = "Animations/Effect4_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect4Mousepad()
        {
            string baseLayer = "Animations/Effect4_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect4Mouse()
        {
            string baseLayer = "Animations/Effect4_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect4Keypad()
        {
            string baseLayer = "Animations/Effect4_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect5Keyboard()
        {
            string baseLayer = "Animations/Effect5_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect5ChromaLink()
        {
            string baseLayer = "Animations/Effect5_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect5Headset()
        {
            string baseLayer = "Animations/Effect5_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect5Mousepad()
        {
            string baseLayer = "Animations/Effect5_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect5Mouse()
        {
            string baseLayer = "Animations/Effect5_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect5Keypad()
        {
            string baseLayer = "Animations/Effect5_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect7ChromaLink()
        {
            string baseLayer = "Animations/Effect7_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect7Headset()
        {
            string baseLayer = "Animations/Effect7_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect7Mousepad()
        {
            string baseLayer = "Animations/Effect7_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect7Mouse()
        {
            string baseLayer = "Animations/Effect7_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect7Keypad()
        {
            string baseLayer = "Animations/Effect7_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect8Keyboard()
        {
            string baseLayer = "Animations/Effect8_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect8ChromaLink()
        {
            string baseLayer = "Animations/Effect8_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect8Headset()
        {
            string baseLayer = "Animations/Effect8_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect8Mousepad()
        {
            string baseLayer = "Animations/Effect8_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect8Mouse()
        {
            string baseLayer = "Animations/Effect8_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect8Keypad()
        {
            string baseLayer = "Animations/Effect8_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect9Keyboard()
        {
            string baseLayer = "Animations/Effect9_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect9ChromaLink()
        {
            string baseLayer = "Animations/Effect9_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect9Headset()
        {
            string baseLayer = "Animations/Effect9_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect9Mousepad()
        {
            string baseLayer = "Animations/Effect9_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect9Mouse()
        {
            string baseLayer = "Animations/Effect9_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect9Keypad()
        {
            string baseLayer = "Animations/Effect9_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect10Keyboard()
        {
            string baseLayer = "Animations/Effect10_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect10ChromaLink()
        {
            string baseLayer = "Animations/Effect10_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect10Headset()
        {
            string baseLayer = "Animations/Effect10_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect10Mousepad()
        {
            string baseLayer = "Animations/Effect10_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect10Mouse()
        {
            string baseLayer = "Animations/Effect10_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect10Keypad()
        {
            string baseLayer = "Animations/Effect10_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect12Keyboard()
        {
            string baseLayer = "Animations/Effect12_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect12ChromaLink()
        {
            string baseLayer = "Animations/Effect12_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect12Headset()
        {
            string baseLayer = "Animations/Effect12_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect12Mousepad()
        {
            string baseLayer = "Animations/Effect12_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect12Mouse()
        {
            string baseLayer = "Animations/Effect12_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect12Keypad()
        {
            string baseLayer = "Animations/Effect12_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect14Keyboard()
        {
            string baseLayer = "Animations/Effect14_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect14ChromaLink()
        {
            string baseLayer = "Animations/Effect14_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect14Headset()
        {
            string baseLayer = "Animations/Effect14_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect14Mousepad()
        {
            string baseLayer = "Animations/Effect14_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect14Mouse()
        {
            string baseLayer = "Animations/Effect14_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect14Keypad()
        {
            string baseLayer = "Animations/Effect14_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect15Keyboard()
        {
            string baseLayer = "Animations/Effect15_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect15ChromaLink()
        {
            string baseLayer = "Animations/Effect15_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect15Headset()
        {
            string baseLayer = "Animations/Effect15_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect15Mousepad()
        {
            string baseLayer = "Animations/Effect15_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect15Mouse()
        {
            string baseLayer = "Animations/Effect15_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowEffect15Keypad()
        {
            string baseLayer = "Animations/Effect15_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowClimbingChromaLink()
        {
            string baseLayer = "Animations/Climbing_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowClimbingHeadset()
        {
            string baseLayer = "Animations/Climbing_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowClimbingMousepad()
        {
            string baseLayer = "Animations/Climbing_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowClimbingMouse()
        {
            string baseLayer = "Animations/Climbing_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowClimbingKeypad()
        {
            string baseLayer = "Animations/Climbing_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
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
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowJumpingChromaLink()
        {
            string baseLayer = "Animations/Jumping_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.TrimStartFramesName(baseLayer, 50);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowJumpingHeadset()
        {
            string baseLayer = "Animations/Jumping_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.TrimStartFramesName(baseLayer, 50);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowJumpingMousepad()
        {
            string baseLayer = "Animations/Jumping_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.TrimStartFramesName(baseLayer, 50);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowJumpingMouse()
        {
            string baseLayer = "Animations/Jumping_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.TrimStartFramesName(baseLayer, 50);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowJumpingKeypad()
        {
            string baseLayer = "Animations/Jumping_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.TrimStartFramesName(baseLayer, 50);
            ChromaAnimationAPI.ReduceFramesName(baseLayer, 2);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }

        #endregion


        #region Running

        void ShowRunningKeyboard()
        {
            string baseLayer = "Animations/Running_Keyboard.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.TrimStartFramesName(baseLayer, 50);
            ChromaAnimationAPI.DuplicateFramesName(baseLayer);
            SetupHotkeys(baseLayer);
            ChromaAnimationAPI.SetChromaCustomFlagName(baseLayer, true);
            ChromaAnimationAPI.SetChromaCustomColorAllFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowRunningChromaLink()
        {
            string baseLayer = "Animations/Running_ChromaLink.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.TrimStartFramesName(baseLayer, 50);
            ChromaAnimationAPI.DuplicateFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowRunningHeadset()
        {
            string baseLayer = "Animations/Running_Headset.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.TrimStartFramesName(baseLayer, 50);
            ChromaAnimationAPI.DuplicateFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowRunningMousepad()
        {
            string baseLayer = "Animations/Running_Mousepad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.TrimStartFramesName(baseLayer, 50);
            ChromaAnimationAPI.DuplicateFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowRunningMouse()
        {
            string baseLayer = "Animations/Running_Mouse.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.TrimStartFramesName(baseLayer, 50);
            ChromaAnimationAPI.DuplicateFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }
        void ShowRunningKeypad()
        {
            string baseLayer = "Animations/Running_Keypad.chroma";
            ChromaAnimationAPI.CloseAnimationName(baseLayer);
            ChromaAnimationAPI.GetAnimation(baseLayer);
            ChromaAnimationAPI.TrimStartFramesName(baseLayer, 50);
            ChromaAnimationAPI.DuplicateFramesName(baseLayer);
            ChromaAnimationAPI.OverrideFrameDurationName(baseLayer, 0.033f);
            ChromaAnimationAPI.PlayAnimationName(baseLayer, false);
        }

        #endregion
    }
}
