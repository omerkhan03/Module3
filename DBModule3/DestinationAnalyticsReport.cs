using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace DBModule3
{
    public partial class DestinationAnalyticsReport : UserControl
    {
        private DataGridView mainGridView;
        private ComboBox reportTypeComboBox;
        private Button generateButton;
        private Label titleLabel;

        public DestinationAnalyticsReport()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Initialize components
            titleLabel = new Label
            {
                Text = "Destination Analytics Report",
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
                "Most-Booked Destinations",
                "Seasonal Trends",
                "Traveler Satisfaction Score",
                "Emerging Destinations"
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
                    case "Most-Booked Destinations":
                        LoadMostBookedDestinations();
                        break;
                    case "Seasonal Trends":
                        LoadSeasonalTrends();
                        break;
                    case "Traveler Satisfaction Score":
                        LoadTravelerSatisfaction();
                        break;
                    case "Emerging Destinations":
                        LoadEmergingDestinations();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMostBookedDestinations()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"
                    SELECT 
                        d.destination_id,
                        d.destination_name,
                        d.country,
                        d.region,
                        COUNT(DISTINCT b.booking_id) as total_bookings,
                        COUNT(DISTINCT b.traveler_id) as unique_travelers,
                        AVG(CAST(r.rating AS DECIMAL(3,2))) as average_rating,
                        SUM(b.total_amount) as total_revenue
                    FROM Destination d
                    JOIN Trip t ON d.destination_id = t.destination_id
                    LEFT JOIN Booking b ON t.trip_id = b.trip_id
                    LEFT JOIN Review r ON t.trip_id = r.trip_id
                    WHERE b.status = 'Confirmed' 
                    AND b.is_cancelled = 0
                    GROUP BY d.destination_id, d.destination_name, d.country, d.region
                    ORDER BY total_bookings DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();

                    // Format columns
                    if (mainGridView.Columns["total_revenue"] != null)
                        mainGridView.Columns["total_revenue"].DefaultCellStyle.Format = "C2";
                    if (mainGridView.Columns["average_rating"] != null)
                        mainGridView.Columns["average_rating"].DefaultCellStyle.Format = "N1";
                }
            }
        }

        private void LoadSeasonalTrends()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"
                    WITH SeasonalBookings AS (
                        SELECT 
                            d.destination_id,
                            d.destination_name,
                            DATEPART(MONTH, t.start_date) as travel_month,
                            COUNT(b.booking_id) as monthly_bookings,
                            AVG(b.total_amount) as avg_booking_value
                        FROM Destination d
                        JOIN Trip t ON d.destination_id = t.destination_id
                        JOIN Booking b ON t.trip_id = b.trip_id
                        WHERE b.status = 'Confirmed' 
                        AND b.is_cancelled = 0
                        GROUP BY d.destination_id, d.destination_name, DATEPART(MONTH, t.start_date)
                    )
                    SELECT 
                        destination_name,
                        travel_month,
                        monthly_bookings,
                        avg_booking_value,
                        CAST(monthly_bookings * 100.0 / SUM(monthly_bookings) OVER (PARTITION BY destination_id) AS DECIMAL(5,2)) as percentage_of_annual_bookings
                    FROM SeasonalBookings
                    ORDER BY destination_name, travel_month";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();

                    // Format columns
                    if (mainGridView.Columns["avg_booking_value"] != null)
                        mainGridView.Columns["avg_booking_value"].DefaultCellStyle.Format = "C2";
                    if (mainGridView.Columns["percentage_of_annual_bookings"] != null)
                        mainGridView.Columns["percentage_of_annual_bookings"].DefaultCellStyle.Format = "N2";
                }
            }
        }

        private void LoadTravelerSatisfaction()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"
                    WITH DestinationRatings AS (
                        SELECT 
                            d.destination_id,
                            d.destination_name,
                            d.country,
                            COUNT(r.review_id) as total_reviews,
                            CAST(AVG(CAST(r.rating AS DECIMAL(4,2))) AS DECIMAL(4,2)) as average_rating,
                            COUNT(CASE WHEN r.rating >= 4 THEN 1 END) as high_ratings,
                            COUNT(CASE WHEN r.rating <= 2 THEN 1 END) as low_ratings
                        FROM Destination d
                        JOIN Trip t ON d.destination_id = t.destination_id
                        LEFT JOIN Review r ON t.trip_id = r.trip_id
                        GROUP BY d.destination_id, d.destination_name, d.country
                    )
                    SELECT 
                        destination_name,
                        country,
                        total_reviews,
                        average_rating,
                        CAST(CAST(high_ratings AS DECIMAL(18,4)) * 100.0 / CAST(NULLIF(total_reviews, 0) AS DECIMAL(18,4)) AS DECIMAL(5,2)) as high_rating_percent,
                        CAST(CAST(low_ratings AS DECIMAL(18,4)) * 100.0 / CAST(NULLIF(total_reviews, 0) AS DECIMAL(18,4)) AS DECIMAL(5,2)) as low_rating_percent
                    FROM DestinationRatings
                    WHERE total_reviews > 0
                    ORDER BY average_rating DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();

                    // Format columns
                    if (mainGridView.Columns["average_rating"] != null)
                        mainGridView.Columns["average_rating"].DefaultCellStyle.Format = "N2";
                    if (mainGridView.Columns["high_rating_percent"] != null)
                        mainGridView.Columns["high_rating_percent"].DefaultCellStyle.Format = "N2";
                    if (mainGridView.Columns["low_rating_percent"] != null)
                        mainGridView.Columns["low_rating_percent"].DefaultCellStyle.Format = "N2";
                }
            }
        }

        private void LoadEmergingDestinations()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"
                    WITH BookingTrends AS (
                        SELECT 
                            d.destination_id,
                            d.destination_name,
                            d.country,
                            d.region,
                            YEAR(b.booking_date) as booking_year,
                            MONTH(b.booking_date) as booking_month,
                            COUNT(*) as monthly_bookings,
                            CAST(AVG(CAST(r.rating AS DECIMAL(4,2))) AS DECIMAL(4,2)) as avg_rating
                        FROM Destination d
                        JOIN Trip t ON d.destination_id = t.destination_id
                        JOIN Booking b ON t.trip_id = b.trip_id
                        LEFT JOIN Review r ON t.trip_id = r.trip_id
                        WHERE b.status = 'Confirmed' 
                        AND b.is_cancelled = 0
                        GROUP BY d.destination_id, d.destination_name, d.country, d.region,
                            YEAR(b.booking_date), MONTH(b.booking_date)
                    )
                    SELECT 
                        destination_name,
                        country,
                        region,
                        CAST(AVG(CAST(monthly_bookings AS DECIMAL(18,2))) AS DECIMAL(18,2)) as avg_monthly_bookings,
                        avg_rating,
                        CAST((
                            SELECT AVG(CAST(monthly_bookings AS DECIMAL(18,4)))
                            FROM BookingTrends bt2
                            WHERE bt2.destination_id = bt1.destination_id
                            AND (bt2.booking_year * 12 + bt2.booking_month) <= 
                                (SELECT MAX(booking_year * 12 + booking_month) - 3 FROM BookingTrends)
                        ) AS DECIMAL(18,2)) as prev_avg_bookings,
                        CAST((
                            AVG(CAST(monthly_bookings AS DECIMAL(18,4))) - (
                                SELECT AVG(CAST(monthly_bookings AS DECIMAL(18,4)))
                                FROM BookingTrends bt2
                                WHERE bt2.destination_id = bt1.destination_id
                                AND (bt2.booking_year * 12 + bt2.booking_month) <= 
                                    (SELECT MAX(booking_year * 12 + booking_month) - 3 FROM BookingTrends)
                            )) * 100.0 / NULLIF((
                                SELECT AVG(CAST(monthly_bookings AS DECIMAL(18,4)))
                                FROM BookingTrends bt2
                                WHERE bt2.destination_id = bt1.destination_id
                                AND (bt2.booking_year * 12 + bt2.booking_month) <= 
                                    (SELECT MAX(booking_year * 12 + booking_month) - 3 FROM BookingTrends)
                            ), 0) AS DECIMAL(5,2)
                        ) as growth_percent
                    FROM BookingTrends bt1
                    GROUP BY destination_id, destination_name, country, region
                    HAVING COUNT(*) >= 3  -- At least 3 months of data
                    ORDER BY growth_percent DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();

                    // Format columns
                    if (mainGridView.Columns["avg_monthly_bookings"] != null)
                        mainGridView.Columns["avg_monthly_bookings"].DefaultCellStyle.Format = "N1";
                    if (mainGridView.Columns["avg_rating"] != null)
                        mainGridView.Columns["avg_rating"].DefaultCellStyle.Format = "N2";
                    if (mainGridView.Columns["prev_avg_bookings"] != null)
                        mainGridView.Columns["prev_avg_bookings"].DefaultCellStyle.Format = "N1";
                    if (mainGridView.Columns["growth_percent"] != null)
                        mainGridView.Columns["growth_percent"].DefaultCellStyle.Format = "N2";
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