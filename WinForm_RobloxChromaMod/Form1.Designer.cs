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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this._mPictureBox = new System.Windows.Forms.PictureBox();
            this._mCaptureTimer = new System.Windows.Forms.Timer(this.components);
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
            this._mPictureBox.Location = new System.Drawing.Point(11, 12);
            this._mPictureBox.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this._mPictureBox.Name = "_mPictureBox";
            this._mPictureBox.Size = new System.Drawing.Size(100, 100);
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(155, 45);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Button Effect Color";
            // 
            // _mButtonColor
            // 
            this._mButtonColor.AutoSize = true;
            this._mButtonColor.Location = new System.Drawing.Point(155, 59);
            this._mButtonColor.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this._mButtonColor.Name = "_mButtonColor";
            this._mButtonColor.Size = new System.Drawing.Size(31, 13);
            this._mButtonColor.TabIndex = 5;
            this._mButtonColor.Text = "Color";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(155, 84);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Current Button Effect";
            // 
            // _mLabelButtonEffect
            // 
            this._mLabelButtonEffect.AutoSize = true;
            this._mLabelButtonEffect.Location = new System.Drawing.Point(155, 98);
            this._mLabelButtonEffect.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this._mLabelButtonEffect.Name = "_mLabelButtonEffect";
            this._mLabelButtonEffect.Size = new System.Drawing.Size(52, 13);
            this._mLabelButtonEffect.TabIndex = 7;
            this._mLabelButtonEffect.Text = "No Effect";
            // 
            // _mLblDebugSwimming
            // 
            this._mLblDebugSwimming.AutoSize = true;
            this._mLblDebugSwimming.Location = new System.Drawing.Point(155, 171);
            this._mLblDebugSwimming.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this._mLblDebugSwimming.Name = "_mLblDebugSwimming";
            this._mLblDebugSwimming.Size = new System.Drawing.Size(82, 13);
            this._mLblDebugSwimming.TabIndex = 8;
            this._mLblDebugSwimming.Text = "Swimming: false";
            // 
            // _mLblDebugDead
            // 
            this._mLblDebugDead.AutoSize = true;
            this._mLblDebugDead.Location = new System.Drawing.Point(155, 128);
            this._mLblDebugDead.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this._mLblDebugDead.Name = "_mLblDebugDead";
            this._mLblDebugDead.Size = new System.Drawing.Size(61, 13);
            this._mLblDebugDead.TabIndex = 9;
            this._mLblDebugDead.Text = "Dead: false";
            // 
            // _mLblDebugClimbing
            // 
            this._mLblDebugClimbing.AutoSize = true;
            this._mLblDebugClimbing.Location = new System.Drawing.Point(155, 150);
            this._mLblDebugClimbing.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this._mLblDebugClimbing.Name = "_mLblDebugClimbing";
            this._mLblDebugClimbing.Size = new System.Drawing.Size(74, 13);
            this._mLblDebugClimbing.TabIndex = 10;
            this._mLblDebugClimbing.Text = "Climbing: false";
            // 
            // _mLblDebugSeated
            // 
            this._mLblDebugSeated.AutoSize = true;
            this._mLblDebugSeated.Location = new System.Drawing.Point(155, 194);
            this._mLblDebugSeated.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this._mLblDebugSeated.Name = "_mLblDebugSeated";
            this._mLblDebugSeated.Size = new System.Drawing.Size(69, 13);
            this._mLblDebugSeated.TabIndex = 11;
            this._mLblDebugSeated.Text = "Seated: false";
            // 
            // _mLblDebugFlying
            // 
            this._mLblDebugFlying.AutoSize = true;
            this._mLblDebugFlying.Location = new System.Drawing.Point(155, 238);
            this._mLblDebugFlying.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this._mLblDebugFlying.Name = "_mLblDebugFlying";
            this._mLblDebugFlying.Size = new System.Drawing.Size(62, 13);
            this._mLblDebugFlying.TabIndex = 12;
            this._mLblDebugFlying.Text = "Flying: false";
            // 
            // _mLblDebugJumping
            // 
            this._mLblDebugJumping.AutoSize = true;
            this._mLblDebugJumping.Location = new System.Drawing.Point(155, 216);
            this._mLblDebugJumping.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this._mLblDebugJumping.Name = "_mLblDebugJumping";
            this._mLblDebugJumping.Size = new System.Drawing.Size(74, 13);
            this._mLblDebugJumping.TabIndex = 13;
            this._mLblDebugJumping.Text = "Jumping: false";
            // 
            // _mLblDebugRunning
            // 
            this._mLblDebugRunning.AutoSize = true;
            this._mLblDebugRunning.Location = new System.Drawing.Point(155, 262);
            this._mLblDebugRunning.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this._mLblDebugRunning.Name = "_mLblDebugRunning";
            this._mLblDebugRunning.Size = new System.Drawing.Size(75, 13);
            this._mLblDebugRunning.TabIndex = 14;
            this._mLblDebugRunning.Text = "Running: false";
            // 
            // _mCboMonitors
            // 
            this._mCboMonitors.FormattingEnabled = true;
            this._mCboMonitors.Items.AddRange(new object[] {
            "--SELECT MONITOR--"});
            this._mCboMonitors.Location = new System.Drawing.Point(155, 11);
            this._mCboMonitors.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this._mCboMonitors.Name = "_mCboMonitors";
            this._mCboMonitors.Size = new System.Drawing.Size(164, 21);
            this._mCboMonitors.TabIndex = 15;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 294);
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
            this.Controls.Add(this._mPictureBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
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

