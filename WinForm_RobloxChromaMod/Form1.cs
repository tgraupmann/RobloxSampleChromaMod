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
            _mPictureBox.Hide();
            SendKeys.Send("{PRTSC}");
            _mCaptureImage = Clipboard.GetImage();
            _mPictureBox.Image = _mCaptureImage;
            _mPictureBox.Show();

            if (_mMouseDown && _mMouseOver)
            {

            }
        }

        bool _mMouseDown = false;
        bool _mMouseOver = false;
        Point _mMouseMoveStart = Point.Empty;
        Point _mMouseMoveEnd = Point.Empty;

        private void PictureMouseDown(object sender, MouseEventArgs e)
        {
            _mMouseDown = true;
            _mMouseMoveStart = new Point(e.X, e.Y);
        }

        private void PictureMouseUp(object sender, MouseEventArgs e)
        {
            _mMouseDown = false;
        }

        private void PictureMouseMove(object sender, MouseEventArgs e)
        {
            _mMouseMoveEnd = new Point(e.X, e.Y);
            if (_mMouseDown && _mMouseOver)
            {
                _mPictureBox.Location = 
                    new Point(_mMouseMoveEnd.X - _mMouseMoveStart.X,
                    _mMouseMoveEnd.Y - _mMouseMoveStart.Y);
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
