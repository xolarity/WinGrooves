namespace WinGrooves
{
    partial class OptionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsForm));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.hotkeyControlPlay = new exscape.HotkeyControl();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.hotkeyControlNext = new exscape.HotkeyControl();
            this.label3 = new System.Windows.Forms.Label();
            this.hotkeyControlPrevious = new exscape.HotkeyControl();
            this.label4 = new System.Windows.Forms.Label();
            this.hotkeyControlLike = new exscape.HotkeyControl();
            this.label5 = new System.Windows.Forms.Label();
            this.hotkeyControlDislike = new exscape.HotkeyControl();
            this.label6 = new System.Windows.Forms.Label();
            this.hotkeyControlFavorite = new exscape.HotkeyControl();
            this.label7 = new System.Windows.Forms.Label();
            this.hotkeyControlShowHide = new exscape.HotkeyControl();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.hotkeyControlShuffle = new exscape.HotkeyControl();
            this.label9 = new System.Windows.Forms.Label();
            this.hotkeyControlMute = new exscape.HotkeyControl();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(119, 368);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(200, 368);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // hotkeyControlPlay
            // 
            this.hotkeyControlPlay.Hotkey = System.Windows.Forms.Keys.None;
            this.hotkeyControlPlay.HotkeyModifiers = System.Windows.Forms.Keys.None;
            this.hotkeyControlPlay.Location = new System.Drawing.Point(120, 114);
            this.hotkeyControlPlay.Name = "hotkeyControlPlay";
            this.hotkeyControlPlay.Size = new System.Drawing.Size(155, 20);
            this.hotkeyControlPlay.TabIndex = 6;
            this.hotkeyControlPlay.Text = "None";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Global Hot Keys";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Play / Pause:";
            // 
            // hotkeyControlNext
            // 
            this.hotkeyControlNext.Hotkey = System.Windows.Forms.Keys.None;
            this.hotkeyControlNext.HotkeyModifiers = System.Windows.Forms.Keys.None;
            this.hotkeyControlNext.Location = new System.Drawing.Point(120, 141);
            this.hotkeyControlNext.Name = "hotkeyControlNext";
            this.hotkeyControlNext.Size = new System.Drawing.Size(155, 20);
            this.hotkeyControlNext.TabIndex = 6;
            this.hotkeyControlNext.Text = "None";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 144);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Next Song:";
            // 
            // hotkeyControlPrevious
            // 
            this.hotkeyControlPrevious.Hotkey = System.Windows.Forms.Keys.None;
            this.hotkeyControlPrevious.HotkeyModifiers = System.Windows.Forms.Keys.None;
            this.hotkeyControlPrevious.Location = new System.Drawing.Point(119, 167);
            this.hotkeyControlPrevious.Name = "hotkeyControlPrevious";
            this.hotkeyControlPrevious.Size = new System.Drawing.Size(155, 20);
            this.hotkeyControlPrevious.TabIndex = 6;
            this.hotkeyControlPrevious.Text = "None";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 170);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Previous song:";
            // 
            // hotkeyControlLike
            // 
            this.hotkeyControlLike.Hotkey = System.Windows.Forms.Keys.None;
            this.hotkeyControlLike.HotkeyModifiers = System.Windows.Forms.Keys.None;
            this.hotkeyControlLike.Location = new System.Drawing.Point(119, 193);
            this.hotkeyControlLike.Name = "hotkeyControlLike";
            this.hotkeyControlLike.Size = new System.Drawing.Size(155, 20);
            this.hotkeyControlLike.TabIndex = 6;
            this.hotkeyControlLike.Text = "None";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 196);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Like Song:";
            // 
            // hotkeyControlDislike
            // 
            this.hotkeyControlDislike.Hotkey = System.Windows.Forms.Keys.None;
            this.hotkeyControlDislike.HotkeyModifiers = System.Windows.Forms.Keys.None;
            this.hotkeyControlDislike.Location = new System.Drawing.Point(119, 219);
            this.hotkeyControlDislike.Name = "hotkeyControlDislike";
            this.hotkeyControlDislike.Size = new System.Drawing.Size(155, 20);
            this.hotkeyControlDislike.TabIndex = 6;
            this.hotkeyControlDislike.Text = "None";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 222);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(67, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Dislike song:";
            // 
            // hotkeyControlFavorite
            // 
            this.hotkeyControlFavorite.Hotkey = System.Windows.Forms.Keys.None;
            this.hotkeyControlFavorite.HotkeyModifiers = System.Windows.Forms.Keys.None;
            this.hotkeyControlFavorite.Location = new System.Drawing.Point(119, 245);
            this.hotkeyControlFavorite.Name = "hotkeyControlFavorite";
            this.hotkeyControlFavorite.Size = new System.Drawing.Size(155, 20);
            this.hotkeyControlFavorite.TabIndex = 6;
            this.hotkeyControlFavorite.Text = "None";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 248);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Favorite Song:";
            // 
            // hotkeyControlShowHide
            // 
            this.hotkeyControlShowHide.Hotkey = System.Windows.Forms.Keys.None;
            this.hotkeyControlShowHide.HotkeyModifiers = System.Windows.Forms.Keys.None;
            this.hotkeyControlShowHide.Location = new System.Drawing.Point(119, 271);
            this.hotkeyControlShowHide.Name = "hotkeyControlShowHide";
            this.hotkeyControlShowHide.Size = new System.Drawing.Size(155, 20);
            this.hotkeyControlShowHide.TabIndex = 6;
            this.hotkeyControlShowHide.Text = "None";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 274);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(106, 13);
            this.label8.TabIndex = 8;
            this.label8.Text = "Show/Hide Window:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 326);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "Shuffle:";
            // 
            // hotkeyControlShuffle
            // 
            this.hotkeyControlShuffle.Hotkey = System.Windows.Forms.Keys.None;
            this.hotkeyControlShuffle.HotkeyModifiers = System.Windows.Forms.Keys.None;
            this.hotkeyControlShuffle.Location = new System.Drawing.Point(120, 323);
            this.hotkeyControlShuffle.Name = "hotkeyControlShuffle";
            this.hotkeyControlShuffle.Size = new System.Drawing.Size(155, 20);
            this.hotkeyControlShuffle.TabIndex = 15;
            this.hotkeyControlShuffle.Text = "None";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 300);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(34, 13);
            this.label9.TabIndex = 14;
            this.label9.Text = "Mute:";
            // 
            // hotkeyControlMute
            // 
            this.hotkeyControlMute.Hotkey = System.Windows.Forms.Keys.None;
            this.hotkeyControlMute.HotkeyModifiers = System.Windows.Forms.Keys.None;
            this.hotkeyControlMute.Location = new System.Drawing.Point(120, 297);
            this.hotkeyControlMute.Name = "hotkeyControlMute";
            this.hotkeyControlMute.Size = new System.Drawing.Size(155, 20);
            this.hotkeyControlMute.TabIndex = 13;
            this.hotkeyControlMute.Text = "None";
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Checked = global::WinGrooves.Properties.Settings.Default.trayClose;
            this.checkBox3.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::WinGrooves.Properties.Settings.Default, "trayClose", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox3.Location = new System.Drawing.Point(12, 35);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(84, 17);
            this.checkBox3.TabIndex = 18;
            this.checkBox3.Text = "Close to tray";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = global::WinGrooves.Properties.Settings.Default.startMinimized;
            this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::WinGrooves.Properties.Settings.Default, "startMinimized", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox1.Location = new System.Drawing.Point(12, 58);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(96, 17);
            this.checkBox1.TabIndex = 17;
            this.checkBox1.Text = "Start minimized";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = global::WinGrooves.Properties.Settings.Default.trayMinimize;
            this.checkBox2.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::WinGrooves.Properties.Settings.Default, "trayMinimize", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.checkBox2.Location = new System.Drawing.Point(12, 12);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(143, 17);
            this.checkBox2.TabIndex = 5;
            this.checkBox2.Text = "Minimize To System Tray";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // OptionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button2;
            this.ClientSize = new System.Drawing.Size(284, 412);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.hotkeyControlShuffle);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.hotkeyControlMute);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.hotkeyControlShowHide);
            this.Controls.Add(this.hotkeyControlFavorite);
            this.Controls.Add(this.hotkeyControlDislike);
            this.Controls.Add(this.hotkeyControlLike);
            this.Controls.Add(this.hotkeyControlPrevious);
            this.Controls.Add(this.hotkeyControlNext);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.hotkeyControlPlay);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(300, 450);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 300);
            this.Name = "OptionsForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WinGrooves Options";
            this.Load += new System.EventHandler(this.OptionsForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox2;
        private exscape.HotkeyControl hotkeyControlPlay;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private exscape.HotkeyControl hotkeyControlNext;
		private System.Windows.Forms.Label label3;
		private exscape.HotkeyControl hotkeyControlPrevious;
		private System.Windows.Forms.Label label4;
		private exscape.HotkeyControl hotkeyControlLike;
		private System.Windows.Forms.Label label5;
		private exscape.HotkeyControl hotkeyControlDislike;
		private System.Windows.Forms.Label label6;
		private exscape.HotkeyControl hotkeyControlFavorite;
		private System.Windows.Forms.Label label7;
		private exscape.HotkeyControl hotkeyControlShowHide;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label10;
		private exscape.HotkeyControl hotkeyControlShuffle;
		private System.Windows.Forms.Label label9;
		private exscape.HotkeyControl hotkeyControlMute;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox3;
    }
}