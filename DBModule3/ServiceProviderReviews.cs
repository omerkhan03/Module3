using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DBModule3
{
    public partial class ServiceProviderReviews : UserControl
    {
        private int userId;
        private DataGridView reviewsGrid;
        private Label titleLabel;

        public ServiceProviderReviews(int userId = 1)
        {
            InitializeComponent();
            this.userId = userId;
            InitializeUI();
            LoadReviews();
        }

        private void InitializeUI()
        {
            // Add title label
            titleLabel = new Label();
            titleLabel.Text = "MY REVIEWS";
            titleLabel.Font = new System.Drawing.Font("Century Gothic", 14F, System.Drawing.FontStyle.Bold);
            titleLabel.Location = new System.Drawing.Point(20, 20);
            titleLabel.AutoSize = true;
            Controls.Add(titleLabel);

            // Initialize grid
            reviewsGrid = new DataGridView();
            reviewsGrid.Location = new System.Drawing.Point(20, 60);
            reviewsGrid.Width = 700;
            reviewsGrid.Height = 400;
            reviewsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            reviewsGrid.BackgroundColor = System.Drawing.Color.White;
            reviewsGrid.Font = new System.Drawing.Font("Century Gothic", 10F);
            reviewsGrid.AutoGenerateColumns = false;
            reviewsGrid.ReadOnly = true;
            
            // Add columns
            reviewsGrid.Columns.Add("review_id", "Review ID");
            reviewsGrid.Columns.Add("traveler_name", "Traveler");
            reviewsGrid.Columns.Add("rating", "Rating");
            reviewsGrid.Columns.Add("comment", "Comment");
            reviewsGrid.Columns.Add("review_date", "Date");
            reviewsGrid.Columns.Add("is_flagged", "Flagged");

            Controls.Add(reviewsGrid);
        }

        private void LoadReviews()
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    string query = @"
                        SELECT r.review_id, 
                               t.first_name + ' ' + t.last_name as traveler_name,
                               r.rating,
                               r.comment,
                               r.review_date,
                               r.is_flagged
                        FROM Review r
                        JOIN Traveler t ON r.traveler_id = t.traveler_id
                        JOIN ServiceProvider sp ON r.provider_id = sp.provider_id
                        WHERE sp.user_id = @UserId
                        ORDER BY r.review_date DESC";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        reviewsGrid.Rows.Clear();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            reviewsGrid.Rows.Add(
                                row["review_id"],
                                row["traveler_name"],
                                row["rating"],
                                row["comment"],
                                ((DateTime)row["review_date"]).ToShortDateString(),
                                (bool)row["is_flagged"] ? "Yes" : "No"
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading reviews: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ServiceProviderReviews_Load(object sender, EventArgs e)
        {

        }
    }
}
