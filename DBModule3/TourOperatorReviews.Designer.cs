using System.Windows.Forms;

namespace DBModule3
{
    partial class TourOperatorReviews
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
            this.sataPanel1 = new SATAUiFramework.SATAPanel();
            this.titleLabel = new System.Windows.Forms.Label();
            this.reviewsDataGridView = new System.Windows.Forms.DataGridView();
            this.sataPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reviewsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // sataPanel1
            // 
            this.sataPanel1.BackColor = System.Drawing.Color.Orange;
            this.sataPanel1.BackColor2 = System.Drawing.Color.DarkOrange;
            this.sataPanel1.BorderColor = System.Drawing.Color.Black;
            borderRadius1.BottomLeft = 10;
            borderRadius1.BottomRight = 10;
            borderRadius1.TopLeft = 10;
            borderRadius1.TopRight = 10;
            this.sataPanel1.BorderRadius = borderRadius1;
            this.sataPanel1.BorderThickness = 0;
            this.sataPanel1.Controls.Add(this.titleLabel);
            this.sataPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.sataPanel1.Location = new System.Drawing.Point(0, 0);
            this.sataPanel1.Name = "sataPanel1";
            this.sataPanel1.Size = new System.Drawing.Size(900, 70);
            this.sataPanel1.TabIndex = 16;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.BackColor = System.Drawing.Color.Transparent;
            this.titleLabel.Font = new System.Drawing.Font("Century Gothic", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.titleLabel.Location = new System.Drawing.Point(39, 19);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(158, 34);
            this.titleLabel.TabIndex = 15;
            this.titleLabel.Text = "MY REVIEWS";
            // 
            // reviewsDataGridView
            // 
            this.reviewsDataGridView.AllowUserToAddRows = false;
            this.reviewsDataGridView.AllowUserToDeleteRows = false;
            this.reviewsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.reviewsDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.reviewsDataGridView.Location = new System.Drawing.Point(20, 90);
            this.reviewsDataGridView.Name = "reviewsDataGridView";
            this.reviewsDataGridView.ReadOnly = true;
            this.reviewsDataGridView.RowHeadersWidth = 51;
            this.reviewsDataGridView.RowTemplate.Height = 24;
            this.reviewsDataGridView.Size = new System.Drawing.Size(860, 400);
            this.reviewsDataGridView.TabIndex = 17;
            // 
            // TourOperatorReviews
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.reviewsDataGridView);
            this.Controls.Add(this.sataPanel1);
            this.Name = "TourOperatorReviews";
            this.Size = new System.Drawing.Size(900, 510);
            this.sataPanel1.ResumeLayout(false);
            this.sataPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.reviewsDataGridView)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private SATAUiFramework.SATAPanel sataPanel1;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.DataGridView reviewsDataGridView;
    }
}
