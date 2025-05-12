using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace DBModule3
{
    public partial class ReviewManager : UserControl
    {
        public ReviewManager()
        {
            InitializeComponent();
            LoadReviews();
        }

        private void LoadReviews()
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            r.review_id,
                            CONCAT(t.first_name, ' ', t.last_name) as traveler_name,
                            CASE 
                                WHEN r.trip_id IS NOT NULL THEN CONCAT('Trip: ', tr.trip_name)
                                ELSE ''
                            END as reviewed_trip,
                            CASE 
                                WHEN r.provider_id IS NOT NULL THEN 
                                    CASE 
                                        WHEN h.hotel_id IS NOT NULL THEN CONCAT('Hotel: ', h.hotel_name)
                                        WHEN ts.transport_id IS NOT NULL THEN CONCAT('Transport: ', ts.transport_type)
                                        WHEN g.guide_id IS NOT NULL THEN CONCAT('Guide: ', g.guide_name)
                                        ELSE 'Unknown Provider'
                                    END
                                ELSE ''
                            END as reviewed_provider,
                            r.rating,
                            r.comment,
                            r.review_date,
                            r.is_flagged
                        FROM Review r
                        JOIN Traveler t ON r.traveler_id = t.traveler_id
                        LEFT JOIN Trip tr ON r.trip_id = tr.trip_id
                        LEFT JOIN ServiceProvider sp ON r.provider_id = sp.provider_id
                        LEFT JOIN Hotel h ON sp.provider_id = h.provider_id
                        LEFT JOIN TransportService ts ON sp.provider_id = ts.provider_id
                        LEFT JOIN Guide g ON sp.provider_id = g.provider_id
                        ORDER BY r.review_date DESC";                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                    {
                        DataTable reviewsTable = new DataTable();
                        adapter.Fill(reviewsTable);
                        
                        // Clear existing columns including button column
                        reviewsGrid.Columns.Clear();
                        
                        // Set the data source
                        reviewsGrid.DataSource = reviewsTable;
                        
                        // Re-add the button column
                        DataGridViewButtonColumn flagButton = new DataGridViewButtonColumn();
                        flagButton.Name = "FlagButton";
                        flagButton.HeaderText = "Flag/Unflag";
                        flagButton.Text = "Toggle Flag";
                        flagButton.UseColumnTextForButtonValue = true;
                        reviewsGrid.Columns.Add(flagButton);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading reviews: {ex.Message}", 
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FlagReview(int reviewId, bool flag)
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();

                    string query = "UPDATE Review SET is_flagged = @IsFlagged WHERE review_id = @ReviewId";
                    
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IsFlagged", flag);
                        cmd.Parameters.AddWithValue("@ReviewId", reviewId);
                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            LoadReviews(); // Refresh the grid
                        }
                        else
                        {
                            MessageBox.Show("Review not found.", "Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating review: {ex.Message}", 
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void reviewsGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == reviewsGrid.Columns["FlagButton"].Index)
            {
                int reviewId = (int)reviewsGrid.Rows[e.RowIndex].Cells["review_id"].Value;
                bool currentlyFlagged = (bool)reviewsGrid.Rows[e.RowIndex].Cells["is_flagged"].Value;
                FlagReview(reviewId, !currentlyFlagged);
            }
        }
    }
}
