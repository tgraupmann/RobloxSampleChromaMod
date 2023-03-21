namespace WinForm_RobloxChromaMod
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this._mPictureBox = new System.Windows.Forms.PictureBox();
            this._mCaptureTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this._mPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _mPictureBox
            // 
            this._mPictureBox.Location = new System.Drawing.Point(67, 113);
            this._mPictureBox.Name = "_mPictureBox";
            this._mPictureBox.Size = new System.Drawing.Size(663, 278);
            this._mPictureBox.TabIndex = 0;
            this._mPictureBox.TabStop = false;
            this._mPictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureMouseDown);
            this._mPictureBox.MouseEnter += new System.EventHandler(this.PictureMouseEnter);
            this._mPictureBox.MouseLeave += new System.EventHandler(this.PictureMouseLeave);
            this._mPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureMouseMove);
            this._mPictureBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureMouseUp);
            // 
            // _mCaptureTimer
            // 
            this._mCaptureTimer.Enabled = true;
            this._mCaptureTimer.Interval = 1000;
            this._mCaptureTimer.Tick += new System.EventHandler(this._mCaptureTimer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this._mPictureBox);
            this.Name = "Form1";
            this.Text = "Roblox Companion App";
            ((System.ComponentModel.ISupportInitialize)(this._mPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox _mPictureBox;
        private System.Windows.Forms.Timer _mCaptureTimer;
    }
}

