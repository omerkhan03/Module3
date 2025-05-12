using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DBModule3
{
    public partial class TourOperatorTrips : UserControl
    {
        private int userId;

        public TourOperatorTrips()
        {
            InitializeComponent();
        }

        public TourOperatorTrips(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadTourOperatorTrips();
        }

        private void LoadTourOperatorTrips()
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            t.trip_id,
                            t.trip_name,
                            d.destination_name,
                            tc.category_name,
                            t.start_date,
                            t.end_date,
                            t.duration,
                            t.price,
                            t.capacity,
                            t.sustainability_score,
                            CASE WHEN t.wheelchair_access = 1 THEN 'Yes' ELSE 'No' END AS wheelchair_access,
                            (
                                SELECT COUNT(*)
                                FROM Booking b
                                WHERE b.trip_id = t.trip_id AND b.is_cancelled = 0
                            ) as booked_count
                        FROM Trip t
                        JOIN Destination d ON t.destination_id = d.destination_id
                        JOIN TripCategory tc ON t.category_id = tc.category_id
                        JOIN TourOperator o ON t.operator_id = o.operator_id
                        WHERE o.user_id = @UserId
                        ORDER BY t.start_date DESC";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Rename columns for better display
                        dataTable.Columns["trip_id"].ColumnName = "Trip ID";
                        dataTable.Columns["trip_name"].ColumnName = "Trip Name";
                        dataTable.Columns["destination_name"].ColumnName = "Destination";
                        dataTable.Columns["category_name"].ColumnName = "Category";
                        dataTable.Columns["start_date"].ColumnName = "Start Date";
                        dataTable.Columns["end_date"].ColumnName = "End Date";
                        dataTable.Columns["duration"].ColumnName = "Duration (days)";
                        dataTable.Columns["price"].ColumnName = "Price";
                        dataTable.Columns["capacity"].ColumnName = "Capacity";
                        dataTable.Columns["sustainability_score"].ColumnName = "Sustainability";
                        dataTable.Columns["wheelchair_access"].ColumnName = "Wheelchair Access";
                        dataTable.Columns["booked_count"].ColumnName = "Bookings";

                        // Display data in the DataGridView
                        tripsDataGridView.DataSource = dataTable;

                        // Format the DataGridView
                        FormatDataGridView();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading trips: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridView()
        {
            // Set column widths
            tripsDataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            
            // Format date columns
            foreach (DataGridViewColumn column in tripsDataGridView.Columns)
            {
                if (column.Name.Contains("Date"))
                {
                    column.DefaultCellStyle.Format = "dd/MM/yyyy";
                }
            }

            // Format the price column
            DataGridViewColumn priceColumn = tripsDataGridView.Columns["Price"];
            if (priceColumn != null)
            {
                priceColumn.DefaultCellStyle.Format = "C2";
            }

            // Style the grid
            tripsDataGridView.RowHeadersVisible = false;
            tripsDataGridView.BorderStyle = BorderStyle.None;
            tripsDataGridView.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(238, 239, 249);
            tripsDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            tripsDataGridView.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkTurquoise;
            tripsDataGridView.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            tripsDataGridView.BackgroundColor = System.Drawing.Color.White;
            tripsDataGridView.EnableHeadersVisualStyles = false;
            tripsDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            tripsDataGridView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(20, 25, 72);
            tripsDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
        }
    }
}
