using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace DBModule3
{
    public class TransportReport : UserControl
    {
        private int userId;
        private DataGridView performanceGrid;
        private Label titleLabel;

        public TransportReport(int userId)
        {
            this.userId = userId;
            InitializeComponent();
            LoadReports();
        }

        private void InitializeComponent()
        {
            this.performanceGrid = new System.Windows.Forms.DataGridView();
            this.titleLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.performanceGrid)).BeginInit();
            this.SuspendLayout();
            
            // titleLabel
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.titleLabel.Location = new System.Drawing.Point(20, 20);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(200, 25);
            this.titleLabel.Text = "Transport Performance";
            
            // performanceGrid
            this.performanceGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.performanceGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.performanceGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.performanceGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.performanceGrid.Location = new System.Drawing.Point(20, 60);
            this.performanceGrid.Name = "performanceGrid";
            this.performanceGrid.Size = new System.Drawing.Size(760, 380);
            this.performanceGrid.TabIndex = 0;
            
            // TransportReport
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.performanceGrid);
            this.Controls.Add(this.titleLabel);
            this.Name = "TransportReport";
            this.Size = new System.Drawing.Size(800, 460);
            ((System.ComponentModel.ISupportInitialize)(this.performanceGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadReports()
        {
            LoadOnTimePerformance();
        }

        private void LoadOnTimePerformance()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.CreateConnection())
                {
                    conn.Open();                    string query = @"
                        WITH TransportStats AS (
                            SELECT 
                                ts.transport_id,
                                ts.transport_type,
                                ts.vehicle_details,
                                ts.capacity,
                                COUNT(sa.assignment_id) as total_assignments,
                                COUNT(DISTINCT sa.booking_id) as unique_bookings,
                                CAST(COUNT(CASE WHEN sa.status = 'Completed' THEN 1 END) * 100.0 / NULLIF(COUNT(sa.assignment_id), 0) AS DECIMAL(5,2)) as completion_rate
                            FROM TransportService ts 
                            JOIN ServiceProvider sp ON ts.provider_id = sp.provider_id
                            LEFT JOIN ServiceAssignment sa ON sp.provider_id = sa.provider_id
                            WHERE sp.user_id = @UserId
                            GROUP BY ts.transport_id, ts.transport_type, ts.vehicle_details, ts.capacity
                        )
                        SELECT 
                            transport_type as vehicle_type,
                            vehicle_details,
                            capacity as max_capacity,
                            total_assignments as total_trips,
                            unique_bookings,
                            completion_rate as service_completion_rate,
                            CAST(CAST(unique_bookings AS DECIMAL(10,2)) / CAST(NULLIF(total_assignments, 0) AS DECIMAL(10,2)) * 100.0 AS DECIMAL(5,2)) as booking_efficiency
                        FROM TransportStats
                        ORDER BY total_assignments DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            performanceGrid.DataSource = dt;                            // Format percentage columns
                            if (performanceGrid.Columns["service_completion_rate"] != null)
                                performanceGrid.Columns["service_completion_rate"].DefaultCellStyle.Format = "N2";
                            if (performanceGrid.Columns["booking_efficiency"] != null)
                                performanceGrid.Columns["booking_efficiency"].DefaultCellStyle.Format = "N2";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading on-time performance: {ex.Message}");
            }
        }
    }
}
