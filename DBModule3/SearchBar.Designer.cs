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
            this.sataTextBox1.Location = new System.Drawing.Point(14, 15);
            this.sataTextBox1.Multiline = false;
            this.sataTextBox1.Name = "sataTextBox1";
            this.sataTextBox1.PasswordChar = false;
            this.sataTextBox1.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.sataTextBox1.PlaceholderText = "Search";
            this.sataTextBox1.Size = new System.Drawing.Size(819, 45);
            this.sataTextBox1.TabIndex = 1;
            this.sataTextBox1.Text = "searchbox";
            this.sataTextBox1.Texts = "";
            this.sataTextBox1.UnderlinedStyle = false;
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
            this.searchbutton.Location = new System.Drawing.Point(842, 15);
            this.searchbutton.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
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
            this.searchbutton.Size = new System.Drawing.Size(121, 48);
            this.searchbutton.TabIndex = 14;
            this.searchbutton.TextAutoCenter = true;
            this.searchbutton.TextOffset = new System.Drawing.Point(0, 0);
            // 
            // SearchBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.searchbutton);
            this.Controls.Add(this.sataTextBox1);
            this.Name = "SearchBar";
            this.Size = new System.Drawing.Size(1003, 78);
            this.ResumeLayout(false);

        }

        #endregion

        private SATATextBox sataTextBox1;
        private FrameworkTest.SATAButton searchbutton;
    }
}
