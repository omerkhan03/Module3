namespace DBModule3
{
    partial class ServiceProviderAssigner
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
            this.pendingBookingsGrid = new System.Windows.Forms.DataGridView();
            this.providersGrid = new System.Windows.Forms.DataGridView();
            this.assignButton = new FrameworkTest.SATAButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pendingBookingsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.providersGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // pendingBookingsGrid
            // 
            this.pendingBookingsGrid.AllowUserToAddRows = false;
            this.pendingBookingsGrid.AllowUserToDeleteRows = false;
            this.pendingBookingsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.pendingBookingsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.pendingBookingsGrid.Location = new System.Drawing.Point(20, 40);
            this.pendingBookingsGrid.Name = "pendingBookingsGrid";
            this.pendingBookingsGrid.ReadOnly = true;
            this.pendingBookingsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.pendingBookingsGrid.Size = new System.Drawing.Size(735, 116);
            this.pendingBookingsGrid.TabIndex = 0;
            this.pendingBookingsGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.pendingBookingsGrid_CellContentClick);
            // 
            // providersGrid
            // 
            this.providersGrid.AllowUserToAddRows = false;
            this.providersGrid.AllowUserToDeleteRows = false;
            this.providersGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.providersGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.providersGrid.Location = new System.Drawing.Point(20, 183);
            this.providersGrid.Name = "providersGrid";
            this.providersGrid.ReadOnly = true;
            this.providersGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.providersGrid.Size = new System.Drawing.Size(735, 117);
            this.providersGrid.TabIndex = 1;
            // 
            // assignButton
            // 
            this.assignButton.BackColor = System.Drawing.Color.DarkGray;
            this.assignButton.ButtonText = "ASSIGN PROVIDER";
            this.assignButton.CheckedBackground = System.Drawing.Color.DodgerBlue;
            this.assignButton.CheckedForeColor = System.Drawing.Color.White;
            this.assignButton.CheckedImageTint = System.Drawing.Color.White;
            this.assignButton.CheckedOutline = System.Drawing.Color.DodgerBlue;
            this.assignButton.CustomDialogResult = System.Windows.Forms.DialogResult.None;
            this.assignButton.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.assignButton.HoverBackground = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(255)))), ((int)(((byte)(106)))), ((int)(((byte)(0)))));
            this.assignButton.HoverForeColor = System.Drawing.Color.White;
            this.assignButton.HoverImage = null;
            this.assignButton.HoverImageTint = System.Drawing.Color.White;
            this.assignButton.HoverOutline = System.Drawing.Color.Empty;
            this.assignButton.Image = null;
            this.assignButton.ImageAutoCenter = true;
            this.assignButton.ImageExpand = new System.Drawing.Point(0, 0);
            this.assignButton.ImageOffset = new System.Drawing.Point(0, 0);
            this.assignButton.ImageTint = System.Drawing.Color.White;
            this.assignButton.IsToggleButton = false;
            this.assignButton.IsToggled = false;
            this.assignButton.Location = new System.Drawing.Point(293, 307);
            this.assignButton.Margin = new System.Windows.Forms.Padding(6, 4, 6, 4);
            this.assignButton.Name = "assignButton";
            this.assignButton.NormalBackground = System.Drawing.Color.DodgerBlue;
            this.assignButton.NormalForeColor = System.Drawing.Color.White;
            this.assignButton.NormalOutline = System.Drawing.Color.Empty;
            this.assignButton.OutlineThickness = 2F;
            this.assignButton.PressedBackground = System.Drawing.Color.RoyalBlue;
            this.assignButton.PressedForeColor = System.Drawing.Color.White;
            this.assignButton.PressedImageTint = System.Drawing.Color.White;
            this.assignButton.PressedOutline = System.Drawing.Color.Empty;
            this.assignButton.Rounding = new System.Windows.Forms.Padding(5);
            this.assignButton.Size = new System.Drawing.Size(200, 40);
            this.assignButton.TabIndex = 2;
            this.assignButton.TextAutoCenter = true;
            this.assignButton.TextOffset = new System.Drawing.Point(0, 0);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(16, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(145, 21);
            this.label1.TabIndex = 3;
            this.label1.Text = "Pending Bookings";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Century Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(16, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(139, 21);
            this.label2.TabIndex = 4;
            this.label2.Text = "Service Providers";
            // 
            // ServiceProviderAssigner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pendingBookingsGrid);
            this.Controls.Add(this.providersGrid);
            this.Controls.Add(this.assignButton);
            this.Name = "ServiceProviderAssigner";
            this.Size = new System.Drawing.Size(764, 356);
            ((System.ComponentModel.ISupportInitialize)(this.pendingBookingsGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.providersGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView pendingBookingsGrid;
        private System.Windows.Forms.DataGridView providersGrid;
        private FrameworkTest.SATAButton assignButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
