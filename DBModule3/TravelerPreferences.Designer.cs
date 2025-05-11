namespace DBModule3
{
    partial class TravelerPreferences
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TravelerPreferences));
            this.preferencesLabel = new System.Windows.Forms.Label();
            this.preferencesChecklist = new System.Windows.Forms.CheckedListBox();
            this.customPreferenceLabel = new System.Windows.Forms.Label();
            this.customPreferenceTextBox = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.back = new FrameworkTest.SATAButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.title = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.registerbutton = new FrameworkTest.SATAButton();
            this.sataButton1 = new FrameworkTest.SATAButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // preferencesLabel
            // 
            this.preferencesLabel.AutoSize = true;
            this.preferencesLabel.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.preferencesLabel.Location = new System.Drawing.Point(50, 158);
            this.preferencesLabel.Name = "preferencesLabel";
            this.preferencesLabel.Size = new System.Drawing.Size(311, 23);
            this.preferencesLabel.TabIndex = 2;
            this.preferencesLabel.Text = "Select your travel preferences:";
            // 
            // preferencesChecklist
            // 
            this.preferencesChecklist.CheckOnClick = true;
            this.preferencesChecklist.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.preferencesChecklist.Location = new System.Drawing.Point(55, 188);
            this.preferencesChecklist.Name = "preferencesChecklist";
            this.preferencesChecklist.Size = new System.Drawing.Size(400, 236);
            this.preferencesChecklist.TabIndex = 3;
            this.preferencesChecklist.SelectedIndexChanged += new System.EventHandler(this.preferencesChecklist_SelectedIndexChanged);
            // 
            // customPreferenceLabel
            // 
            this.customPreferenceLabel.AutoSize = true;
            this.customPreferenceLabel.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.customPreferenceLabel.Location = new System.Drawing.Point(500, 158);
            this.customPreferenceLabel.Name = "customPreferenceLabel";
            this.customPreferenceLabel.Size = new System.Drawing.Size(251, 23);
            this.customPreferenceLabel.TabIndex = 4;
            this.customPreferenceLabel.Text = "Add custom preference:";
            // 
            // customPreferenceTextBox
            // 
            this.customPreferenceTextBox.Font = new System.Drawing.Font("Microsoft YaHei", 12F);
            this.customPreferenceTextBox.Location = new System.Drawing.Point(504, 188);
            this.customPreferenceTextBox.Name = "customPreferenceTextBox";
            this.customPreferenceTextBox.Size = new System.Drawing.Size(300, 34);
            this.customPreferenceTextBox.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkOrange;
            this.panel1.Controls.Add(this.back);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.title);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1224, 112);
            this.panel1.TabIndex = 8;
            // 
            // back
            // 
            this.back.ButtonText = "BACK";
            this.back.CheckedBackground = System.Drawing.Color.DodgerBlue;
            this.back.CheckedForeColor = System.Drawing.Color.White;
            this.back.CheckedImageTint = System.Drawing.Color.White;
            this.back.CheckedOutline = System.Drawing.Color.Transparent;
            this.back.CustomDialogResult = System.Windows.Forms.DialogResult.None;
            this.back.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.back.HoverBackground = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(255)))), ((int)(((byte)(106)))), ((int)(((byte)(0)))));
            this.back.HoverForeColor = System.Drawing.Color.White;
            this.back.HoverImage = null;
            this.back.HoverImageTint = System.Drawing.Color.White;
            this.back.HoverOutline = System.Drawing.Color.Empty;
            this.back.Image = null;
            this.back.ImageAutoCenter = true;
            this.back.ImageExpand = new System.Drawing.Point(0, 0);
            this.back.ImageOffset = new System.Drawing.Point(0, 0);
            this.back.ImageTint = System.Drawing.Color.White;
            this.back.IsToggleButton = false;
            this.back.IsToggled = false;
            this.back.Location = new System.Drawing.Point(1027, 32);
            this.back.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.back.Name = "back";
            this.back.NormalBackground = System.Drawing.Color.Orange;
            this.back.NormalForeColor = System.Drawing.Color.White;
            this.back.NormalOutline = System.Drawing.Color.Empty;
            this.back.OutlineThickness = 2F;
            this.back.PressedBackground = System.Drawing.Color.RoyalBlue;
            this.back.PressedForeColor = System.Drawing.Color.White;
            this.back.PressedImageTint = System.Drawing.Color.White;
            this.back.PressedOutline = System.Drawing.Color.Empty;
            this.back.Rounding = new System.Windows.Forms.Padding(5);
            this.back.Size = new System.Drawing.Size(167, 58);
            this.back.TabIndex = 7;
            this.back.TextAutoCenter = true;
            this.back.TextOffset = new System.Drawing.Point(0, 0);
            this.back.Click += new System.EventHandler(this.back_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("pictureBox1.BackgroundImage")));
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(-19, -26);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(170, 163);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // title
            // 
            this.title.AutoSize = true;
            this.title.BackColor = System.Drawing.Color.Transparent;
            this.title.Font = new System.Drawing.Font("Segoe UI", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.title.Location = new System.Drawing.Point(157, 9);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(507, 81);
            this.title.TabIndex = 4;
            this.title.Text = "Your Preferences";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 112);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1224, 25);
            this.panel2.TabIndex = 24;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label3.Location = new System.Drawing.Point(514, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(180, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "TRAVEL. COMFORT. LUXURY.";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel3.Controls.Add(this.pictureBox4);
            this.panel3.Controls.Add(this.linkLabel4);
            this.panel3.Controls.Add(this.linkLabel3);
            this.panel3.Controls.Add(this.linkLabel2);
            this.panel3.Controls.Add(this.pictureBox3);
            this.panel3.Controls.Add(this.pictureBox2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.ImeMode = System.Windows.Forms.ImeMode.On;
            this.panel3.Location = new System.Drawing.Point(0, 648);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1224, 40);
            this.panel3.TabIndex = 25;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.BackgroundImage = global::DBModule3.Properties.Resources.bxl_meta;
            this.pictureBox4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox4.Location = new System.Drawing.Point(49, 8);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(29, 27);
            this.pictureBox4.TabIndex = 17;
            this.pictureBox4.TabStop = false;
            // 
            // linkLabel4
            // 
            this.linkLabel4.AutoSize = true;
            this.linkLabel4.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel4.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel4.LinkColor = System.Drawing.Color.White;
            this.linkLabel4.Location = new System.Drawing.Point(850, 14);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(85, 17);
            this.linkLabel4.TabIndex = 16;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "Terms of Use";
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel3.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel3.LinkColor = System.Drawing.Color.White;
            this.linkLabel3.Location = new System.Drawing.Point(1037, 14);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(140, 17);
            this.linkLabel3.TabIndex = 15;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "Privacy And Cookies";
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.BackColor = System.Drawing.Color.Transparent;
            this.linkLabel2.Font = new System.Drawing.Font("Century Gothic", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel2.LinkColor = System.Drawing.Color.White;
            this.linkLabel2.Location = new System.Drawing.Point(941, 14);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(80, 17);
            this.linkLabel2.TabIndex = 14;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Contact Us";
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.BackgroundImage = global::DBModule3.Properties.Resources.bxl_instagram;
            this.pictureBox3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox3.Location = new System.Drawing.Point(9, 5);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(29, 30);
            this.pictureBox3.TabIndex = 12;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = global::DBModule3.Properties.Resources.bxl_discord_alt;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox2.Location = new System.Drawing.Point(87, 7);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(29, 27);
            this.pictureBox2.TabIndex = 11;
            this.pictureBox2.TabStop = false;
            // 
            // registerbutton
            // 
            this.registerbutton.BackColor = System.Drawing.Color.Transparent;
            this.registerbutton.ButtonText = "SAVE PREFERENCES";
            this.registerbutton.CheckedBackground = System.Drawing.Color.DodgerBlue;
            this.registerbutton.CheckedForeColor = System.Drawing.Color.White;
            this.registerbutton.CheckedImageTint = System.Drawing.Color.White;
            this.registerbutton.CheckedOutline = System.Drawing.Color.DodgerBlue;
            this.registerbutton.CustomDialogResult = System.Windows.Forms.DialogResult.None;
            this.registerbutton.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.registerbutton.HoverBackground = System.Drawing.Color.Black;
            this.registerbutton.HoverForeColor = System.Drawing.Color.White;
            this.registerbutton.HoverImage = null;
            this.registerbutton.HoverImageTint = System.Drawing.Color.White;
            this.registerbutton.HoverOutline = System.Drawing.Color.Empty;
            this.registerbutton.Image = null;
            this.registerbutton.ImageAutoCenter = true;
            this.registerbutton.ImageExpand = new System.Drawing.Point(0, 0);
            this.registerbutton.ImageOffset = new System.Drawing.Point(0, 0);
            this.registerbutton.ImageTint = System.Drawing.Color.White;
            this.registerbutton.IsToggleButton = false;
            this.registerbutton.IsToggled = false;
            this.registerbutton.Location = new System.Drawing.Point(55, 449);
            this.registerbutton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.registerbutton.Name = "registerbutton";
            this.registerbutton.NormalBackground = System.Drawing.Color.DarkBlue;
            this.registerbutton.NormalForeColor = System.Drawing.Color.White;
            this.registerbutton.NormalOutline = System.Drawing.Color.Empty;
            this.registerbutton.OutlineThickness = 2F;
            this.registerbutton.PressedBackground = System.Drawing.Color.RoyalBlue;
            this.registerbutton.PressedForeColor = System.Drawing.Color.White;
            this.registerbutton.PressedImageTint = System.Drawing.Color.White;
            this.registerbutton.PressedOutline = System.Drawing.Color.Empty;
            this.registerbutton.Rounding = new System.Windows.Forms.Padding(5);
            this.registerbutton.Size = new System.Drawing.Size(244, 65);
            this.registerbutton.TabIndex = 26;
            this.registerbutton.TextAutoCenter = true;
            this.registerbutton.TextOffset = new System.Drawing.Point(0, 0);
            this.registerbutton.Click += new System.EventHandler(this.registerbutton_Click);
            // 
            // sataButton1
            // 
            this.sataButton1.BackColor = System.Drawing.Color.Transparent;
            this.sataButton1.ButtonText = "ADD";
            this.sataButton1.CheckedBackground = System.Drawing.Color.DodgerBlue;
            this.sataButton1.CheckedForeColor = System.Drawing.Color.White;
            this.sataButton1.CheckedImageTint = System.Drawing.Color.White;
            this.sataButton1.CheckedOutline = System.Drawing.Color.DodgerBlue;
            this.sataButton1.CustomDialogResult = System.Windows.Forms.DialogResult.None;
            this.sataButton1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sataButton1.HoverBackground = System.Drawing.Color.Black;
            this.sataButton1.HoverForeColor = System.Drawing.Color.White;
            this.sataButton1.HoverImage = null;
            this.sataButton1.HoverImageTint = System.Drawing.Color.White;
            this.sataButton1.HoverOutline = System.Drawing.Color.Empty;
            this.sataButton1.Image = null;
            this.sataButton1.ImageAutoCenter = true;
            this.sataButton1.ImageExpand = new System.Drawing.Point(0, 0);
            this.sataButton1.ImageOffset = new System.Drawing.Point(0, 0);
            this.sataButton1.ImageTint = System.Drawing.Color.White;
            this.sataButton1.IsToggleButton = false;
            this.sataButton1.IsToggled = false;
            this.sataButton1.Location = new System.Drawing.Point(504, 241);
            this.sataButton1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.sataButton1.Name = "sataButton1";
            this.sataButton1.NormalBackground = System.Drawing.Color.DarkBlue;
            this.sataButton1.NormalForeColor = System.Drawing.Color.White;
            this.sataButton1.NormalOutline = System.Drawing.Color.Empty;
            this.sataButton1.OutlineThickness = 2F;
            this.sataButton1.PressedBackground = System.Drawing.Color.RoyalBlue;
            this.sataButton1.PressedForeColor = System.Drawing.Color.White;
            this.sataButton1.PressedImageTint = System.Drawing.Color.White;
            this.sataButton1.PressedOutline = System.Drawing.Color.Empty;
            this.sataButton1.Rounding = new System.Windows.Forms.Padding(5);
            this.sataButton1.Size = new System.Drawing.Size(244, 65);
            this.sataButton1.TabIndex = 27;
            this.sataButton1.TextAutoCenter = true;
            this.sataButton1.TextOffset = new System.Drawing.Point(0, 0);
            this.sataButton1.Click += new System.EventHandler(this.sataButton1_Click);
            // 
            // TravelerPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSkyBlue;
            this.ClientSize = new System.Drawing.Size(1224, 688);
            this.Controls.Add(this.sataButton1);
            this.Controls.Add(this.registerbutton);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.preferencesLabel);
            this.Controls.Add(this.preferencesChecklist);
            this.Controls.Add(this.customPreferenceLabel);
            this.Controls.Add(this.customPreferenceTextBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "TravelerPreferences";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Travel Preferences";
            this.Load += new System.EventHandler(this.TravelerPreferences_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label preferencesLabel;
        private System.Windows.Forms.CheckedListBox preferencesChecklist;
        private System.Windows.Forms.Label customPreferenceLabel;
        private System.Windows.Forms.TextBox customPreferenceTextBox;
        private System.Windows.Forms.Panel panel1;
        private FrameworkTest.SATAButton back;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label title;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.LinkLabel linkLabel4;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private FrameworkTest.SATAButton registerbutton;
        private FrameworkTest.SATAButton sataButton1;
    }
}