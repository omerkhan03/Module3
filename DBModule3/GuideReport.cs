using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing;

namespace DBModule3
{
    public class GuideReport : UserControl
    {
        private int userId;
        private DataGridView ratingGrid;
        private DataGridView languageGrid;
        private Label titleLabel;

        public GuideReport(int userId)
        {
            this.userId = userId;
            InitializeComponent();
            LoadReports();
        }

        private void InitializeComponent()
        {
            this.ratingGrid = new DataGridView();
            this.languageGrid = new DataGridView();
            this.titleLabel = new Label();

            ((System.ComponentModel.ISupportInitialize)(this.ratingGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.languageGrid)).BeginInit();
            this.SuspendLayout();

            // 
            // titleLabel
            // 
            this.titleLabel = new Label
            {
                Text = "Guide Performance Report",
                Font = new Font("Century Gothic", 14, FontStyle.Bold),
                Dock = DockStyle.Top,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 40
            };

            // 
            // ratingGrid
            // 
            this.ratingGrid.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.ratingGrid.Location = new Point(10, 50);
            this.ratingGrid.Width = ClientSize.Width - 20;
            this.ratingGrid.Height = 200;
            this.ratingGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.ratingGrid.BackgroundColor = Color.White;
            this.ratingGrid.Font = new Font("Century Gothic", 10);
            this.ratingGrid.ReadOnly = true;
            this.ratingGrid.AllowUserToAddRows = false;
            this.ratingGrid.RowHeadersVisible = false;
            this.ratingGrid.BorderStyle = BorderStyle.None;
            this.ratingGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.ratingGrid.DefaultCellStyle = new DataGridViewCellStyle
            {
                SelectionBackColor = Color.DarkTurquoise,
                SelectionForeColor = Color.WhiteSmoke
            };

            // 
            // languageGrid
            // 
            this.languageGrid.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.languageGrid.Location = new Point(10, 270);
            this.languageGrid.Width = ClientSize.Width - 20;
            this.languageGrid.Height = 200;
            this.languageGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.languageGrid.BackgroundColor = Color.White;
            this.languageGrid.Font = new Font("Century Gothic", 10);
            this.languageGrid.ReadOnly = true;
            this.languageGrid.AllowUserToAddRows = false;
            this.languageGrid.RowHeadersVisible = false;
            this.languageGrid.BorderStyle = BorderStyle.None;
            this.languageGrid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.languageGrid.DefaultCellStyle = new DataGridViewCellStyle
            {
                SelectionBackColor = Color.DarkTurquoise,
                SelectionForeColor = Color.WhiteSmoke
            };

            this.Controls.Add(titleLabel);
            this.Controls.Add(ratingGrid);
            this.Controls.Add(languageGrid);
            this.Dock = DockStyle.Fill;

            ((System.ComponentModel.ISupportInitialize)(this.ratingGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.languageGrid)).EndInit();
            this.ResumeLayout(false);
        }

        private void LoadReports()
        {
            LoadRatings();
            LoadLanguages();
        }

        private void LoadRatings()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.CreateConnection())
                {
                    conn.Open();
                    string query = @"
                        WITH GuideRatings AS (
                            SELECT 
                                g.guide_id,
                                g.guide_name,
                                COUNT(r.review_id) as total_reviews,
                                AVG(CAST(r.rating AS DECIMAL(3,2))) as average_rating,
                                COUNT(CASE WHEN r.rating >= 4 THEN 1 END) as high_ratings,
                                COUNT(CASE WHEN r.rating <= 2 THEN 1 END) as low_ratings
                            FROM Guide g
                            JOIN ServiceProvider sp ON g.provider_id = sp.provider_id
                            LEFT JOIN Review r ON sp.provider_id = r.provider_id
                            WHERE sp.user_id = @UserId
                            GROUP BY g.guide_id, g.guide_name
                        )
                        SELECT 
                            guide_name as [Guide Name],
                            total_reviews as [Total Reviews],
                            average_rating as [Average Rating],
                            CAST(high_ratings * 100.0 / NULLIF(total_reviews, 0) AS DECIMAL(5,2)) as [High Rating %],
                            CAST(low_ratings * 100.0 / NULLIF(total_reviews, 0) AS DECIMAL(5,2)) as [Low Rating %]
                        FROM GuideRatings
                        ORDER BY average_rating DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            ratingGrid.DataSource = dt;

                            // Format columns
                            if (ratingGrid.Columns["Average Rating"] != null)
                                ratingGrid.Columns["Average Rating"].DefaultCellStyle.Format = "N2";
                            if (ratingGrid.Columns["High Rating %"] != null)
                                ratingGrid.Columns["High Rating %"].DefaultCellStyle.Format = "N2";
                            if (ratingGrid.Columns["Low Rating %"] != null)
                                ratingGrid.Columns["Low Rating %"].DefaultCellStyle.Format = "N2";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading ratings: {ex.Message}");
            }
        }

        private void LoadLanguages()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.CreateConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            l.language_name as [Language],
                            COUNT(gl.guide_id) as [Number of Guides],
                            CAST(COUNT(gl.guide_id) * 100.0 / 
                                (SELECT COUNT(DISTINCT g.guide_id) 
                                FROM Guide g 
                                JOIN ServiceProvider sp ON g.provider_id = sp.provider_id 
                                WHERE sp.user_id = @UserId) AS DECIMAL(5,2)) as [% of Guides]
                        FROM Language l
                        JOIN GuideLanguage gl ON l.language_id = gl.language_id
                        JOIN Guide g ON gl.guide_id = g.guide_id
                        JOIN ServiceProvider sp ON g.provider_id = sp.provider_id
                        WHERE sp.user_id = @UserId
                        GROUP BY l.language_id, l.language_name
                        ORDER BY COUNT(gl.guide_id) DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            languageGrid.DataSource = dt;

                            // Format percentage column
                            if (languageGrid.Columns["% of Guides"] != null)
                                languageGrid.Columns["% of Guides"].DefaultCellStyle.Format = "N2";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading languages: {ex.Message}");
            }
        }
    }
}
