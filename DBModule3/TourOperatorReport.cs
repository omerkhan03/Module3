using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace DBModule3
{
    public class TourOperatorReport : UserControl
    {
        private int userId;
        private DataGridView ratingGrid;
        private DataGridView revenueGrid;
        private DataGridView responseTimeGrid;
        private Label titleLabel;

        public TourOperatorReport(int userId)
        {
            this.userId = userId;
            InitializeComponent();
            LoadReports();
        }

        private void InitializeComponent()
        {
            this.ratingGrid = new DataGridView();
            this.revenueGrid = new DataGridView();
            this.responseTimeGrid = new DataGridView();
            this.titleLabel = new Label();

            ((System.ComponentModel.ISupportInitialize)(this.ratingGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.revenueGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.responseTimeGrid)).BeginInit();
            this.SuspendLayout();

            // Set up the base control first
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Dock = DockStyle.Fill;
            this.Padding = new Padding(10);
            this.MinimumSize = new Size(800, 600);
            
            // titleLabel
            this.titleLabel.Text = "Tour Operator Performance Report";
            this.titleLabel.Font = new Font("Century Gothic", 14, FontStyle.Bold);
            this.titleLabel.Dock = DockStyle.Top;
            this.titleLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.titleLabel.Height = 40;
            this.titleLabel.Padding = new Padding(0, 10, 0, 0);
            this.titleLabel.BackColor = Color.Transparent;

            // Common grid settings
            DataGridView[] grids = { ratingGrid, revenueGrid, responseTimeGrid };
            foreach (var grid in grids)
            {
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                grid.BackgroundColor = Color.White;
                grid.BorderStyle = BorderStyle.None;
                grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                grid.ReadOnly = true;
                grid.RowHeadersVisible = false;
                grid.Font = new Font("Century Gothic", 10);
                grid.AllowUserToAddRows = false;
                grid.AllowUserToDeleteRows = false;
                grid.AllowUserToOrderColumns = true;
                grid.MultiSelect = false;
                grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                grid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
                grid.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                grid.DefaultCellStyle = new DataGridViewCellStyle
                {
                    SelectionBackColor = Color.DarkTurquoise,
                    SelectionForeColor = Color.WhiteSmoke,
                    Padding = new Padding(5)
                };
            }

            // Individual grid positioning
            this.ratingGrid.Location = new Point(10, 50);
            this.ratingGrid.Size = new Size(ClientSize.Width - 20, 150);
            this.ratingGrid.Name = "ratingGrid";

            this.revenueGrid.Location = new Point(10, 220);
            this.revenueGrid.Size = new Size(ClientSize.Width - 20, 150);
            this.revenueGrid.Name = "revenueGrid";

            this.responseTimeGrid.Location = new Point(10, 390);
            this.responseTimeGrid.Size = new Size(ClientSize.Width - 20, 150);
            this.responseTimeGrid.Name = "responseTimeGrid";

            // Add controls in correct order
            this.Controls.Add(this.responseTimeGrid);
            this.Controls.Add(this.revenueGrid);
            this.Controls.Add(this.ratingGrid);
            this.Controls.Add(this.titleLabel);
            
            // Finish initialization
            ((System.ComponentModel.ISupportInitialize)(this.ratingGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.revenueGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.responseTimeGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void LoadReports()
        {
            LoadAverageRating();
            LoadRevenue();
            LoadResponseTime();
        }

        private void LoadAverageRating()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.CreateConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            t.company_name as [Company Name],
                            COUNT(r.review_id) as [Total Reviews],
                            FORMAT(AVG(CAST(r.rating AS DECIMAL(3,2))), 'N2') as [Average Rating]
                        FROM TourOperator t
                        JOIN Trip tr ON t.operator_id = tr.operator_id
                        LEFT JOIN Review r ON tr.trip_id = r.trip_id
                        JOIN [User] u ON t.user_id = u.user_id
                        WHERE u.user_id = @UserId
                        GROUP BY t.operator_id, t.company_name";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            ratingGrid.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading ratings: {ex.Message}");
            }
        }

        private void LoadRevenue()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.CreateConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            t.company_name as [Company Name],
                            COUNT(b.booking_id) as [Total Bookings],
                            FORMAT(SUM(b.total_amount), 'C2') as [Total Revenue],
                            FORMAT(AVG(b.total_amount), 'C2') as [Average Booking Value]
                        FROM TourOperator t
                        JOIN Trip tr ON t.operator_id = tr.operator_id
                        JOIN Booking b ON tr.trip_id = b.trip_id
                        JOIN [User] u ON t.user_id = u.user_id
                        WHERE b.status = 'Confirmed' 
                        AND b.is_cancelled = 0
                        AND u.user_id = @UserId
                        GROUP BY t.operator_id, t.company_name";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            revenueGrid.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading revenue: {ex.Message}");
            }
        }

        private void LoadResponseTime()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.CreateConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            t.company_name as [Company Name],
                            FORMAT(AVG(CAST(DATEDIFF(HOUR, b.booking_date, sa.assignment_date) AS DECIMAL(10,2))), 'N2') as [Average Response (Hours)]
                        FROM TourOperator t
                        JOIN Trip tr ON t.operator_id = tr.operator_id
                        JOIN Booking b ON tr.trip_id = b.trip_id
                        JOIN ServiceAssignment sa ON b.booking_id = sa.booking_id
                        JOIN [User] u ON t.user_id = u.user_id
                        WHERE u.user_id = @UserId
                        GROUP BY t.operator_id, t.company_name";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            responseTimeGrid.DataSource = dt;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading response time: {ex.Message}");
            }
        }
    }
}
