namespace DBModule3
{
    partial class SearchBar
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.sataTextBox1 = new SATATextBox();
            this.searchbutton = new FrameworkTest.SATAButton();
            this.TripIDbox = new SATATextBox();
            this.BookButton = new FrameworkTest.SATAButton();
            this.SuspendLayout();
            // 
            // sataTextBox1
            // 
            this.sataTextBox1.BorderColor = System.Drawing.Color.Transparent;
            this.sataTextBox1.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(68)))), ((int)(((byte)(142)))));
            this.sataTextBox1.BorderRadius = 5;
            this.sataTextBox1.BorderSize = 3;
            this.sataTextBox1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sataTextBox1.Icon = null;
            this.sataTextBox1.IconSize = new System.Drawing.Size(20, 20);
            this.sataTextBox1.Location = new System.Drawing.Point(374, 12);
            this.sataTextBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.sataTextBox1.Multiline = false;
            this.sataTextBox1.Name = "sataTextBox1";
            this.sataTextBox1.PasswordChar = false;
            this.sataTextBox1.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.sataTextBox1.PlaceholderText = "Search";
            this.sataTextBox1.Size = new System.Drawing.Size(267, 43);
            this.sataTextBox1.TabIndex = 1;
            this.sataTextBox1.Text = "searchbox";
            this.sataTextBox1.Texts = "";
            this.sataTextBox1.UnderlinedStyle = false;
            this.sataTextBox1.Click += new System.EventHandler(this.sataTextBox1_Click);
            // 
            // searchbutton
            // 
            this.searchbutton.BackColor = System.Drawing.Color.DarkGray;
            this.searchbutton.ButtonText = "SEARCH";
            this.searchbutton.CheckedBackground = System.Drawing.Color.DodgerBlue;
            this.searchbutton.CheckedForeColor = System.Drawing.Color.Transparent;
            this.searchbutton.CheckedImageTint = System.Drawing.Color.White;
            this.searchbutton.CheckedOutline = System.Drawing.Color.Transparent;
            this.searchbutton.CustomDialogResult = System.Windows.Forms.DialogResult.None;
            this.searchbutton.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.searchbutton.HoverBackground = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(255)))), ((int)(((byte)(106)))), ((int)(((byte)(0)))));
            this.searchbutton.HoverForeColor = System.Drawing.Color.White;
            this.searchbutton.HoverImage = null;
            this.searchbutton.HoverImageTint = System.Drawing.Color.White;
            this.searchbutton.HoverOutline = System.Drawing.Color.Empty;
            this.searchbutton.Image = null;
            this.searchbutton.ImageAutoCenter = true;
            this.searchbutton.ImageExpand = new System.Drawing.Point(0, 0);
            this.searchbutton.ImageOffset = new System.Drawing.Point(0, 0);
            this.searchbutton.ImageTint = System.Drawing.Color.Transparent;
            this.searchbutton.IsToggleButton = false;
            this.searchbutton.IsToggled = false;
            this.searchbutton.Location = new System.Drawing.Point(647, 12);
            this.searchbutton.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.searchbutton.Name = "searchbutton";
            this.searchbutton.NormalBackground = System.Drawing.Color.DarkBlue;
            this.searchbutton.NormalForeColor = System.Drawing.Color.White;
            this.searchbutton.NormalOutline = System.Drawing.Color.Empty;
            this.searchbutton.OutlineThickness = 2F;
            this.searchbutton.PressedBackground = System.Drawing.Color.RoyalBlue;
            this.searchbutton.PressedForeColor = System.Drawing.Color.White;
            this.searchbutton.PressedImageTint = System.Drawing.Color.White;
            this.searchbutton.PressedOutline = System.Drawing.Color.Empty;
            this.searchbutton.Rounding = new System.Windows.Forms.Padding(5);
            this.searchbutton.Size = new System.Drawing.Size(91, 39);
            this.searchbutton.TabIndex = 14;
            this.searchbutton.TextAutoCenter = true;
            this.searchbutton.TextOffset = new System.Drawing.Point(0, 0);
            this.searchbutton.Click += new System.EventHandler(this.searchbutton_Click_1);
            // 
            // TripIDbox
            // 
            this.TripIDbox.BorderColor = System.Drawing.Color.Transparent;
            this.TripIDbox.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(68)))), ((int)(((byte)(142)))));
            this.TripIDbox.BorderRadius = 5;
            this.TripIDbox.BorderSize = 3;
            this.TripIDbox.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TripIDbox.Icon = null;
            this.TripIDbox.IconSize = new System.Drawing.Size(20, 20);
            this.TripIDbox.Location = new System.Drawing.Point(10, 12);
            this.TripIDbox.Margin = new System.Windows.Forms.Padding(2);
            this.TripIDbox.Multiline = false;
            this.TripIDbox.Name = "TripIDbox";
            this.TripIDbox.PasswordChar = false;
            this.TripIDbox.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.TripIDbox.PlaceholderText = "Trip ID";
            this.TripIDbox.Size = new System.Drawing.Size(267, 43);
            this.TripIDbox.TabIndex = 1;
            this.TripIDbox.Text = "searchbox";
            this.TripIDbox.Texts = "";
            this.TripIDbox.UnderlinedStyle = false;
            // 
            // BookButton
            // 
            this.BookButton.BackColor = System.Drawing.Color.DarkGray;
            this.BookButton.ButtonText = "BOOK";
            this.BookButton.CheckedBackground = System.Drawing.Color.DodgerBlue;
            this.BookButton.CheckedForeColor = System.Drawing.Color.Transparent;
            this.BookButton.CheckedImageTint = System.Drawing.Color.White;
            this.BookButton.CheckedOutline = System.Drawing.Color.Transparent;
            this.BookButton.CustomDialogResult = System.Windows.Forms.DialogResult.None;
            this.BookButton.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BookButton.HoverBackground = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(255)))), ((int)(((byte)(106)))), ((int)(((byte)(0)))));
            this.BookButton.HoverForeColor = System.Drawing.Color.White;
            this.BookButton.HoverImage = null;
            this.BookButton.HoverImageTint = System.Drawing.Color.White;
            this.BookButton.HoverOutline = System.Drawing.Color.Empty;
            this.BookButton.Image = null;
            this.BookButton.ImageAutoCenter = true;
            this.BookButton.ImageExpand = new System.Drawing.Point(0, 0);
            this.BookButton.ImageOffset = new System.Drawing.Point(0, 0);
            this.BookButton.ImageTint = System.Drawing.Color.Transparent;
            this.BookButton.IsToggleButton = false;
            this.BookButton.IsToggled = false;
            this.BookButton.Location = new System.Drawing.Point(277, 12);
            this.BookButton.Margin = new System.Windows.Forms.Padding(4);
            this.BookButton.Name = "BookButton";
            this.BookButton.NormalBackground = System.Drawing.Color.ForestGreen;
            this.BookButton.NormalForeColor = System.Drawing.Color.White;
            this.BookButton.NormalOutline = System.Drawing.Color.Empty;
            this.BookButton.OutlineThickness = 2F;
            this.BookButton.PressedBackground = System.Drawing.Color.RoyalBlue;
            this.BookButton.PressedForeColor = System.Drawing.Color.White;
            this.BookButton.PressedImageTint = System.Drawing.Color.White;
            this.BookButton.PressedOutline = System.Drawing.Color.Empty;
            this.BookButton.Rounding = new System.Windows.Forms.Padding(5);
            this.BookButton.Size = new System.Drawing.Size(91, 39);
            this.BookButton.TabIndex = 14;
            this.BookButton.TextAutoCenter = true;
            this.BookButton.TextOffset = new System.Drawing.Point(0, 0);
            // 
            // SearchBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.BookButton);
            this.Controls.Add(this.TripIDbox);
            this.Controls.Add(this.searchbutton);
            this.Controls.Add(this.sataTextBox1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "SearchBar";
            this.Size = new System.Drawing.Size(752, 63);
            this.ResumeLayout(false);

        }

        #endregion

        private SATATextBox sataTextBox1;
        private FrameworkTest.SATAButton searchbutton;
        private SATATextBox TripIDbox;
        private FrameworkTest.SATAButton BookButton;
    }
}
