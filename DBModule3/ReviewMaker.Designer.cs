namespace DBModule3
{
    partial class ReviewMaker
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
            SATAUiFramework.BorderRadius borderRadius1 = new SATAUiFramework.BorderRadius();
            this.label3 = new System.Windows.Forms.Label();
            this.sataPanel1 = new SATAUiFramework.SATAPanel();
            this.categoryType = new System.Windows.Forms.ComboBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.destinationtext = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label = new System.Windows.Forms.Label();
            this.reviewbutton = new FrameworkTest.SATAButton();
            this.sataPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Century Gothic", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(39, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(234, 34);
            this.label3.TabIndex = 15;
            this.label3.Text = "LEAVE A REVIEW";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // sataPanel1
            // 
            this.sataPanel1.BackColor2 = System.Drawing.Color.DarkOrange;
            this.sataPanel1.BorderColor = System.Drawing.Color.Black;
            borderRadius1.BottomLeft = 10;
            borderRadius1.BottomRight = 10;
            borderRadius1.TopLeft = 10;
            borderRadius1.TopRight = 10;
            this.sataPanel1.BorderRadius = borderRadius1;
            this.sataPanel1.BorderThickness = 0;
            this.sataPanel1.Controls.Add(this.label3);
            this.sataPanel1.Location = new System.Drawing.Point(-8, 30);
            this.sataPanel1.Name = "sataPanel1";
            this.sataPanel1.Size = new System.Drawing.Size(1000, 70);
            this.sataPanel1.TabIndex = 16;
            // 
            // categoryType
            // 
            this.categoryType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.categoryType.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.categoryType.Location = new System.Drawing.Point(178, 254);
            this.categoryType.Name = "categoryType";
            this.categoryType.Size = new System.Drawing.Size(78, 31);
            this.categoryType.TabIndex = 54;
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.textBox2.Location = new System.Drawing.Point(178, 198);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(255, 35);
            this.textBox2.TabIndex = 53;
            // 
            // destinationtext
            // 
            this.destinationtext.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.destinationtext.Location = new System.Drawing.Point(178, 151);
            this.destinationtext.Multiline = true;
            this.destinationtext.Name = "destinationtext";
            this.destinationtext.Size = new System.Drawing.Size(255, 35);
            this.destinationtext.TabIndex = 52;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Century Gothic", 12F);
            this.textBox1.Location = new System.Drawing.Point(178, 295);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(255, 130);
            this.textBox1.TabIndex = 51;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(50, 295);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(114, 23);
            this.label4.TabIndex = 50;
            this.label4.Text = "Comment:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(50, 254);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 23);
            this.label2.TabIndex = 49;
            this.label2.Text = "Rating:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(47, 198);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 23);
            this.label1.TabIndex = 48;
            this.label1.Text = "Provider ID:";
            // 
            // label
            // 
            this.label.AutoSize = true;
            this.label.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label.Location = new System.Drawing.Point(47, 151);
            this.label.Name = "label";
            this.label.Size = new System.Drawing.Size(72, 23);
            this.label.TabIndex = 47;
            this.label.Text = "Trip ID:";
            // 
            // reviewbutton
            // 
            this.reviewbutton.BackColor = System.Drawing.Color.DarkGray;
            this.reviewbutton.ButtonText = "MAKE REVIEW";
            this.reviewbutton.CheckedBackground = System.Drawing.Color.DodgerBlue;
            this.reviewbutton.CheckedForeColor = System.Drawing.Color.Transparent;
            this.reviewbutton.CheckedImageTint = System.Drawing.Color.White;
            this.reviewbutton.CheckedOutline = System.Drawing.Color.Transparent;
            this.reviewbutton.CustomDialogResult = System.Windows.Forms.DialogResult.None;
            this.reviewbutton.Font = new System.Drawing.Font("Century Gothic", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.reviewbutton.HoverBackground = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(255)))), ((int)(((byte)(106)))), ((int)(((byte)(0)))));
            this.reviewbutton.HoverForeColor = System.Drawing.Color.White;
            this.reviewbutton.HoverImage = null;
            this.reviewbutton.HoverImageTint = System.Drawing.Color.White;
            this.reviewbutton.HoverOutline = System.Drawing.Color.Empty;
            this.reviewbutton.Image = null;
            this.reviewbutton.ImageAutoCenter = true;
            this.reviewbutton.ImageExpand = new System.Drawing.Point(0, 0);
            this.reviewbutton.ImageOffset = new System.Drawing.Point(0, 0);
            this.reviewbutton.ImageTint = System.Drawing.Color.Transparent;
            this.reviewbutton.IsToggleButton = false;
            this.reviewbutton.IsToggled = false;
            this.reviewbutton.Location = new System.Drawing.Point(533, 377);
            this.reviewbutton.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.reviewbutton.Name = "reviewbutton";
            this.reviewbutton.NormalBackground = System.Drawing.Color.DarkBlue;
            this.reviewbutton.NormalForeColor = System.Drawing.Color.White;
            this.reviewbutton.NormalOutline = System.Drawing.Color.Empty;
            this.reviewbutton.OutlineThickness = 2F;
            this.reviewbutton.PressedBackground = System.Drawing.Color.RoyalBlue;
            this.reviewbutton.PressedForeColor = System.Drawing.Color.White;
            this.reviewbutton.PressedImageTint = System.Drawing.Color.White;
            this.reviewbutton.PressedOutline = System.Drawing.Color.Empty;
            this.reviewbutton.Rounding = new System.Windows.Forms.Padding(5);
            this.reviewbutton.Size = new System.Drawing.Size(177, 48);
            this.reviewbutton.TabIndex = 55;
            this.reviewbutton.TextAutoCenter = true;
            this.reviewbutton.TextOffset = new System.Drawing.Point(0, 0);
            this.reviewbutton.Click += new System.EventHandler(this.reviewbutton_Click);
            // 
            // ReviewMaker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.reviewbutton);
            this.Controls.Add(this.categoryType);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.destinationtext);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label);
            this.Controls.Add(this.sataPanel1);
            this.Name = "ReviewMaker";
            this.Size = new System.Drawing.Size(801, 458);
            this.Load += new System.EventHandler(this.ReviewMaker_Load);
            this.sataPanel1.ResumeLayout(false);
            this.sataPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private SATAUiFramework.SATAPanel sataPanel1;
        private System.Windows.Forms.ComboBox categoryType;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox destinationtext;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label;
        private FrameworkTest.SATAButton reviewbutton;
    }
}
