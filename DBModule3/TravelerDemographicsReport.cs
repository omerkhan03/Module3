using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace DBModule3
{
    public partial class TravelerDemographicsReport : UserControl
    {
        private DataGridView mainGridView;
        private ComboBox reportTypeComboBox;
        private Button generateButton;
        private Label titleLabel;

        public TravelerDemographicsReport()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Initialize components
            titleLabel = new Label
            {
                Text = "Traveler Demographics and Preferences Report",
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
                "Age and Nationality Distribution",
                "Preferred Trip Types",
                "Spending Habits"
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
                    case "Age and Nationality Distribution":
                        LoadAgeAndNationalityDistribution();
                        break;
                    case "Preferred Trip Types":
                        LoadPreferredTripTypes();
                        break;
                    case "Spending Habits":
                        LoadSpendingHabits();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadAgeAndNationalityDistribution()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"
                    SELECT 
                        CASE 
                            WHEN DATEDIFF(YEAR, date_of_birth, GETDATE()) < 25 THEN 'Under 25'
                            WHEN DATEDIFF(YEAR, date_of_birth, GETDATE()) BETWEEN 25 AND 34 THEN '25-34'
                            WHEN DATEDIFF(YEAR, date_of_birth, GETDATE()) BETWEEN 35 AND 44 THEN '35-44'
                            WHEN DATEDIFF(YEAR, date_of_birth, GETDATE()) BETWEEN 45 AND 54 THEN '45-54'
                            WHEN DATEDIFF(YEAR, date_of_birth, GETDATE()) BETWEEN 55 AND 64 THEN '55-64'
                            ELSE '65 and above'
                        END as age_group,
                        nationality,
                        COUNT(*) as traveler_count
                    FROM Traveler
                    WHERE date_of_birth IS NOT NULL
                    GROUP BY 
                        CASE 
                            WHEN DATEDIFF(YEAR, date_of_birth, GETDATE()) < 25 THEN 'Under 25'
                            WHEN DATEDIFF(YEAR, date_of_birth, GETDATE()) BETWEEN 25 AND 34 THEN '25-34'
                            WHEN DATEDIFF(YEAR, date_of_birth, GETDATE()) BETWEEN 35 AND 44 THEN '35-44'
                            WHEN DATEDIFF(YEAR, date_of_birth, GETDATE()) BETWEEN 45 AND 54 THEN '45-54'
                            WHEN DATEDIFF(YEAR, date_of_birth, GETDATE()) BETWEEN 55 AND 64 THEN '55-64'
                            ELSE '65 and above'
                        END,
                        nationality
                    ORDER BY age_group, traveler_count DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();
                }
            }
        }

        private void LoadPreferredTripTypes()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"
                    SELECT 
                        tc.category_name,
                        COUNT(DISTINCT tp.traveler_id) as number_of_travelers,
                        COUNT(b.booking_id) as number_of_bookings,
                        CAST(COUNT(b.booking_id) * 100.0 / COUNT(*) OVER() AS DECIMAL(5,2)) as booking_percentage
                    FROM TripCategory tc
                    JOIN Trip t ON tc.category_id = t.category_id
                    JOIN Booking b ON t.trip_id = b.trip_id
                    JOIN Traveler tp ON b.traveler_id = tp.traveler_id
                    WHERE b.status = 'Confirmed'
                    AND b.is_cancelled = 0
                    GROUP BY tc.category_name
                    ORDER BY number_of_bookings DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();

                    // Format percentage column
                    if (mainGridView.Columns["booking_percentage"] != null)
                    {
                        mainGridView.Columns["booking_percentage"].DefaultCellStyle.Format = "N2";
                    }
                }
            }
        }

        private void LoadSpendingHabits()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"
                    WITH TravelerSpending AS (
                        SELECT 
                            t.traveler_id,
                            t.first_name + ' ' + t.last_name as traveler_name,
                            COUNT(b.booking_id) as total_trips,
                            SUM(b.total_amount) as total_spent,
                            AVG(b.total_amount) as avg_trip_cost,
                            MAX(b.total_amount) as highest_trip_cost,
                            MIN(b.total_amount) as lowest_trip_cost
                        FROM Traveler t
                        JOIN Booking b ON t.traveler_id = b.traveler_id
                        WHERE b.status = 'Confirmed' 
                        AND b.is_cancelled = 0
                        GROUP BY t.traveler_id, t.first_name, t.last_name
                    )
                    SELECT 
                        CASE 
                            WHEN total_spent > 10000 THEN 'High Spender'
                            WHEN total_spent BETWEEN 5000 AND 10000 THEN 'Medium Spender'
                            ELSE 'Budget Traveler'
                        END as spending_category,
                        COUNT(*) as number_of_travelers,
                        AVG(total_spent) as average_total_spent,
                        AVG(avg_trip_cost) as average_trip_cost,
                        AVG(total_trips) as average_trips_taken
                    FROM TravelerSpending
                    GROUP BY 
                        CASE 
                            WHEN total_spent > 10000 THEN 'High Spender'
                            WHEN total_spent BETWEEN 5000 AND 10000 THEN 'Medium Spender'
                            ELSE 'Budget Traveler'
                        END
                    ORDER BY average_total_spent DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();

                    // Format monetary columns
                    string[] moneyColumns = { "average_total_spent", "average_trip_cost" };
                    foreach (string columnName in moneyColumns)
                    {
                        if (mainGridView.Columns[columnName] != null)
                        {
                            mainGridView.Columns[columnName].DefaultCellStyle.Format = "C2";
                        }
                    }

                    // Format average trips
                    if (mainGridView.Columns["average_trips_taken"] != null)
                    {
                        mainGridView.Columns["average_trips_taken"].DefaultCellStyle.Format = "N1";
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