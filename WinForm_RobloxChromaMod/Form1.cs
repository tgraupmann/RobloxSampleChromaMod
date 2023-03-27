// Ref: https://www.youtube.com/watch?v=6iv7y5v1e0A - C# Capture
// Ref: https://www.youtube.com/watch?v=Q63GV5tnXN0 - Crop Bitmap
// Ref: https://www.c-sharpcorner.com/UploadFile/2d2d83/how-to-capture-a-screen-using-C-Sharp/ - Capture Screen

using ChromaSDK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;
using System.Windows.Forms;
using static ChromaSDK.ChromaAnimationAPI;

namespace WinForm_RobloxChromaMod
{
    public partial class Form1 : Form
    {
        bool _mWaitForExit = true;

        bool _mMouseDown = false;
        bool _mMouseOver = false;
        public static Point _sMouseMoveStart = Point.Empty;
        public static Point _sMouseMoveEnd = Point.Empty;
        public static Point _sMouseMoveOffset = Point.Empty;
        public static int _sScreenIndex = -1;

        Image _mCaptureImage = null;

        FChromaSDKScene _mScene = null;
        Dictionary<string, int> _mEffectIndexes = new Dictionary<string, int>();
        bool _mExtended = true; // extended keyboars support
        int _mAmbientColor = 0;

        string _mPreviousEffect = string.Empty;

        const string ANIMATION_DEAD = "Animations/Dead";
        const string ANIMATION_CLIMBING = "Animations/Climbing";
        const string ANIMATION_FLYING = "Animations/Flying";
        const string ANIMATION_RUNNING = "Animations/Running";
        const string ANIMATION_SEATED = "Animations/Seated";
        const string ANIMATION_SWIMMING = "Animations/Swimming";
        const string ANIMATION_JUMPING = "Animations/Jumping";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // list all monitors
            int indexScreen = 1;
            foreach (Screen screen in Screen.AllScreens)
            {
                string deviceName = string.Format("{0}: {1}", indexScreen, screen.DeviceName);
                _mCboMonitors.Items.Add(deviceName);
                ++indexScreen;
            }

            _mCboMonitors.SelectedIndexChanged -= CboItemChanged;
            try
            {
                _mCboMonitors.SelectedIndex = _sScreenIndex + 1;
            }
            catch
            {
                _mCboMonitors.SelectedIndex = 0;
            }
            _mCboMonitors.SelectedIndexChanged += CboItemChanged;

            UpdateDebugLabels();

            // create capture bitmap
            _mCaptureImage = new Bitmap(_mPictureBox.Width, _mPictureBox.Height, PixelFormat.Format24bppRgb);

            // setup scene
            _mScene = new FChromaSDKScene();

            const int SPEED_MULTIPLIER = 3;

            FChromaSDKSceneEffect effect;

            for (int animation = 1; animation <= 15; ++animation)
            {
                effect = new FChromaSDKSceneEffect();
                effect._mAnimation = string.Format("Animations/Effect{0}", animation);
                effect._mSpeed = SPEED_MULTIPLIER;
                effect._mBlend = EChromaSDKSceneBlend.SB_None;
                effect._mState = false;
                effect._mMode = EChromaSDKSceneMode.SM_Add;
                _mScene._mEffects.Add(effect);
                _mEffectIndexes[effect._mAnimation] = (int)_mScene._mEffects.Count - 1;
            }

            // climbing
            effect = new FChromaSDKSceneEffect();
            effect._mAnimation = ANIMATION_CLIMBING;
            effect._mSpeed = SPEED_MULTIPLIER;
            effect._mBlend = EChromaSDKSceneBlend.SB_None;
            effect._mState = false;
            effect._mMode = EChromaSDKSceneMode.SM_Add;
            _mScene._mEffects.Add(effect);
            _mEffectIndexes[effect._mAnimation] = (int)_mScene._mEffects.Count - 1;

            // flying
            effect = new FChromaSDKSceneEffect();
            effect._mAnimation = ANIMATION_FLYING;
            effect._mSpeed = SPEED_MULTIPLIER;
            effect._mBlend = EChromaSDKSceneBlend.SB_None;
            effect._mState = false;
            effect._mMode = EChromaSDKSceneMode.SM_Add;
            _mScene._mEffects.Add(effect);
            _mEffectIndexes[effect._mAnimation] = (int)_mScene._mEffects.Count - 1;

            // running
            effect = new FChromaSDKSceneEffect();
            effect._mAnimation = ANIMATION_RUNNING;
            effect._mSpeed = SPEED_MULTIPLIER;
            effect._mBlend = EChromaSDKSceneBlend.SB_None;
            effect._mState = false;
            effect._mMode = EChromaSDKSceneMode.SM_Add;
            _mScene._mEffects.Add(effect);
            _mEffectIndexes[effect._mAnimation] = (int)_mScene._mEffects.Count - 1;

            // seated
            effect = new FChromaSDKSceneEffect();
            effect._mAnimation = ANIMATION_SEATED;
            effect._mSpeed = SPEED_MULTIPLIER;
            effect._mBlend = EChromaSDKSceneBlend.SB_None;
            effect._mState = false;
            effect._mMode = EChromaSDKSceneMode.SM_Add;
            _mScene._mEffects.Add(effect);
            _mEffectIndexes[effect._mAnimation] = (int)_mScene._mEffects.Count - 1;

            // swimming
            effect = new FChromaSDKSceneEffect();
            effect._mAnimation = ANIMATION_SWIMMING;
            effect._mSpeed = SPEED_MULTIPLIER;
            effect._mBlend = EChromaSDKSceneBlend.SB_None;
            effect._mState = false;
            effect._mMode = EChromaSDKSceneMode.SM_Add;
            _mScene._mEffects.Add(effect);
            _mEffectIndexes[effect._mAnimation] = (int)_mScene._mEffects.Count - 1;

            // jumping
            effect = new FChromaSDKSceneEffect();
            effect._mAnimation = ANIMATION_JUMPING;
            effect._mSpeed = SPEED_MULTIPLIER;
            effect._mBlend = EChromaSDKSceneBlend.SB_None;
            effect._mState = false;
            effect._mMode = EChromaSDKSceneMode.SM_Add;
            _mScene._mEffects.Add(effect);
            _mEffectIndexes[effect._mAnimation] = (int)_mScene._mEffects.Count - 1;

            // dead
            effect = new FChromaSDKSceneEffect();
            effect._mAnimation = ANIMATION_DEAD;
            effect._mSpeed = SPEED_MULTIPLIER;
            effect._mBlend = EChromaSDKSceneBlend.SB_None;
            effect._mState = false;
            effect._mMode = EChromaSDKSceneMode.SM_Add;
            _mScene._mEffects.Add(effect);
            _mEffectIndexes[effect._mAnimation] = (int)_mScene._mEffects.Count - 1;

            // Start game loop
            ThreadStart ts = new ThreadStart(GameLoop);
            Thread thread = new Thread(ts);
            thread.Start();
        }

        private void Form1_FormClosed(object sender, System.Windows.Forms.FormClosedEventArgs e)
        {
            _mWaitForExit = false;
            if (_mCaptureImage != null)
            {
                _mCaptureImage.Dispose();
            }
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

                for (int animation = 1; animation <= 15; ++animation)
                {
                    string animationName = string.Format("Animations/Effect{0}", animation);
                    int effectIndex = _mEffectIndexes[animationName];
                    _mScene._mEffects[effectIndex]._mState = false;
                }

                switch (effectName)
                {
                    case "Effect1":
                    case "Effect2":
                    case "Effect3":
                    case "Effect4":
                    case "Effect5":
                    case "Effect6":
                    case "Effect7":
                    case "Effect8":
                    case "Effect9":
                    case "Effect10":
                    case "Effect11":
                    case "Effect12":
                    case "Effect13":
                    case "Effect14":
                    case "Effect15":
                        {
                            string animationName = string.Format("Animations/{0}", effectName);
                            int effectIndex = _mEffectIndexes[animationName];
                            _mScene._mEffects[effectIndex]._mState = true;
                        }
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
                #region Capture with clipboard
                
                /*

                // replace clipboard operation later
                SendKeys.Send("{PRTSC}");

                // clipboard operation can fail
                _mCaptureImage = Clipboard.GetImage();

                */

                #endregion Capture with clipboard
            }
            catch
            {

            }

            Graphics captureGraphics = null;
            Graphics g = null;
            Bitmap bmp = null;

            try
            {
                int selectedIndex = _mCboMonitors.SelectedIndex;

                if (selectedIndex < 1 || (selectedIndex - 1) >= Screen.AllScreens.Length)
                {
                    return; // skip capture
                }

                // get the selected screen
                Screen screen = Screen.AllScreens[selectedIndex - 1];


                // capture from screen
                Rectangle captureRectangle = screen.Bounds;
                // create graphics
                captureGraphics = Graphics.FromImage(_mCaptureImage);
                // copy pixels from screen
                captureGraphics.CopyFromScreen(
                    captureRectangle.Left + _sMouseMoveStart.X - _sMouseMoveEnd.X,
                    captureRectangle.Top + _sMouseMoveStart.Y - _sMouseMoveEnd.Y, 0, 0, captureRectangle.Size);

                // do some cropping
                g = _mPictureBox.CreateGraphics();

                Rectangle rectCropArea = new Rectangle(
                    0,
                    0,
                    _mPictureBox.Width,
                    _mPictureBox.Height);
                g.DrawImage(_mCaptureImage, 0, 0);

                // read the center pixel
                bmp = new Bitmap(_mCaptureImage);
                int x = Math.Max(0, _mPictureBox.Width / 2);
                int y = Math.Max(0, _mPictureBox.Height / 2);
                Color color = bmp.GetPixel(x, y);
                _mButtonColor.Text = string.Format("{0},{1},{2}", color.R, color.G, color.B);

                #region Button Effects

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

                #endregion Button Effects

                const byte MASK_DEAD = 0x1;
                const byte MASK_CLIMBING = 0x1 << 1;
                const byte MASK_JUMPING = 0x1 << 2;
                const byte MASK_FLYING = 0x1 << 3;
                const byte MASK_RUNNING = 0x1 << 4;
                const byte MASK_SWIMMING = 0x1 << 5;
                const byte MASK_SEATED = 0x1 << 6;

                _mScene._mEffects[_mEffectIndexes[ANIMATION_DEAD]]._mState =
                    MatchColorGeeenMask(color, MASK_DEAD);

                _mScene._mEffects[_mEffectIndexes[ANIMATION_CLIMBING]]._mState =
                    MatchColorGeeenMask(color, MASK_CLIMBING);

                _mScene._mEffects[_mEffectIndexes[ANIMATION_RUNNING]]._mState =
                    MatchColorGeeenMask(color, MASK_RUNNING);

                _mScene._mEffects[_mEffectIndexes[ANIMATION_FLYING]]._mState =
                    MatchColorGeeenMask(color, MASK_FLYING);

                _mScene._mEffects[_mEffectIndexes[ANIMATION_SEATED]]._mState =
                    MatchColorGeeenMask(color, MASK_SEATED);

                _mScene._mEffects[_mEffectIndexes[ANIMATION_SWIMMING]]._mState =
                    MatchColorGeeenMask(color, MASK_SWIMMING);

                _mScene._mEffects[_mEffectIndexes[ANIMATION_JUMPING]]._mState =
                    MatchColorGeeenMask(color, MASK_JUMPING); 

                _mLblDebugDead.Text = string.Format("Dead: {0}", _mScene._mEffects[_mEffectIndexes[ANIMATION_DEAD]]._mState);
                _mLblDebugClimbing.Text = string.Format("Climbing: {0}", _mScene._mEffects[_mEffectIndexes[ANIMATION_CLIMBING]]._mState);
                _mLblDebugJumping.Text = string.Format("Jumping: {0}", _mScene._mEffects[_mEffectIndexes[ANIMATION_JUMPING]]._mState);
                _mLblDebugFlying.Text = string.Format("Flying: {0}", _mScene._mEffects[_mEffectIndexes[ANIMATION_FLYING]]._mState);
                _mLblDebugRunning.Text = string.Format("Running: {0}", _mScene._mEffects[_mEffectIndexes[ANIMATION_RUNNING]]._mState);
                _mLblDebugSeated.Text = string.Format("Seated: {0}", _mScene._mEffects[_mEffectIndexes[ANIMATION_SEATED]]._mState);
                _mLblDebugSwimming.Text = string.Format("Swimming: {0}", _mScene._mEffects[_mEffectIndexes[ANIMATION_SWIMMING]]._mState);

            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to capture image exception: {0}", ex);
            }
            finally
            {
                // cleanup
                if (bmp != null)
                {
                    bmp.Dispose();
                }
                if (g != null)
                {
                    g.Dispose();
                }
                if (captureGraphics != null)
                {
                    captureGraphics.Dispose();
                }
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
            return ((color.G & mask) == mask);
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


        #region Blending

        int HIBYTE(int a)
        {
            return (a & 0xFF00) >> 8;
        }

        int LOBYTE(int a)
        {
            return (a & 0x00FF);
        }


        int GetKeyColorIndex(int row, int column)
        {
            return ChromaAnimationAPI.GetMaxColumn(Device2D.Keyboard) * row + column;
        }

        void SetKeyColor(int[] colors, int rzkey, int color)
        {
            const int customFlag = 1 << 24;
            int row = HIBYTE(rzkey);
            int column = LOBYTE(rzkey);
            colors[GetKeyColorIndex(row, column)] = color | customFlag;
        }

        void SetKeyColorRGB(int[] colors, int rzkey, int red, int green, int blue)
        {
            SetKeyColor(colors, rzkey, ChromaAnimationAPI.GetRGB(red, green, blue));
        }

        int GetColorArraySize1D(Device1D device)
        {
            int maxLeds = ChromaAnimationAPI.GetMaxLeds(device);
            return maxLeds;
        }

        int GetColorArraySize2D(Device2D device)
        {
            int maxRow = ChromaAnimationAPI.GetMaxRow(device);
            int maxColumn = ChromaAnimationAPI.GetMaxColumn(device);
            return maxRow * maxColumn;
        }

        int MultiplyColor(int color1, int color2)
        {
            int redColor1 = color1 & 0xFF;
            int greenColor1 = (color1 >> 8) & 0xFF;
            int blueColor1 = (color1 >> 16) & 0xFF;

            int redColor2 = color2 & 0xFF;
            int greenColor2 = (color2 >> 8) & 0xFF;
            int blueColor2 = (color2 >> 16) & 0xFF;

            int red = (int)Math.Floor(255 * ((redColor1 / 255.0f) * (redColor2 / 255.0f)));
            int green = (int)Math.Floor(255 * ((greenColor1 / 255.0f) * (greenColor2 / 255.0f)));
            int blue = (int)Math.Floor(255 * ((blueColor1 / 255.0f) * (blueColor2 / 255.0f)));

            return ChromaAnimationAPI.GetRGB(red, green, blue);
        }

        int AverageColor(int color1, int color2)
        {
            return ChromaAnimationAPI.LerpColor(color1, color2, 0.5f);
        }

        int AddColor(int color1, int color2)
        {
            int redColor1 = color1 & 0xFF;
            int greenColor1 = (color1 >> 8) & 0xFF;
            int blueColor1 = (color1 >> 16) & 0xFF;

            int redColor2 = color2 & 0xFF;
            int greenColor2 = (color2 >> 8) & 0xFF;
            int blueColor2 = (color2 >> 16) & 0xFF;

            int red = Math.Min(redColor1 + redColor2, 255) & 0xFF;
            int green = Math.Min(greenColor1 + greenColor2, 255) & 0xFF;
            int blue = Math.Min(blueColor1 + blueColor2, 255) & 0xFF;

            return ChromaAnimationAPI.GetRGB(red, green, blue);
        }

        int SubtractColor(int color1, int color2)
        {
            int redColor1 = color1 & 0xFF;
            int greenColor1 = (color1 >> 8) & 0xFF;
            int blueColor1 = (color1 >> 16) & 0xFF;

            int redColor2 = color2 & 0xFF;
            int greenColor2 = (color2 >> 8) & 0xFF;
            int blueColor2 = (color2 >> 16) & 0xFF;

            int red = Math.Max(redColor1 - redColor2, 0) & 0xFF;
            int green = Math.Max(greenColor1 - greenColor2, 0) & 0xFF;
            int blue = Math.Max(blueColor1 - blueColor2, 0) & 0xFF;

            return ChromaAnimationAPI.GetRGB(red, green, blue);
        }

        int MaxColor(int color1, int color2)
        {
            int redColor1 = color1 & 0xFF;
            int greenColor1 = (color1 >> 8) & 0xFF;
            int blueColor1 = (color1 >> 16) & 0xFF;

            int redColor2 = color2 & 0xFF;
            int greenColor2 = (color2 >> 8) & 0xFF;
            int blueColor2 = (color2 >> 16) & 0xFF;

            int red = Math.Max(redColor1, redColor2) & 0xFF;
            int green = Math.Max(greenColor1, greenColor2) & 0xFF;
            int blue = Math.Max(blueColor1, blueColor2) & 0xFF;

            return ChromaAnimationAPI.GetRGB(red, green, blue);
        }

        int MinColor(int color1, int color2)
        {
            int redColor1 = color1 & 0xFF;
            int greenColor1 = (color1 >> 8) & 0xFF;
            int blueColor1 = (color1 >> 16) & 0xFF;

            int redColor2 = color2 & 0xFF;
            int greenColor2 = (color2 >> 8) & 0xFF;
            int blueColor2 = (color2 >> 16) & 0xFF;

            int red = Math.Min(redColor1, redColor2) & 0xFF;
            int green = Math.Min(greenColor1, greenColor2) & 0xFF;
            int blue = Math.Min(blueColor1, blueColor2) & 0xFF;

            return ChromaAnimationAPI.GetRGB(red, green, blue);
        }

        int InvertColor(int color)
        {
            int red = 255 - (color & 0xFF);
            int green = 255 - ((color >> 8) & 0xFF);
            int blue = 255 - ((color >> 16) & 0xFF);

            return ChromaAnimationAPI.GetRGB(red, green, blue);
        }

        int MultiplyNonZeroTargetColorLerp(int color1, int color2, int inputColor)
        {
            if (inputColor == 0)
            {
                return inputColor;
            }
            float red = (inputColor & 0xFF) / 255.0f;
            float green = ((inputColor & 0xFF00) >> 8) / 255.0f;
            float blue = ((inputColor & 0xFF0000) >> 16) / 255.0f;
            float t = (red + green + blue) / 3.0f;
            return ChromaAnimationAPI.LerpColor(color1, color2, t);
        }

        int Thresh(int color1, int color2, int inputColor)
        {
            float red = (inputColor & 0xFF) / 255.0f;
            float green = ((inputColor & 0xFF00) >> 8) / 255.0f;
            float blue = ((inputColor & 0xFF0000) >> 16) / 255.0f;
            float t = (red + green + blue) / 3.0f;
            if (t == 0.0)
            {
                return 0;
            }
            if (t < 0.5)
            {
                return color1;
            }
            else
            {
                return color2;
            }
        }


        void BlendAnimation1D(FChromaSDKSceneEffect effect, FChromaSDKDeviceFrameIndex deviceFrameIndex, int device, Device1D device1d, string animationName,
            int[] colors, int[] tempColors)
        {
            int size = GetColorArraySize1D(device1d);
            int frameId = deviceFrameIndex._mFrameIndex[device];
            int frameCount = ChromaAnimationAPI.GetFrameCountName(animationName);
            if (frameId < frameCount)
            {
                //cout << animationName << ": " << (1 + frameId) << " of " << frameCount << endl;
                float duration;
                ChromaAnimationAPI.GetFrameName(animationName, frameId, out duration, tempColors, size, null, 0);
                for (int i = 0; i < size; ++i)
                {
                    int color1 = colors[i]; //target
                    int tempColor = tempColors[i]; //source

                    // BLEND
                    int color2;
                    switch (effect._mBlend)
                    {
                        case EChromaSDKSceneBlend.SB_None:
                            color2 = tempColor; //source
                            break;
                        case EChromaSDKSceneBlend.SB_Invert:
                            if (tempColor != 0) //source
                            {
                                color2 = InvertColor(tempColor); //source inverted
                            }
                            else
                            {
                                color2 = 0;
                            }
                            break;
                        case EChromaSDKSceneBlend.SB_Threshold:
                            color2 = Thresh(effect._mPrimaryColor, effect._mSecondaryColor, tempColor); //source
                            break;
                        case EChromaSDKSceneBlend.SB_Lerp:
                        default:
                            color2 = MultiplyNonZeroTargetColorLerp(effect._mPrimaryColor, effect._mSecondaryColor, tempColor); //source
                            break;
                    }

                    // MODE
                    switch (effect._mMode)
                    {
                        case EChromaSDKSceneMode.SM_Max:
                            colors[i] = MaxColor(color1, color2);
                            break;
                        case EChromaSDKSceneMode.SM_Min:
                            colors[i] = MinColor(color1, color2);
                            break;
                        case EChromaSDKSceneMode.SM_Average:
                            colors[i] = AverageColor(color1, color2);
                            break;
                        case EChromaSDKSceneMode.SM_Multiply:
                            colors[i] = MultiplyColor(color1, color2);
                            break;
                        case EChromaSDKSceneMode.SM_Add:
                            colors[i] = AddColor(color1, color2);
                            break;
                        case EChromaSDKSceneMode.SM_Subtract:
                            colors[i] = SubtractColor(color1, color2);
                            break;
                        case EChromaSDKSceneMode.SM_Replace:
                        default:
                            if (color2 != 0)
                            {
                                colors[i] = color2;
                            }
                            break;
                    }
                }
                deviceFrameIndex._mFrameIndex[device] = (frameId + frameCount + effect._mSpeed) % frameCount;
            }
        }

        void BlendAnimation2D(FChromaSDKSceneEffect effect, FChromaSDKDeviceFrameIndex deviceFrameIndex, int device, Device2D device2D, string animationName,
            int[] colors, int[] tempColors)
        {
            int size = GetColorArraySize2D(device2D);
            int frameId = deviceFrameIndex._mFrameIndex[device];
            int frameCount = ChromaAnimationAPI.GetFrameCountName(animationName);
            if (frameId < frameCount)
            {
                //cout << animationName << ": " << (1 + frameId) << " of " << frameCount << endl;
                float duration;
                ChromaAnimationAPI.GetFrameName(animationName, frameId, out duration, tempColors, size, null, 0);
                for (int i = 0; i < size; ++i)
                {
                    int color1 = colors[i]; //target
                    int tempColor = tempColors[i]; //source

                    // BLEND
                    int color2;
                    switch (effect._mBlend)
                    {
                        case EChromaSDKSceneBlend.SB_None:
                            color2 = tempColor; //source
                            break;
                        case EChromaSDKSceneBlend.SB_Invert:
                            if (tempColor != 0) //source
                            {
                                color2 = InvertColor(tempColor); //source inverted
                            }
                            else
                            {
                                color2 = 0;
                            }
                            break;
                        case EChromaSDKSceneBlend.SB_Threshold:
                            color2 = Thresh(effect._mPrimaryColor, effect._mSecondaryColor, tempColor); //source
                            break;
                        case EChromaSDKSceneBlend.SB_Lerp:
                        default:
                            color2 = MultiplyNonZeroTargetColorLerp(effect._mPrimaryColor, effect._mSecondaryColor, tempColor); //source
                            break;
                    }

                    // MODE
                    switch (effect._mMode)
                    {
                        case EChromaSDKSceneMode.SM_Max:
                            colors[i] = MaxColor(color1, color2);
                            break;
                        case EChromaSDKSceneMode.SM_Min:
                            colors[i] = MinColor(color1, color2);
                            break;
                        case EChromaSDKSceneMode.SM_Average:
                            colors[i] = AverageColor(color1, color2);
                            break;
                        case EChromaSDKSceneMode.SM_Multiply:
                            colors[i] = MultiplyColor(color1, color2);
                            break;
                        case EChromaSDKSceneMode.SM_Add:
                            colors[i] = AddColor(color1, color2);
                            break;
                        case EChromaSDKSceneMode.SM_Subtract:
                            colors[i] = SubtractColor(color1, color2);
                            break;
                        case EChromaSDKSceneMode.SM_Replace:
                        default:
                            if (color2 != 0)
                            {
                                colors[i] = color2;
                            }
                            break;
                    }
                }
                deviceFrameIndex._mFrameIndex[device] = (frameId + frameCount + effect._mSpeed) % frameCount;
            }
        }

        void BlendAnimations(FChromaSDKScene scene,
            int[] colorsChromaLink, int[] tempColorsChromaLink,
            int[] colorsHeadset, int[] tempColorsHeadset,
            int[] colorsKeyboard, int[] tempColorsKeyboard,
            int[] colorsKeyboardExtended, int[] tempColorsKeyboardExtended,
            int[] colorsKeypad, int[] tempColorsKeypad,
            int[] colorsMouse, int[] tempColorsMouse,
            int[] colorsMousepad, int[] tempColorsMousepad)
        {
            // blend active animations
            List<FChromaSDKSceneEffect> effects = scene._mEffects;
            foreach (FChromaSDKSceneEffect effect in effects)
            {
                if (effect._mState)
                {
                    FChromaSDKDeviceFrameIndex deviceFrameIndex = effect._mFrameIndex;

                    //iterate all device types
                    for (int d = (int)Device.ChromaLink; d < (int)Device.MAX; ++d)
                    {
                        string animationName = effect._mAnimation;

                        switch ((Device)d)
                        {
                            case Device.ChromaLink:
                                animationName += "_ChromaLink.chroma";
                                BlendAnimation1D(effect, deviceFrameIndex, d, Device1D.ChromaLink, animationName, colorsChromaLink, tempColorsChromaLink);
                                break;
                            case Device.Headset:
                                animationName += "_Headset.chroma";
                                BlendAnimation1D(effect, deviceFrameIndex, d, Device1D.Headset, animationName, colorsHeadset, tempColorsHeadset);
                                break;
                            case Device.Keyboard:
                                animationName += "_Keyboard.chroma";
                                BlendAnimation2D(effect, deviceFrameIndex, d, Device2D.Keyboard, animationName, colorsKeyboard, tempColorsKeyboard);
                                break;
                            case Device.KeyboardExtended:
                                animationName += "_KeyboardExtended.chroma";
                                BlendAnimation2D(effect, deviceFrameIndex, d, Device2D.KeyboardExtended, animationName, colorsKeyboardExtended, tempColorsKeyboardExtended);
                                break;
                            case Device.Keypad:
                                animationName += "_Keypad.chroma";
                                BlendAnimation2D(effect, deviceFrameIndex, d, Device2D.Keypad, animationName, colorsKeypad, tempColorsKeypad);
                                break;
                            case Device.Mouse:
                                animationName += "_Mouse.chroma";
                                BlendAnimation2D(effect, deviceFrameIndex, d, Device2D.Mouse, animationName, colorsMouse, tempColorsMouse);
                                break;
                            case Device.Mousepad:
                                animationName += "_Mousepad.chroma";
                                BlendAnimation1D(effect, deviceFrameIndex, d, Device1D.Mousepad, animationName, colorsMousepad, tempColorsMousepad);
                                break;
                        }
                    }
                }

            }
        }

        public void SetStaticColor(int[] colors, int color)
        {
            for (int i = 0; i < colors.Length; ++i)
            {
                colors[i] = color;
            }
        }

        #endregion Blending

        public void GameLoop()
        {
            int sizeChromaLink = GetColorArraySize1D(Device1D.ChromaLink);
            int sizeHeadset = GetColorArraySize1D(Device1D.Headset);
            int sizeKeyboard = GetColorArraySize2D(Device2D.Keyboard);
            int sizeKeyboardExtended = GetColorArraySize2D(Device2D.KeyboardExtended);
            int sizeKeypad = GetColorArraySize2D(Device2D.Keypad);
            int sizeMouse = GetColorArraySize2D(Device2D.Mouse);
            int sizeMousepad = GetColorArraySize1D(Device1D.Mousepad);

            int[] colorsChromaLink = new int[sizeChromaLink];
            int[] colorsHeadset = new int[sizeHeadset];
            int[] colorsKeyboard = new int[sizeKeyboard];
            int[] colorsKeyboardExtended = new int[sizeKeyboardExtended];
            int[] colorsKeyboardKeys = new int[sizeKeyboard];
            int[] colorsKeypad = new int[sizeKeypad];
            int[] colorsMouse = new int[sizeMouse];
            int[] colorsMousepad = new int[sizeMousepad];

            int[] tempColorsChromaLink = new int[sizeChromaLink];
            int[] tempColorsHeadset = new int[sizeHeadset];
            int[] tempColorsKeyboard = new int[sizeKeyboard];
            int[] tempColorsKeyboardExtended = new int[sizeKeyboardExtended];
            int[] tempColorsKeypad = new int[sizeKeypad];
            int[] tempColorsMouse = new int[sizeMouse];
            int[] tempColorsMousepad = new int[sizeMousepad];

            uint timeMS = 0;

            while (_mWaitForExit)
            {
                // start with a blank frame
                SetStaticColor(colorsChromaLink, _mAmbientColor);
                SetStaticColor(colorsHeadset, _mAmbientColor);
                if (_mExtended)
                {
                    SetStaticColor(colorsKeyboardExtended, _mAmbientColor);
                }
                else
                {
                    SetStaticColor(colorsKeyboard, _mAmbientColor);
                }
                SetStaticColor(colorsKeyboardKeys, _mAmbientColor);
                SetStaticColor(colorsKeypad, _mAmbientColor);
                SetStaticColor(colorsMouse, _mAmbientColor);
                SetStaticColor(colorsMousepad, _mAmbientColor);


                BlendAnimations(_mScene,
                    colorsChromaLink, tempColorsChromaLink,
                    colorsHeadset, tempColorsHeadset,
                    colorsKeyboard, tempColorsKeyboard,
                    colorsKeyboardExtended, tempColorsKeyboardExtended,
                    colorsKeypad, tempColorsKeypad,
                    colorsMouse, tempColorsMouse,
                    colorsMousepad, tempColorsMousepad);

                SetKeyColorRGB(colorsKeyboardKeys, (int)Keyboard.RZKEY.RZKEY_W, 255, 255, 0);
                SetKeyColorRGB(colorsKeyboardKeys, (int)Keyboard.RZKEY.RZKEY_A, 255, 255, 0);
                SetKeyColorRGB(colorsKeyboardKeys, (int)Keyboard.RZKEY.RZKEY_S, 255, 255, 0);
                SetKeyColorRGB(colorsKeyboardKeys, (int)Keyboard.RZKEY.RZKEY_D, 255, 255, 0);


                ChromaAnimationAPI.SetEffectCustom1D((int)Device1D.ChromaLink, colorsChromaLink);
                ChromaAnimationAPI.SetEffectCustom1D((int)Device1D.Headset, colorsHeadset);
                ChromaAnimationAPI.SetEffectCustom1D((int)Device1D.Mousepad, colorsMousepad);

                if (_mExtended)
                {
                    ChromaAnimationAPI.SetCustomColorFlag2D((int)Device2D.KeyboardExtended, colorsKeyboardExtended);
                    ChromaAnimationAPI.SetEffectKeyboardCustom2D((int)Device2D.KeyboardExtended, colorsKeyboardExtended, colorsKeyboardKeys);
                }
                else
                {
                    ChromaAnimationAPI.SetCustomColorFlag2D((int)Device2D.Keyboard, colorsKeyboard);
                    ChromaAnimationAPI.SetEffectKeyboardCustom2D((int)Device2D.Keyboard, colorsKeyboard, colorsKeyboardKeys);
                }

                ChromaAnimationAPI.SetEffectCustom2D((int)Device2D.Keypad, colorsKeypad);
                ChromaAnimationAPI.SetEffectCustom2D((int)Device2D.Mouse, colorsMouse);


                Thread.Sleep(33); //30 FPS
                timeMS += 33;
            }

        }

        private void CboItemChanged(object sender, EventArgs e)
        {
            _sMouseMoveStart = Point.Empty;
            _sMouseMoveEnd = Point.Empty;
            _sMouseMoveOffset = Point.Empty;
            _sScreenIndex = _mCboMonitors.SelectedIndex - 1;
        }
    }
}
