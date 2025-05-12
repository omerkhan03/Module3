using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace DBModule3
{
    public class HotelReport : UserControl
    {
        private int userId;
        private DataGridView occupancyGrid;
        private Label titleLabel;        
        public HotelReport(int userId)
        {
            this.userId = userId;
            InitializeComponent();
            this.Dock = DockStyle.Fill;
            LoadReports();
        }
        private void InitializeComponent()
        {
            this.occupancyGrid = new System.Windows.Forms.DataGridView();
            this.titleLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.occupancyGrid)).BeginInit();
            this.SuspendLayout();
            
            // titleLabel
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, FontStyle.Bold);
            this.titleLabel.Location = new System.Drawing.Point(20, 20);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(200, 25);
            this.titleLabel.Text = "Hotel Performance Report";
            
            // occupancyGrid
            this.occupancyGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.occupancyGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.occupancyGrid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.occupancyGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.occupancyGrid.Location = new System.Drawing.Point(20, 60);
            this.occupancyGrid.Name = "occupancyGrid";
            this.occupancyGrid.Size = new System.Drawing.Size(760, 380);
            this.occupancyGrid.TabIndex = 0;
            this.occupancyGrid.ReadOnly = true;
            this.occupancyGrid.AllowUserToAddRows = false;
            this.occupancyGrid.RowHeadersVisible = false;
            this.occupancyGrid.BorderStyle = BorderStyle.None;
            this.occupancyGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.occupancyGrid.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.occupancyGrid.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkTurquoise;
            this.occupancyGrid.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            
            // HotelReport
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.occupancyGrid);
            this.Controls.Add(this.titleLabel);
            this.Name = "HotelReport";
            this.Size = new System.Drawing.Size(800, 460);
            ((System.ComponentModel.ISupportInitialize)(this.occupancyGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadReports()
        {
            LoadOccupancyRate();
        }

        private void LoadOccupancyRate()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.CreateConnection())
                {
                    conn.Open();                    string query = @"
                        WITH ServiceStats AS (
                            SELECT 
                                h.hotel_id,
                                h.hotel_name,
                                h.location,
                                h.total_rooms,
                                COUNT(DISTINCT sa.assignment_id) as total_assignments,
                                COUNT(DISTINCT sa.booking_id) as unique_bookings,
                                COUNT(CASE WHEN sa.status = 'Completed' THEN 1 END) as completed_services
                            FROM Hotel h
                            JOIN ServiceProvider sp ON h.provider_id = sp.provider_id
                            LEFT JOIN ServiceAssignment sa ON sp.provider_id = sa.provider_id
                            WHERE sp.user_id = @UserId
                            GROUP BY h.hotel_id, h.hotel_name, h.location, h.total_rooms
                        )
                        SELECT 
                            hotel_name as 'Hotel Name',
                            location as 'Location',
                            total_rooms as 'Total Rooms',
                            total_assignments as 'Total Bookings',
                            unique_bookings as 'Unique Bookings',
                            CAST(CAST(unique_bookings AS FLOAT) / NULLIF(CAST(total_rooms AS FLOAT), 0) * 100 AS DECIMAL(5,2)) as 'Occupancy Rate',
                            CAST(CAST(completed_services AS FLOAT) / NULLIF(CAST(total_assignments AS FLOAT), 0) * 100 AS DECIMAL(5,2)) as 'Service Completion'
                        FROM ServiceStats
                        ORDER BY 'Occupancy Rate' DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            occupancyGrid.DataSource = dt;                            // Format percentage columns
                            if (occupancyGrid.Columns["Occupancy Rate"] != null)
                            {
                                occupancyGrid.Columns["Occupancy Rate"].DefaultCellStyle.Format = "N2";
                                occupancyGrid.Columns["Occupancy Rate"].Width = 120;
                            }
                            if (occupancyGrid.Columns["Service Completion"] != null)
                            {
                                occupancyGrid.Columns["Service Completion"].DefaultCellStyle.Format = "N2";
                                occupancyGrid.Columns["Service Completion"].Width = 120;
                            }

                            // Set other column widths
                            if (occupancyGrid.Columns["Hotel Name"] != null)
                                occupancyGrid.Columns["Hotel Name"].Width = 150;
                            if (occupancyGrid.Columns["Location"] != null)
                                occupancyGrid.Columns["Location"].Width = 150;
                            if (occupancyGrid.Columns["Total Rooms"] != null)
                                occupancyGrid.Columns["Total Rooms"].Width = 100;
                            if (occupancyGrid.Columns["Total Bookings"] != null)
                                occupancyGrid.Columns["Total Bookings"].Width = 100;
                            if (occupancyGrid.Columns["Unique Bookings"] != null)
                                occupancyGrid.Columns["Unique Bookings"].Width = 100;

                            // Style alternating rows
                            occupancyGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
                            occupancyGrid.EnableHeadersVisualStyles = false;
                            occupancyGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
                            occupancyGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                            occupancyGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Century Gothic", 10, FontStyle.Bold);
                            occupancyGrid.ColumnHeadersHeight = 40;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               // MessageBox.Show($"Error loading occupancy rate: {ex.Message}");
            }
        }
    }
}
