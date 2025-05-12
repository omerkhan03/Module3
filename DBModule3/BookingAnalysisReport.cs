using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace DBModule3
{
    public partial class BookingAnalysisReport : UserControl
    {
        private DataGridView mainGridView;
        private ComboBox reportTypeComboBox;
        private Button generateButton;
        private Label titleLabel;

        public BookingAnalysisReport()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Initialize components
            titleLabel = new Label
            {
                Text = "Abandoned Booking Analysis Report",
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
                "Abandonment Rate",
                "Common Abandonment Reasons",
                "Recovery Rate",
                "Potential Revenue Loss"
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
                    case "Abandonment Rate":
                        LoadAbandonmentRate();
                        break;
                    case "Common Abandonment Reasons":
                        LoadCommonReasons();
                        break;
                    case "Recovery Rate":
                        LoadRecoveryRate();
                        break;
                    case "Potential Revenue Loss":
                        LoadPotentialRevenueLoss();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAbandonmentRate()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"
                    SELECT 
                        COUNT(*) as total_started_bookings,
                        COUNT(CASE WHEN status = 'Confirmed' THEN 1 END) as completed_bookings,
                        COUNT(ab.abandoned_id) as abandoned_bookings,
                        CAST(COUNT(ab.abandoned_id) * 100.0 / NULLIF(COUNT(*), 0) AS DECIMAL(5,2)) as abandonment_rate
                    FROM Booking b
                    LEFT JOIN AbandonedBooking ab ON b.trip_id = ab.trip_id AND b.traveler_id = ab.traveler_id";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();

                    // Format percentage column
                    if (mainGridView.Columns["abandonment_rate"] != null)
                        mainGridView.Columns["abandonment_rate"].DefaultCellStyle.Format = "N2";
                }
            }
        }

        private void LoadCommonReasons()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"
                    SELECT 
                        stage_abandoned,
                        COUNT(*) as abandonment_count,
                        CAST(COUNT(*) * 100.0 / COUNT(*) OVER() AS DECIMAL(5,2)) as percentage
                    FROM AbandonedBooking
                    GROUP BY stage_abandoned
                    ORDER BY abandonment_count DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();

                    // Format percentage column
                    if (mainGridView.Columns["percentage"] != null)
                        mainGridView.Columns["percentage"].DefaultCellStyle.Format = "N2";
                }
            }
        }

        private void LoadRecoveryRate()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"
                    SELECT 
                        COUNT(DISTINCT ab.traveler_id) as total_abandoning_travelers,
                        COUNT(DISTINCT CASE WHEN b.status = 'Confirmed' AND b.booking_date > ab.timestamp 
                              THEN ab.traveler_id END) as recovered_travelers,
                        CAST(COUNT(DISTINCT CASE WHEN b.status = 'Confirmed' AND b.booking_date > ab.timestamp 
                              THEN ab.traveler_id END) * 100.0 / NULLIF(COUNT(DISTINCT ab.traveler_id), 0) AS DECIMAL(5,2)) as recovery_rate
                    FROM AbandonedBooking ab
                    LEFT JOIN Booking b ON ab.traveler_id = b.traveler_id";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();

                    // Format percentage column
                    if (mainGridView.Columns["recovery_rate"] != null)
                        mainGridView.Columns["recovery_rate"].DefaultCellStyle.Format = "N2";
                }
            }
        }

        private void LoadPotentialRevenueLoss()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"
                    SELECT 
                        ab.stage_abandoned,
                        COUNT(*) as abandonment_count,
                        AVG(t.price) as avg_trip_price,
                        SUM(t.price) as potential_revenue_loss
                    FROM AbandonedBooking ab
                    JOIN Trip t ON ab.trip_id = t.trip_id
                    GROUP BY ab.stage_abandoned
                    ORDER BY potential_revenue_loss DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();

                    // Format monetary columns
                    if (mainGridView.Columns["avg_trip_price"] != null)
                        mainGridView.Columns["avg_trip_price"].DefaultCellStyle.Format = "C2";
                    if (mainGridView.Columns["potential_revenue_loss"] != null)
                        mainGridView.Columns["potential_revenue_loss"].DefaultCellStyle.Format = "C2";
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