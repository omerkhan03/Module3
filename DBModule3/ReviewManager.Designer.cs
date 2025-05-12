using System.Windows.Forms;

namespace DBModule3
{
    partial class ReviewManager
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
            this.topPanel = new SATAUiFramework.SATAPanel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.reviewsGrid = new System.Windows.Forms.DataGridView();
            
            // Top panel with title
            SATAUiFramework.BorderRadius borderRadius = new SATAUiFramework.BorderRadius();
            borderRadius.BottomLeft = 10;
            borderRadius.BottomRight = 10;
            borderRadius.TopLeft = 10;
            borderRadius.TopRight = 10;

            this.topPanel.BackColor2 = System.Drawing.Color.DarkOrange;
            this.topPanel.BorderColor = System.Drawing.Color.Black;
            this.topPanel.BorderRadius = borderRadius;
            this.topPanel.BorderThickness = 0;
            this.topPanel.Controls.Add(this.titleLabel);
            this.topPanel.Location = new System.Drawing.Point(-8, 30);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(1000, 70);

            // Title label
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.Transparent;
            this.titleLabel.Font = new System.Drawing.Font("Century Gothic", 16.2F, System.Drawing.FontStyle.Bold);
            this.titleLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.titleLabel.Location = new System.Drawing.Point(39, 19);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(250, 34);
            this.titleLabel.Text = "MANAGE REVIEWS";

            // Reviews grid
            this.reviewsGrid.AllowUserToAddRows = false;
            this.reviewsGrid.AllowUserToDeleteRows = false;
            this.reviewsGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.reviewsGrid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.reviewsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.reviewsGrid.Location = new System.Drawing.Point(20, 120);
            this.reviewsGrid.Name = "reviewsGrid";
            this.reviewsGrid.ReadOnly = true;
            this.reviewsGrid.RowHeadersVisible = false;            this.reviewsGrid.RowTemplate.Height = 24;
            this.reviewsGrid.Size = new System.Drawing.Size(950, 500);
            this.reviewsGrid.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.reviewsGrid_CellContentClick);
              
            // Add Flag/Unflag button column
            System.Windows.Forms.DataGridViewButtonColumn flagButton = new System.Windows.Forms.DataGridViewButtonColumn();
            flagButton.Name = "FlagButton";
            flagButton.HeaderText = "Flag/Unflag";
            flagButton.Text = "Toggle Flag";
            flagButton.UseColumnTextForButtonValue = true;
            this.reviewsGrid.Columns.Add(flagButton);

            // Configure the ReviewManager control
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.reviewsGrid);
            this.Controls.Add(this.topPanel);
            this.Name = "ReviewManager";
            this.Size = new System.Drawing.Size(990, 650);
            
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reviewsGrid)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private SATAUiFramework.SATAPanel topPanel;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.DataGridView reviewsGrid;
    }
}
