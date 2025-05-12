using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace DBModule3
{
    public partial class PlatformGrowthReport : UserControl
    {
        private DataGridView mainGridView;
        private ComboBox reportTypeComboBox;
        private Button generateButton;
        private Label titleLabel;

        public PlatformGrowthReport()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Initialize components
            titleLabel = new Label
            {
                Text = "Platform Growth Report",
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
                "New User Registrations",
                "Active Users",
                "Partnership Growth",
                "Regional Expansion",
                "Service Utilization"
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
                    case "New User Registrations":
                        LoadNewUserRegistrations();
                        break;
                    case "Active Users":
                        LoadActiveUsers();
                        break;
                    case "Partnership Growth":
                        LoadPartnershipGrowth();
                        break;
                    case "Regional Expansion":
                        LoadRegionalExpansion();
                        break;
                    case "Service Utilization":
                        LoadServiceUtilization();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadNewUserRegistrations()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"
                    SELECT 
                        YEAR(registration_date) as reg_year,
                        MONTH(registration_date) as reg_month,
                        COUNT(*) as new_users,
                        COUNT(CASE WHEN role = 'traveler' THEN 1 END) as new_travelers,
                        COUNT(CASE WHEN role = 'tour_operator' THEN 1 END) as new_operators,
                        COUNT(CASE WHEN role = 'service_provider' THEN 1 END) as new_providers
                    FROM [User]
                    GROUP BY YEAR(registration_date), MONTH(registration_date)
                    ORDER BY reg_year DESC, reg_month DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();
                }
            }
        }

        private void LoadActiveUsers()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"SELECT 
                        role,
                        COUNT(*) as total_users,
                        COUNT(CASE WHEN is_active = 1 THEN 1 END) as active_users,
                        CAST(COUNT(CASE WHEN is_active = 1 THEN 1 END) * 100.0 / 
                            NULLIF(COUNT(*), 0) AS DECIMAL(5,2)) as active_percentage
                    FROM [User]
                    GROUP BY role
                    ORDER BY total_users DESC;";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();

                    // Format percentage column
                    if (mainGridView.Columns["active_percentage"] != null)
                        mainGridView.Columns["active_percentage"].DefaultCellStyle.Format = "N2";
                }
            }
        }

        private void LoadPartnershipGrowth()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"
                    WITH PartnerGrowth AS (
                        SELECT 
                            YEAR(u.registration_date) as year,
                            MONTH(u.registration_date) as month,
                            COUNT(DISTINCT to2.operator_id) as new_tour_operators,
                            COUNT(DISTINCT sp.provider_id) as new_service_providers,
                            COUNT(DISTINCT h.hotel_id) as new_hotels,
                            COUNT(DISTINCT ts.transport_id) as new_transport_services,
                            COUNT(DISTINCT g.guide_id) as new_guides
                        FROM [User] u
                        LEFT JOIN TourOperator to2 ON u.user_id = to2.user_id
                        LEFT JOIN ServiceProvider sp ON u.user_id = sp.user_id
                        LEFT JOIN Hotel h ON sp.provider_id = h.provider_id
                        LEFT JOIN TransportService ts ON sp.provider_id = ts.provider_id
                        LEFT JOIN Guide g ON sp.provider_id = g.provider_id
                        WHERE u.role IN ('tour_operator', 'service_provider')
                        GROUP BY YEAR(u.registration_date), MONTH(u.registration_date)
                    )
                    SELECT 
                        year,
                        month,
                        new_tour_operators,
                        new_service_providers,
                        new_hotels + new_transport_services + new_guides as new_services,
                        new_tour_operators + new_service_providers as total_new_partners
                    FROM PartnerGrowth
                    ORDER BY year DESC, month DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();
                }
            }
        }

        private void LoadRegionalExpansion()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"
                    WITH RegionalStats AS (
                        SELECT 
                            d.country,
                            d.region,
                            COUNT(DISTINCT t.trip_id) as available_trips,
                            COUNT(DISTINCT b.booking_id) as total_bookings,
                            COUNT(DISTINCT b.traveler_id) as unique_travelers,
                            SUM(b.total_amount) as total_revenue
                        FROM Destination d
                        JOIN Trip t ON d.destination_id = t.destination_id
                        LEFT JOIN Booking b ON t.trip_id = b.trip_id AND b.status = 'Confirmed'
                        GROUP BY d.country, d.region
                    )
                    SELECT 
                        country,
                        region,
                        available_trips,
                        total_bookings,
                        unique_travelers,
                        total_revenue,
                        CAST(total_bookings * 100.0 / NULLIF(available_trips, 0) AS DECIMAL(5,2)) as booking_rate_percentage
                    FROM RegionalStats
                    ORDER BY total_revenue DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();

                    // Format columns
                    if (mainGridView.Columns["total_revenue"] != null)
                        mainGridView.Columns["total_revenue"].DefaultCellStyle.Format = "C2";
                    if (mainGridView.Columns["booking_rate_percentage"] != null)
                        mainGridView.Columns["booking_rate_percentage"].DefaultCellStyle.Format = "N2";
                }
            }
        }

        private void LoadServiceUtilization()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"
                    SELECT 
                        sp.provider_type,
                        COUNT(DISTINCT sp.provider_id) as total_providers,
                        COUNT(sa.assignment_id) as total_assignments,
                        COUNT(DISTINCT sa.booking_id) as unique_bookings,
                        CAST(AVG(CAST(COUNT(sa.assignment_id) AS FLOAT)) OVER (PARTITION BY sp.provider_type) AS DECIMAL(10,2)) as avg_assignments_per_provider
                    FROM ServiceProvider sp
                    LEFT JOIN ServiceAssignment sa ON sp.provider_id = sa.provider_id
                    GROUP BY sp.provider_type
                    ORDER BY total_assignments DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();

                    // Format the average assignments column
                    if (mainGridView.Columns["avg_assignments_per_provider"] != null)
                        mainGridView.Columns["avg_assignments_per_provider"].DefaultCellStyle.Format = "N2";
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