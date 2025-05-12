using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DBModule3
{
    public partial class UserBookings : UserControl
    {
        private int userId;

        public UserBookings()
        {
            InitializeComponent();
        }

        public UserBookings(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadUserBookings();
        }

        private void LoadUserBookings()
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();

                    string query = @"SELECT b.booking_id, t.trip_name, b.booking_date, b.status, 
                                    b.total_amount, b.is_cancelled,
                                    t.start_date, t.end_date, d.destination_name
                                    FROM Booking b
                                    JOIN Trip t ON b.trip_id = t.trip_id
                                    JOIN Destination d ON t.destination_id = d.destination_id
                                    JOIN Traveler tr ON b.traveler_id = tr.traveler_id
                                    WHERE tr.user_id = @UserId
                                    ORDER BY b.booking_date DESC";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Rename columns for better display
                        dataTable.Columns["booking_id"].ColumnName = "Booking ID";
                        dataTable.Columns["trip_name"].ColumnName = "Trip Name";
                        dataTable.Columns["booking_date"].ColumnName = "Booking Date";
                        dataTable.Columns["status"].ColumnName = "Status";
                        dataTable.Columns["total_amount"].ColumnName = "Total Amount";
                        dataTable.Columns["is_cancelled"].ColumnName = "Cancelled";
                        dataTable.Columns["start_date"].ColumnName = "Start Date";
                        dataTable.Columns["end_date"].ColumnName = "End Date";
                        dataTable.Columns["destination_name"].ColumnName = "Destination";

                        // Display data in the DataGridView
                        bookingsDataGridView.DataSource = dataTable;

                        // Format the DataGridView
                        FormatDataGridView();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading bookings: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridView()
        {
            // Set column widths
            bookingsDataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            
            // Format date columns
            foreach (DataGridViewColumn column in bookingsDataGridView.Columns)
            {
                if (column.Name.Contains("Date"))
                {
                    column.DefaultCellStyle.Format = "dd/MM/yyyy";
                }
            }

            // Format the cancelled column
            DataGridViewColumn cancelledColumn = bookingsDataGridView.Columns["Cancelled"];
            if (cancelledColumn != null)
            {
                cancelledColumn.DefaultCellStyle.Format = "Yes;No;No";
            }

            // Format the total amount column
            DataGridViewColumn amountColumn = bookingsDataGridView.Columns["Total Amount"];
            if (amountColumn != null)
            {
                amountColumn.DefaultCellStyle.Format = "C2";
            }

            // Style the grid
            bookingsDataGridView.RowHeadersVisible = false;
            bookingsDataGridView.BorderStyle = BorderStyle.None;
            bookingsDataGridView.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(238, 239, 249);
            bookingsDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            bookingsDataGridView.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkTurquoise;
            bookingsDataGridView.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            bookingsDataGridView.BackgroundColor = System.Drawing.Color.White;
            bookingsDataGridView.EnableHeadersVisualStyles = false;
            bookingsDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            bookingsDataGridView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(20, 25, 72);
            bookingsDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
        }

        private void UserBookings_Load(object sender, EventArgs e)
        {
            if (userId > 0)
            {
                LoadUserBookings();
            }
        }
    }
}
