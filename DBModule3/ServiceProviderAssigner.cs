using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DBModule3
{
    public partial class ServiceProviderAssigner : UserControl
    {
        private int userId;

        public ServiceProviderAssigner(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadPendingBookings();
            LoadServiceProviders();
            assignButton.Click += AssignButton_Click;
        }

        private void LoadPendingBookings()
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    string query = @"SELECT b.booking_id, t.trip_name, b.booking_date, tr.first_name + ' ' + tr.last_name as traveler_name
                                   FROM Booking b
                                   JOIN Trip t ON b.trip_id = t.trip_id
                                   JOIN TourOperator to2 ON t.operator_id = to2.operator_id
                                   JOIN Traveler tr ON b.traveler_id = tr.traveler_id
                                   WHERE to2.user_id = @UserId
                                   ORDER BY b.booking_date DESC";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        DataTable dt = new DataTable();
                        dt.Load(cmd.ExecuteReader());
                        pendingBookingsGrid.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading bookings: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadServiceProviders()
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    string query = @"SELECT sp.provider_id, 
                                   CASE 
                                       WHEN h.hotel_name IS NOT NULL THEN h.hotel_name
                                       WHEN ts.transport_type IS NOT NULL THEN 'Transport: ' + ts.transport_type
                                       WHEN g.guide_name IS NOT NULL THEN 'Guide: ' + g.guide_name
                                       ELSE 'Unknown Provider Type'
                                   END as provider_name,
                                   sp.provider_type
                                   FROM ServiceProvider sp
                                   LEFT JOIN Hotel h ON sp.provider_id = h.provider_id
                                   LEFT JOIN TransportService ts ON sp.provider_id = ts.provider_id
                                   LEFT JOIN Guide g ON sp.provider_id = g.provider_id
                                   ORDER BY sp.provider_type, provider_name";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        DataTable dt = new DataTable();
                        dt.Load(cmd.ExecuteReader());
                        providersGrid.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading service providers: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AssignButton_Click(object sender, EventArgs e)
        {
            if (pendingBookingsGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a booking", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (providersGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a service provider", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int bookingId = Convert.ToInt32(pendingBookingsGrid.SelectedRows[0].Cells["booking_id"].Value);
            int providerId = Convert.ToInt32(providersGrid.SelectedRows[0].Cells["provider_id"].Value);

            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Check if assignment already exists
                        string checkQuery = @"SELECT COUNT(*) FROM ServiceAssignment 
                                           WHERE booking_id = @BookingId 
                                           AND provider_id = @ProviderId";

                        using (SqlCommand cmd = new SqlCommand(checkQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@BookingId", bookingId);
                            cmd.Parameters.AddWithValue("@ProviderId", providerId);
                            int exists = (int)cmd.ExecuteScalar();
                            if (exists > 0)
                            {
                                MessageBox.Show("This provider is already assigned to this booking.",
                                    "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }

                        // Create the assignment
                        string insertQuery = @"INSERT INTO ServiceAssignment (booking_id, provider_id, status)
                                            VALUES (@BookingId, @ProviderId, 'Pending')";

                        using (SqlCommand cmd = new SqlCommand(insertQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@BookingId", bookingId);
                            cmd.Parameters.AddWithValue("@ProviderId", providerId);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        MessageBox.Show("Service provider successfully assigned!", 
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error creating assignment: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pendingBookingsGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
