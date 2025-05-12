using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DBModule3
{
    public partial class TravelPasses : UserControl
    {
        private int userId;

        public TravelPasses(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadTravelPasses();
        }

        private void LoadTravelPasses()
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            dp.pass_id,
                            t.trip_name,
                            b.booking_id,
                            dp.pass_type,
                            dp.expiry_date,
                            dp.document_link,
                            dp.qr_code
                        FROM DigitalPass dp
                        JOIN Booking b ON dp.booking_id = b.booking_id
                        JOIN Trip t ON b.trip_id = t.trip_id
                        JOIN Traveler tr ON b.traveler_id = tr.traveler_id
                        WHERE tr.user_id = @UserId
                        ORDER BY dp.expiry_date DESC";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Rename columns for better display
                        dataTable.Columns["pass_id"].ColumnName = "Pass ID";
                        dataTable.Columns["trip_name"].ColumnName = "Trip Name";
                        dataTable.Columns["booking_id"].ColumnName = "Booking ID";
                        dataTable.Columns["pass_type"].ColumnName = "Pass Type";
                        dataTable.Columns["expiry_date"].ColumnName = "Expiry Date";
                        dataTable.Columns["document_link"].ColumnName = "Document Link";
                        dataTable.Columns["qr_code"].ColumnName = "QR Code";

                        bookingsDataGridView.DataSource = dataTable;

                        // Format the DataGridView
                        FormatDataGridView();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading travel passes: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridView()
        {
            bookingsDataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            // Format date columns
            foreach (DataGridViewColumn column in bookingsDataGridView.Columns)
            {
                if (column.Name.Contains("Date"))
                {
                    column.DefaultCellStyle.Format = "dd/MM/yyyy";
                }
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

        private void bookingsDataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Handle click events if needed
        }
    }
}
