using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace DBModule3
{
    public partial class BookingRevenueReport : UserControl
    {
        private DataGridView mainGridView;
        private ComboBox reportTypeComboBox;
        private Button generateButton;
        private Label titleLabel;

        public BookingRevenueReport()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Initialize components
            titleLabel = new Label
            {
                Text = "Booking and Revenue Report",
                Font = new Font("Century Gothic", 16, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            reportTypeComboBox = new ComboBox
            {
                Location = new Point(20, 60),
                Width = 300,
                Font = new Font("Century Gothic", 10)
            };

            reportTypeComboBox.Items.AddRange(new string[] {
                "Total Bookings by Tour Operator",
                "Revenue by Trip Type",
                "Cancellation Rate",
                "Peak Booking Periods",
                "Average Booking Value"
            });

            generateButton = new Button
            {
                Text = "Generate Report",
                Location = new Point(340, 60),
                Font = new Font("Century Gothic", 10),
                Width = 150
            };
            generateButton.Click += GenerateButton_Click;

            mainGridView = new DataGridView
            {
                Location = new Point(20, 100),
                Width = 900,
                Height = 400,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                Font = new Font("Century Gothic", 10),
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    SelectionBackColor = Color.DarkTurquoise,
                    SelectionForeColor = Color.WhiteSmoke
                }
            };

            // Add controls
            Controls.Add(titleLabel);
            Controls.Add(reportTypeComboBox);
            Controls.Add(generateButton);
            Controls.Add(mainGridView);
        }

        private void GenerateButton_Click(object sender, EventArgs e)
        {
            if (reportTypeComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a report type.", "Warning", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                switch (reportTypeComboBox.SelectedItem.ToString())
                {
                    case "Total Bookings by Tour Operator":
                        LoadTotalBookings();
                        break;
                    case "Revenue by Trip Type":
                        LoadRevenueByTripType();
                        break;
                    case "Cancellation Rate":
                        LoadCancellationRate();
                        break;
                    case "Peak Booking Periods":
                        LoadPeakBookingPeriods();
                        break;
                    case "Average Booking Value":
                        LoadAverageBookingValue();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadTotalBookings()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"
                    Select tou.operator_id, COUNT(tou.operator_id) as numTrips
                    FROM Booking b 
                    JOIN Trip t ON b.trip_id = t.trip_id
                    JOIN TourOperator tou ON tou.operator_id = t.operator_id
                    GROUP by tou.operator_id";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();
                }
            }
        }

        private void LoadRevenueByTripType()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"
                    SELECT tt.category_name, SUM(t.price) as TotalRevenue
                    FROM TripCategory tt
                    JOIN Trip t ON t.category_id = tt.category_id
                    GROUP BY tt.category_name";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();

                    // Format revenue column
                    if (mainGridView.Columns["TotalRevenue"] != null)
                    {
                        mainGridView.Columns["TotalRevenue"].DefaultCellStyle.Format = "C2";
                    }
                }
            }
        }

        private void LoadCancellationRate()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"
                    SELECT 
                        COUNT(CASE WHEN is_cancelled = 1 THEN 1 END) * 100.0 / NULLIF(COUNT(*), 0) as cancellation_rate_percentage,
                        COUNT(*) as total_bookings,
                        COUNT(CASE WHEN is_cancelled = 1 THEN 1 END) as cancelled_bookings
                    FROM Booking";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();

                    // Format percentage column
                    if (mainGridView.Columns["cancellation_rate_percentage"] != null)
                    {
                        mainGridView.Columns["cancellation_rate_percentage"].DefaultCellStyle.Format = "N2";
                    }
                }
            }
        }

        private void LoadPeakBookingPeriods()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"
                    SELECT 
                        MONTH(booking_date) as booking_month,
                        YEAR(booking_date) as booking_year,
                        COUNT(*) as total_bookings,
                        SUM(total_amount) as total_revenue
                    FROM Booking
                    WHERE status = 'Confirmed'
                    GROUP BY YEAR(booking_date), MONTH(booking_date)
                    ORDER BY total_bookings DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();

                    // Format revenue column
                    if (mainGridView.Columns["total_revenue"] != null)
                    {
                        mainGridView.Columns["total_revenue"].DefaultCellStyle.Format = "C2";
                    }
                }
            }
        }

        private void LoadAverageBookingValue()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"
                    SELECT 
                        AVG(total_amount) as avg_booking_value,
                        MIN(total_amount) as min_booking_value,
                        MAX(total_amount) as max_booking_value,
                        COUNT(*) as total_bookings,
                        SUM(total_amount) as total_revenue
                    FROM Booking
                    WHERE status = 'Confirmed' 
                    AND is_cancelled = 0";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();

                    // Format monetary columns
                    string[] moneyColumns = { "avg_booking_value", "min_booking_value", "max_booking_value", "total_revenue" };
                    foreach (string columnName in moneyColumns)
                    {
                        if (mainGridView.Columns[columnName] != null)
                        {
                            mainGridView.Columns[columnName].DefaultCellStyle.Format = "C2";
                        }
                    }
                }
            }
        }

        private void FormatGridView()
        {
            mainGridView.EnableHeadersVisualStyles = false;
            mainGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            mainGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            mainGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
        }
    }
}