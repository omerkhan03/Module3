using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace DBModule3
{
    public partial class PaymentAnalyticsReport : UserControl
    {
        private DataGridView mainGridView;
        private ComboBox reportTypeComboBox;
        private Button generateButton;
        private Label titleLabel;

        public PaymentAnalyticsReport()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Initialize components
            titleLabel = new Label
            {
                Text = "Payment Analytics Report",
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
                "Payment Success/Failure Rate",
                "Payment Method Analysis",
                "Chargeback Rate"
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
                    case "Payment Success/Failure Rate":
                        LoadPaymentSuccessRate();
                        break;
                    case "Payment Method Analysis":
                        LoadPaymentMethodAnalysis();
                        break;
                    case "Chargeback Rate":
                        LoadChargebackRate();
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error generating report: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadPaymentSuccessRate()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();                
                string query = @"
                    SELECT 
                        status, 
                        COUNT(*) as total_transactions, 
                        CAST(CAST(COUNT(*) AS DECIMAL(18,4)) * 100.0 / CAST(NULLIF(SUM(COUNT(*)) OVER(), 0) AS DECIMAL(18,4)) AS DECIMAL(5,2)) as percentage, 
                        SUM(amount) as total_amount,
                        AVG(CAST(amount AS DECIMAL(18,2))) as average_amount
                    FROM Payment
                    GROUP BY status
                    ORDER BY total_transactions DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();

                    // Format columns
                    if (mainGridView.Columns["percentage"] != null)
                        mainGridView.Columns["percentage"].DefaultCellStyle.Format = "N2";
                    if (mainGridView.Columns["total_amount"] != null)
                        mainGridView.Columns["total_amount"].DefaultCellStyle.Format = "C2";
                    if (mainGridView.Columns["average_amount"] != null)
                        mainGridView.Columns["average_amount"].DefaultCellStyle.Format = "C2";
                }
            }
        }

        private void LoadPaymentMethodAnalysis()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"
                    SELECT 
                        payment_method,
                        status,
                        COUNT(*) as transaction_count,
                        CAST(CAST(COUNT(*) AS DECIMAL(18,4)) * 100.0 / CAST(NULLIF(SUM(COUNT(*)) OVER(PARTITION BY payment_method), 0) AS DECIMAL(18,4)) AS DECIMAL(5,2)) as percentage_by_method,
                        SUM(amount) as total_amount
                    FROM Payment
                    GROUP BY payment_method, status
                    ORDER BY payment_method, status";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();

                    // Format columns
                    if (mainGridView.Columns["percentage_by_method"] != null)
                        mainGridView.Columns["percentage_by_method"].DefaultCellStyle.Format = "N2";
                    if (mainGridView.Columns["total_amount"] != null)
                        mainGridView.Columns["total_amount"].DefaultCellStyle.Format = "C2";
                }
            }
        }

        private void LoadChargebackRate()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                string query = @"
                    WITH PaymentStats AS (
                        SELECT
                            MONTH(payment_date) as payment_month,
                            YEAR(payment_date) as payment_year,
                            COUNT(*) as total_transactions,
                            COUNT(CASE WHEN is_refunded = 1 THEN 1 END) as refunded_transactions,
                            SUM(amount) as total_amount,
                            SUM(CASE WHEN is_refunded = 1 THEN amount ELSE 0 END) as refunded_amount
                        FROM Payment
                        WHERE status = 'Completed'  -- Only consider completed payments
                        GROUP BY YEAR(payment_date), MONTH(payment_date)
                    )
                    SELECT 
                        payment_year,
                        payment_month,
                        total_transactions,
                        refunded_transactions,
                        CAST(CAST(refunded_transactions AS DECIMAL(18,4)) * 100.0 / CAST(NULLIF(total_transactions, 0) AS DECIMAL(18,4)) AS DECIMAL(5,2)) as refund_rate,
                        total_amount,
                        refunded_amount,
                        CAST(CAST(refunded_amount AS DECIMAL(18,4)) * 100.0 / CAST(NULLIF(total_amount, 0) AS DECIMAL(18,4)) AS DECIMAL(5,2)) as refund_amount_percent
                    FROM PaymentStats
                    ORDER BY payment_year DESC, payment_month DESC";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());
                    mainGridView.DataSource = dt;
                    FormatGridView();

                    // Format columns
                    if (mainGridView.Columns["refund_rate"] != null)
                        mainGridView.Columns["refund_rate"].DefaultCellStyle.Format = "N2";
                    if (mainGridView.Columns["refund_amount_percent"] != null)
                        mainGridView.Columns["refund_amount_percent"].DefaultCellStyle.Format = "N2";
                    if (mainGridView.Columns["total_amount"] != null)
                        mainGridView.Columns["total_amount"].DefaultCellStyle.Format = "C2";
                    if (mainGridView.Columns["refunded_amount"] != null)
                        mainGridView.Columns["refunded_amount"].DefaultCellStyle.Format = "C2";
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