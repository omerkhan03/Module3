namespace DBModule3
{
    partial class CancelBooking
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
            this.bookingtext = new SATATextBox();
            this.cancelbutton = new FrameworkTest.SATAButton();
            this.SuspendLayout();
            // 
            // bookingtext
            // 
            this.bookingtext.BorderColor = System.Drawing.Color.Transparent;
            this.bookingtext.BorderFocusColor = System.Drawing.Color.FromArgb(((int)(((byte)(6)))), ((int)(((byte)(68)))), ((int)(((byte)(142)))));
            this.bookingtext.BorderRadius = 5;
            this.bookingtext.BorderSize = 3;
            this.bookingtext.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.bookingtext.Icon = null;
            this.bookingtext.IconSize = new System.Drawing.Size(20, 20);
            this.bookingtext.Location = new System.Drawing.Point(57, 16);
            this.bookingtext.Multiline = true;
            this.bookingtext.Name = "bookingtext";
            this.bookingtext.PasswordChar = false;
            this.bookingtext.PlaceholderColor = System.Drawing.Color.DarkGray;
            this.bookingtext.PlaceholderText = "Booking ID";
            this.bookingtext.Size = new System.Drawing.Size(306, 48);
            this.bookingtext.TabIndex = 16;
            this.bookingtext.Text = "sataTextBox2";
            this.bookingtext.Texts = "";
            this.bookingtext.UnderlinedStyle = false;
            // 
            // cancelbutton
            // 
            this.cancelbutton.BackColor = System.Drawing.Color.DarkGray;
            this.cancelbutton.ButtonText = "CANCEL";
            this.cancelbutton.CheckedBackground = System.Drawing.Color.DodgerBlue;
            this.cancelbutton.CheckedForeColor = System.Drawing.Color.Transparent;
            this.cancelbutton.CheckedImageTint = System.Drawing.Color.White;
            this.cancelbutton.CheckedOutline = System.Drawing.Color.Transparent;
            this.cancelbutton.CustomDialogResult = System.Windows.Forms.DialogResult.None;
            this.cancelbutton.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cancelbutton.HoverBackground = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(255)))), ((int)(((byte)(106)))), ((int)(((byte)(0)))));
            this.cancelbutton.HoverForeColor = System.Drawing.Color.White;
            this.cancelbutton.HoverImage = null;
            this.cancelbutton.HoverImageTint = System.Drawing.Color.White;
            this.cancelbutton.HoverOutline = System.Drawing.Color.Empty;
            this.cancelbutton.Image = null;
            this.cancelbutton.ImageAutoCenter = true;
            this.cancelbutton.ImageExpand = new System.Drawing.Point(0, 0);
            this.cancelbutton.ImageOffset = new System.Drawing.Point(0, 0);
            this.cancelbutton.ImageTint = System.Drawing.Color.Transparent;
            this.cancelbutton.IsToggleButton = false;
            this.cancelbutton.IsToggled = false;
            this.cancelbutton.Location = new System.Drawing.Point(807, 16);
            this.cancelbutton.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.cancelbutton.Name = "cancelbutton";
            this.cancelbutton.NormalBackground = System.Drawing.Color.DarkBlue;
            this.cancelbutton.NormalForeColor = System.Drawing.Color.White;
            this.cancelbutton.NormalOutline = System.Drawing.Color.Empty;
            this.cancelbutton.OutlineThickness = 2F;
            this.cancelbutton.PressedBackground = System.Drawing.Color.RoyalBlue;
            this.cancelbutton.PressedForeColor = System.Drawing.Color.White;
            this.cancelbutton.PressedImageTint = System.Drawing.Color.White;
            this.cancelbutton.PressedOutline = System.Drawing.Color.Empty;
            this.cancelbutton.Rounding = new System.Windows.Forms.Padding(5);
            this.cancelbutton.Size = new System.Drawing.Size(121, 48);
            this.cancelbutton.TabIndex = 17;
            this.cancelbutton.TextAutoCenter = true;
            this.cancelbutton.TextOffset = new System.Drawing.Point(0, 0);
            // 
            // CancelBooking
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Controls.Add(this.cancelbutton);
            this.Controls.Add(this.bookingtext);
            this.Name = "CancelBooking";
            this.Size = new System.Drawing.Size(1003, 78);
            this.ResumeLayout(false);

        }

        #endregion

        private SATATextBox bookingtext;
        private FrameworkTest.SATAButton cancelbutton;
    }
}