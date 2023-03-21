// Ref: https://www.youtube.com/watch?v=6iv7y5v1e0A - C# Capture
// Ref: https://www.youtube.com/watch?v=Q63GV5tnXN0 - Crop Bitmap

using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinForm_RobloxChromaMod
{
    public partial class Form1 : Form
    {
        Image _mCaptureImage = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void _mCaptureTimer_Tick(object sender, EventArgs e)
        {
            // replace clipboard operation later
            SendKeys.Send("{PRTSC}");

            try
            {
                // clipboard operation can fail
                _mCaptureImage = Clipboard.GetImage();

                // do some cropping
                Graphics g = _mPictureBox.CreateGraphics();
                Rectangle rectCropArea = new Rectangle(
                    _mMouseMoveStart.X - _mMouseMoveEnd.X,
                    _mMouseMoveStart.Y - _mMouseMoveEnd.Y,
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

                if (MatchColor(color, 255, 0, 0))
                {
                    _mLabelButtonEffect.Text = "BtnEffect1";
                }
                else if (MatchColor(color, 0, 255, 0))
                {
                    _mLabelButtonEffect.Text = "BtnEffect2";
                }
                else if (MatchColor(color, 0, 0, 255))
                {
                    _mLabelButtonEffect.Text = "BtnEffect3";
                }
                else if (MatchColor(color, 191, 0, 0))
                {
                    _mLabelButtonEffect.Text = "BtnEffect4";
                }
                else if (MatchColor(color, 0, 191, 0))
                {
                    _mLabelButtonEffect.Text = "BtnEffect5";
                }
                else if (MatchColor(color, 0, 0, 191))
                {
                    _mLabelButtonEffect.Text = "BtnEffect6";
                }
                else if (MatchColor(color, 127, 0, 0))
                {
                    _mLabelButtonEffect.Text = "BtnEffect7";
                }
                else if (MatchColor(color, 0, 127, 0))
                {
                    _mLabelButtonEffect.Text = "BtnEffect8";
                }
                else if (MatchColor(color, 0, 0, 127))
                {
                    _mLabelButtonEffect.Text = "BtnEffect9";
                }
                else if (MatchColor(color, 63, 0, 0))
                {
                    _mLabelButtonEffect.Text = "BtnEffect10";
                }
                else if (MatchColor(color, 0, 63, 0))
                {
                    _mLabelButtonEffect.Text = "BtnEffect11";
                }
                else if (MatchColor(color, 0, 0, 63))
                {
                    _mLabelButtonEffect.Text = "BtnEffect12";
                }
                else if (MatchColor(color, 255, 255, 0))
                {
                    _mLabelButtonEffect.Text = "BtnEffect13";
                }
                else if (MatchColor(color, 0, 255, 255))
                {
                    _mLabelButtonEffect.Text = "BtnEffect14";
                }
                else if (MatchColor(color, 255, 0, 255))
                {
                    _mLabelButtonEffect.Text = "BtnEffect15";
                }
                else
                {
                    _mLabelButtonEffect.Text = "None";
                }
            }
            catch
            {
                _mMouseMoveStart = Point.Empty;
                _mMouseMoveEnd = Point.Empty;
                _mMouseMoveOffset = Point.Empty;
            }
        }

        private bool MatchColor(Color color, byte red, byte green, byte blue)
        {
            return (color.R == red && color.G == green && color.B == blue);
        }

        bool _mMouseDown = false;
        bool _mMouseOver = false;
        Point _mMouseMoveStart = Point.Empty;
        Point _mMouseMoveEnd = Point.Empty;
        Point _mMouseMoveOffset = Point.Empty;

        private void PictureMouseDown(object sender, MouseEventArgs e)
        {
            if (!_mMouseDown && _mMouseOver)
            {
                _mMouseDown = true;
                _mMouseMoveStart = new Point(e.X + _mMouseMoveOffset.X, e.Y + _mMouseMoveOffset.Y);
                _mMouseMoveOffset = Point.Empty;
                _mDebugLabel1.Text = string.Format("Start {0},{1}", _mMouseMoveStart.X, _mMouseMoveStart.Y);
            }
        }

        private void PictureMouseUp(object sender, MouseEventArgs e)
        {
            if (_mMouseDown)
            {
                _mMouseMoveEnd = new Point(e.X + _mMouseMoveOffset.X, e.Y + _mMouseMoveOffset.Y);
                _mMouseMoveOffset = new Point(_mMouseMoveStart.X - _mMouseMoveEnd.X, _mMouseMoveStart.Y - _mMouseMoveEnd.Y);
                _mDebugLabel3.Text = string.Format("Offset {0},{1}", _mMouseMoveOffset.X, _mMouseMoveOffset.Y);
            }
            _mMouseDown = false;
        }

        private void PictureMouseMove(object sender, MouseEventArgs e)
        {
            if (_mMouseDown)
            {
                _mMouseMoveEnd = new Point(e.X + _mMouseMoveOffset.X, e.Y + _mMouseMoveOffset.Y);
                _mDebugLabel2.Text = string.Format("End {0},{1}", _mMouseMoveEnd.X, _mMouseMoveEnd.Y);
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
    }
}
