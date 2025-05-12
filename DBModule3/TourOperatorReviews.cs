using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DBModule3
{
    public partial class TourOperatorReviews : UserControl
    {
        private int userId;

        public TourOperatorReviews()
        {
            InitializeComponent();
        }

        public TourOperatorReviews(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadTourOperatorReviews();
        }

        private void LoadTourOperatorReviews()
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            r.review_id,
                            t.trip_name,
                            tr.first_name + ' ' + tr.last_name as reviewer_name,
                            r.rating,
                            r.comment,
                            r.review_date,
                            r.is_flagged
                        FROM Review r
                        JOIN Trip t ON r.trip_id = t.trip_id
                        JOIN TourOperator o ON t.operator_id = o.operator_id
                        JOIN Traveler tr ON r.traveler_id = tr.traveler_id
                        WHERE o.user_id = @UserId
                        ORDER BY r.review_date DESC";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Rename columns for better display
                        dataTable.Columns["review_id"].ColumnName = "Review ID";
                        dataTable.Columns["trip_name"].ColumnName = "Trip Name";
                        dataTable.Columns["reviewer_name"].ColumnName = "Reviewer";
                        dataTable.Columns["rating"].ColumnName = "Rating";
                        dataTable.Columns["comment"].ColumnName = "Comment";
                        dataTable.Columns["review_date"].ColumnName = "Review Date";
                        dataTable.Columns["is_flagged"].ColumnName = "Flagged";

                        // Display data in the DataGridView
                        reviewsDataGridView.DataSource = dataTable;

                        // Format the DataGridView
                        FormatDataGridView();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading reviews: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridView()
        {
            // Set column widths
            reviewsDataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            
            // Format date columns
            foreach (DataGridViewColumn column in reviewsDataGridView.Columns)
            {
                if (column.Name.Contains("Date"))
                {
                    column.DefaultCellStyle.Format = "dd/MM/yyyy";
                }

                // Format rating column to show one decimal place
                if (column.Name == "Rating")
                {
                    column.DefaultCellStyle.Format = "N1";
                }
            }

            // Format the flagged column to show Yes/No
            DataGridViewColumn flaggedColumn = reviewsDataGridView.Columns["Flagged"];
            if (flaggedColumn != null)
            {
                flaggedColumn.DefaultCellStyle.Format = "Yes;No;No";
            }

            // Style the grid
            reviewsDataGridView.RowHeadersVisible = false;
            reviewsDataGridView.BorderStyle = BorderStyle.None;
            reviewsDataGridView.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(238, 239, 249);
            reviewsDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            reviewsDataGridView.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkTurquoise;
            reviewsDataGridView.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            reviewsDataGridView.BackgroundColor = System.Drawing.Color.White;
            reviewsDataGridView.EnableHeadersVisualStyles = false;
            reviewsDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            reviewsDataGridView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(20, 25, 72);
            reviewsDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
        }
    }
}
