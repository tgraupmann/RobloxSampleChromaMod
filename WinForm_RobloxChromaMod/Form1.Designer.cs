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
            this._mLblDebugSwimming = new System.Windows.Forms.Label();
            this._mLblDebugDead = new System.Windows.Forms.Label();
            this._mLblDebugClimbing = new System.Windows.Forms.Label();
            this._mLblDebugSeated = new System.Windows.Forms.Label();
            this._mLblDebugFlying = new System.Windows.Forms.Label();
            this._mLblDebugJumping = new System.Windows.Forms.Label();
            this._mLblDebugRunning = new System.Windows.Forms.Label();
            this._mCboMonitors = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this._mPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // _mPictureBox
            // 
            this._mPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this._mPictureBox.Location = new System.Drawing.Point(18, 19);
            this._mPictureBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this._mPictureBox.Name = "_mPictureBox";
            this._mPictureBox.Size = new System.Drawing.Size(598, 623);
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
            this._mDebugLabel1.Location = new System.Drawing.Point(661, 105);
            this._mDebugLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._mDebugLabel1.Name = "_mDebugLabel1";
            this._mDebugLabel1.Size = new System.Drawing.Size(89, 25);
            this._mDebugLabel1.TabIndex = 1;
            this._mDebugLabel1.Text = "Position";
            // 
            // _mDebugLabel2
            // 
            this._mDebugLabel2.AutoSize = true;
            this._mDebugLabel2.Location = new System.Drawing.Point(661, 131);
            this._mDebugLabel2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._mDebugLabel2.Name = "_mDebugLabel2";
            this._mDebugLabel2.Size = new System.Drawing.Size(57, 25);
            this._mDebugLabel2.TabIndex = 2;
            this._mDebugLabel2.Text = "Start";
            // 
            // _mDebugLabel3
            // 
            this._mDebugLabel3.AutoSize = true;
            this._mDebugLabel3.Location = new System.Drawing.Point(661, 158);
            this._mDebugLabel3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._mDebugLabel3.Name = "_mDebugLabel3";
            this._mDebugLabel3.Size = new System.Drawing.Size(50, 25);
            this._mDebugLabel3.TabIndex = 3;
            this._mDebugLabel3.Text = "End";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(661, 205);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(192, 25);
            this.label1.TabIndex = 4;
            this.label1.Text = "Button Effect Color";
            // 
            // _mButtonColor
            // 
            this._mButtonColor.AutoSize = true;
            this._mButtonColor.Location = new System.Drawing.Point(661, 231);
            this._mButtonColor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._mButtonColor.Name = "_mButtonColor";
            this._mButtonColor.Size = new System.Drawing.Size(63, 25);
            this._mButtonColor.TabIndex = 5;
            this._mButtonColor.Text = "Color";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(661, 280);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(212, 25);
            this.label2.TabIndex = 6;
            this.label2.Text = "Current Button Effect";
            // 
            // _mLabelButtonEffect
            // 
            this._mLabelButtonEffect.AutoSize = true;
            this._mLabelButtonEffect.Location = new System.Drawing.Point(661, 306);
            this._mLabelButtonEffect.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._mLabelButtonEffect.Name = "_mLabelButtonEffect";
            this._mLabelButtonEffect.Size = new System.Drawing.Size(100, 25);
            this._mLabelButtonEffect.TabIndex = 7;
            this._mLabelButtonEffect.Text = "No Effect";
            // 
            // _mLblDebugSwimming
            // 
            this._mLblDebugSwimming.AutoSize = true;
            this._mLblDebugSwimming.Location = new System.Drawing.Point(661, 447);
            this._mLblDebugSwimming.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._mLblDebugSwimming.Name = "_mLblDebugSwimming";
            this._mLblDebugSwimming.Size = new System.Drawing.Size(167, 25);
            this._mLblDebugSwimming.TabIndex = 8;
            this._mLblDebugSwimming.Text = "Swimming: false";
            // 
            // _mLblDebugDead
            // 
            this._mLblDebugDead.AutoSize = true;
            this._mLblDebugDead.Location = new System.Drawing.Point(661, 364);
            this._mLblDebugDead.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._mLblDebugDead.Name = "_mLblDebugDead";
            this._mLblDebugDead.Size = new System.Drawing.Size(121, 25);
            this._mLblDebugDead.TabIndex = 9;
            this._mLblDebugDead.Text = "Dead: false";
            // 
            // _mLblDebugClimbing
            // 
            this._mLblDebugClimbing.AutoSize = true;
            this._mLblDebugClimbing.Location = new System.Drawing.Point(661, 406);
            this._mLblDebugClimbing.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._mLblDebugClimbing.Name = "_mLblDebugClimbing";
            this._mLblDebugClimbing.Size = new System.Drawing.Size(153, 25);
            this._mLblDebugClimbing.TabIndex = 10;
            this._mLblDebugClimbing.Text = "Climbing: false";
            // 
            // _mLblDebugSeated
            // 
            this._mLblDebugSeated.AutoSize = true;
            this._mLblDebugSeated.Location = new System.Drawing.Point(661, 491);
            this._mLblDebugSeated.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._mLblDebugSeated.Name = "_mLblDebugSeated";
            this._mLblDebugSeated.Size = new System.Drawing.Size(138, 25);
            this._mLblDebugSeated.TabIndex = 11;
            this._mLblDebugSeated.Text = "Seated: false";
            // 
            // _mLblDebugFlying
            // 
            this._mLblDebugFlying.AutoSize = true;
            this._mLblDebugFlying.Location = new System.Drawing.Point(661, 577);
            this._mLblDebugFlying.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._mLblDebugFlying.Name = "_mLblDebugFlying";
            this._mLblDebugFlying.Size = new System.Drawing.Size(128, 25);
            this._mLblDebugFlying.TabIndex = 12;
            this._mLblDebugFlying.Text = "Flying: false";
            // 
            // _mLblDebugJumping
            // 
            this._mLblDebugJumping.AutoSize = true;
            this._mLblDebugJumping.Location = new System.Drawing.Point(661, 534);
            this._mLblDebugJumping.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._mLblDebugJumping.Name = "_mLblDebugJumping";
            this._mLblDebugJumping.Size = new System.Drawing.Size(151, 25);
            this._mLblDebugJumping.TabIndex = 13;
            this._mLblDebugJumping.Text = "Jumping: false";
            // 
            // _mLblDebugRunning
            // 
            this._mLblDebugRunning.AutoSize = true;
            this._mLblDebugRunning.Location = new System.Drawing.Point(661, 622);
            this._mLblDebugRunning.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._mLblDebugRunning.Name = "_mLblDebugRunning";
            this._mLblDebugRunning.Size = new System.Drawing.Size(150, 25);
            this._mLblDebugRunning.TabIndex = 14;
            this._mLblDebugRunning.Text = "Running: false";
            // 
            // _mCboMonitors
            // 
            this._mCboMonitors.FormattingEnabled = true;
            this._mCboMonitors.Items.AddRange(new object[] {
            "--SELECT MONITOR--"});
            this._mCboMonitors.Location = new System.Drawing.Point(666, 19);
            this._mCboMonitors.Name = "_mCboMonitors";
            this._mCboMonitors.Size = new System.Drawing.Size(325, 33);
            this._mCboMonitors.TabIndex = 15;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1038, 658);
            this.Controls.Add(this._mCboMonitors);
            this.Controls.Add(this._mLblDebugRunning);
            this.Controls.Add(this._mLblDebugJumping);
            this.Controls.Add(this._mLblDebugFlying);
            this.Controls.Add(this._mLblDebugSeated);
            this.Controls.Add(this._mLblDebugClimbing);
            this.Controls.Add(this._mLblDebugDead);
            this.Controls.Add(this._mLblDebugSwimming);
            this.Controls.Add(this._mLabelButtonEffect);
            this.Controls.Add(this.label2);
            this.Controls.Add(this._mButtonColor);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._mDebugLabel3);
            this.Controls.Add(this._mDebugLabel2);
            this.Controls.Add(this._mDebugLabel1);
            this.Controls.Add(this._mPictureBox);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Roblox Companion App";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
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
        private System.Windows.Forms.Label _mLblDebugSwimming;
        private System.Windows.Forms.Label _mLblDebugDead;
        private System.Windows.Forms.Label _mLblDebugClimbing;
        private System.Windows.Forms.Label _mLblDebugSeated;
        private System.Windows.Forms.Label _mLblDebugFlying;
        private System.Windows.Forms.Label _mLblDebugJumping;
        private System.Windows.Forms.Label _mLblDebugRunning;
        private System.Windows.Forms.ComboBox _mCboMonitors;
    }
}

