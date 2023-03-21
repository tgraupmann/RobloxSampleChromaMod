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
            this._mDebugLabel1 = new System.Windows.Forms.Label();
            this._mDebugLabel2 = new System.Windows.Forms.Label();
            this._mDebugLabel3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this._mButtonColor = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this._mLabelButtonEffect = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this._mPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _mPictureBox
            // 
            this._mPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._mPictureBox.Location = new System.Drawing.Point(12, 12);
            this._mPictureBox.Name = "_mPictureBox";
            this._mPictureBox.Size = new System.Drawing.Size(400, 400);
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
            this._mCaptureTimer.Tick += new System.EventHandler(this._mCaptureTimer_Tick);
            // 
            // _mDebugLabel1
            // 
            this._mDebugLabel1.AutoSize = true;
            this._mDebugLabel1.Location = new System.Drawing.Point(442, 12);
            this._mDebugLabel1.Name = "_mDebugLabel1";
            this._mDebugLabel1.Size = new System.Drawing.Size(58, 17);
            this._mDebugLabel1.TabIndex = 1;
            this._mDebugLabel1.Text = "Position";
            // 
            // _mDebugLabel2
            // 
            this._mDebugLabel2.AutoSize = true;
            this._mDebugLabel2.Location = new System.Drawing.Point(442, 62);
            this._mDebugLabel2.Name = "_mDebugLabel2";
            this._mDebugLabel2.Size = new System.Drawing.Size(38, 17);
            this._mDebugLabel2.TabIndex = 2;
            this._mDebugLabel2.Text = "Start";
            // 
            // _mDebugLabel3
            // 
            this._mDebugLabel3.AutoSize = true;
            this._mDebugLabel3.Location = new System.Drawing.Point(442, 110);
            this._mDebugLabel3.Name = "_mDebugLabel3";
            this._mDebugLabel3.Size = new System.Drawing.Size(33, 17);
            this._mDebugLabel3.TabIndex = 3;
            this._mDebugLabel3.Text = "End";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(442, 160);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Button Effect Color";
            // 
            // _mButtonColor
            // 
            this._mButtonColor.AutoSize = true;
            this._mButtonColor.Location = new System.Drawing.Point(442, 187);
            this._mButtonColor.Name = "_mButtonColor";
            this._mButtonColor.Size = new System.Drawing.Size(41, 17);
            this._mButtonColor.TabIndex = 5;
            this._mButtonColor.Text = "Color";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(442, 234);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "Current Button Effect";
            // 
            // _mLabelButtonEffect
            // 
            this._mLabelButtonEffect.AutoSize = true;
            this._mLabelButtonEffect.Location = new System.Drawing.Point(442, 260);
            this._mLabelButtonEffect.Name = "_mLabelButtonEffect";
            this._mLabelButtonEffect.Size = new System.Drawing.Size(66, 17);
            this._mLabelButtonEffect.TabIndex = 7;
            this._mLabelButtonEffect.Text = "No Effect";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 421);
            this.Controls.Add(this._mLabelButtonEffect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._mButtonColor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._mDebugLabel3);
            this.Controls.Add(this._mDebugLabel2);
            this.Controls.Add(this._mDebugLabel1);
            this.Controls.Add(this._mPictureBox);
            this.Name = "Form1";
            this.Text = "Roblox Companion App";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this._mPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox _mPictureBox;
        private System.Windows.Forms.Timer _mCaptureTimer;
        private System.Windows.Forms.Label _mDebugLabel1;
        private System.Windows.Forms.Label _mDebugLabel2;
        private System.Windows.Forms.Label _mDebugLabel3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label _mButtonColor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label _mLabelButtonEffect;
    }
}

