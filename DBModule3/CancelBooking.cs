using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DBModule3
{
    public partial class PayBooking : UserControl
    {
        private int userId;

        public PayBooking(int userId)
        {
            InitializeComponent();
            InitializeControls();
            this.userId = userId;
        }

        private void InitializeControls()
        {
            // Set up initial text
            bookingtext.PlaceholderText = "Enter Booking ID";
            bookingtext.Texts = "";

            // Wire up button click events
            cancelbutton.Click += CancelButton_Click;
            sataButton1.Click += PayButton_Click;
        }

        private void PayButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(bookingtext.Texts))
            {
                MessageBox.Show("Please enter a booking ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(bookingtext.Texts, out int bookingId))
            {
                MessageBox.Show("Please enter a valid booking ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();

                    // First check if the booking exists, is not cancelled, and belongs to the current user
                    string checkQuery = @"SELECT b.total_amount, b.status, b.is_cancelled 
                                        FROM Booking b
                                        JOIN Traveler t ON b.traveler_id = t.traveler_id
                                        WHERE b.booking_id = @BookingId
                                        AND t.user_id = @UserId";

                    using (SqlCommand checkCmd = new SqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@BookingId", bookingId);
                        checkCmd.Parameters.AddWithValue("@UserId", userId);

                        using (SqlDataReader reader = checkCmd.ExecuteReader())
                        {
                            if (!reader.Read())
                            {
                                MessageBox.Show("Booking not found or you don't have permission to modify it.", 
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            if (reader.GetBoolean(reader.GetOrdinal("is_cancelled")))
                            {
                                MessageBox.Show("This booking has been cancelled.", 
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            if (reader.GetString(reader.GetOrdinal("status")) == "Paid")
                            {
                                MessageBox.Show("This booking has already been paid.", 
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }

                    // Create payment record
                    string paymentQuery = @"INSERT INTO Payment (booking_id, amount, payment_method, status)
                                          SELECT @BookingId, total_amount, @PaymentMethod, 'Completed'
                                          FROM Booking b
                                          JOIN Traveler t ON b.traveler_id = t.traveler_id
                                          WHERE b.booking_id = @BookingId
                                          AND t.user_id = @UserId;
                                          
                                          UPDATE Booking 
                                          SET status = 'Paid'
                                          FROM Booking b
                                          JOIN Traveler t ON b.traveler_id = t.traveler_id
                                          WHERE b.booking_id = @BookingId
                                          AND t.user_id = @UserId;";

                    using (SqlCommand paymentCmd = new SqlCommand(paymentQuery, connection))
                    {
                        paymentCmd.Parameters.AddWithValue("@BookingId", bookingId);
                        paymentCmd.Parameters.AddWithValue("@UserId", userId);
                        paymentCmd.Parameters.AddWithValue("@PaymentMethod", "Credit Card");

                        int rowsAffected = paymentCmd.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Payment processed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            bookingtext.Texts = "";
                        }
                        else
                        {
                            MessageBox.Show("Payment failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error processing payment: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(bookingtext.Texts))
            {
                MessageBox.Show("Please enter a booking ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(bookingtext.Texts, out int bookingId))
            {
                MessageBox.Show("Please enter a valid booking ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Get booking and traveler information, ensuring it belongs to the current user
                        string getBookingInfo = @"SELECT b.traveler_id, b.trip_id, b.is_cancelled
                                                FROM Booking b
                                                JOIN Traveler t ON b.traveler_id = t.traveler_id
                                                WHERE b.booking_id = @BookingId
                                                AND t.user_id = @UserId";

                        int travelerId = 0;
                        int tripId = 0;
                        bool isCancelled = false;

                        using (SqlCommand infoCmd = new SqlCommand(getBookingInfo, connection, transaction))
                        {
                            infoCmd.Parameters.AddWithValue("@BookingId", bookingId);
                            infoCmd.Parameters.AddWithValue("@UserId", userId);

                            using (SqlDataReader reader = infoCmd.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                    MessageBox.Show("Booking not found or you don't have permission to modify it.", 
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                travelerId = reader.GetInt32(reader.GetOrdinal("traveler_id"));
                                tripId = reader.GetInt32(reader.GetOrdinal("trip_id"));
                                isCancelled = reader.GetBoolean(reader.GetOrdinal("is_cancelled"));

                                if (isCancelled)
                                {
                                    MessageBox.Show("This booking is already cancelled.", 
                                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                        }

                        // Cancel the booking
                        string cancelQuery = @"UPDATE Booking 
                                            SET is_cancelled = 1,
                                                status = 'Cancelled',
                                                cancellation_reason = 'Cancelled by user'
                                            FROM Booking b
                                            JOIN Traveler t ON b.traveler_id = t.traveler_id
                                            WHERE b.booking_id = @BookingId
                                            AND t.user_id = @UserId";

                        using (SqlCommand cancelCmd = new SqlCommand(cancelQuery, connection, transaction))
                        {
                            cancelCmd.Parameters.AddWithValue("@BookingId", bookingId);
                            cancelCmd.Parameters.AddWithValue("@UserId", userId);
                            
                            int rowsAffected = cancelCmd.ExecuteNonQuery();
                            if (rowsAffected == 0)
                            {
                                throw new Exception("Failed to cancel the booking.");
                            }
                        }

                        // Add to AbandonedBooking
                        string abandonedQuery = @"INSERT INTO AbandonedBooking (traveler_id, trip_id, stage_abandoned)
                                               VALUES (@TravelerId, @TripId, 'Cancelled')";

                        using (SqlCommand abandonedCmd = new SqlCommand(abandonedQuery, connection, transaction))
                        {
                            abandonedCmd.Parameters.AddWithValue("@TravelerId", travelerId);
                            abandonedCmd.Parameters.AddWithValue("@TripId", tripId);
                            abandonedCmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        MessageBox.Show("Booking cancelled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        bookingtext.Texts = "";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error during cancellation process: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error cancelling booking: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PayBooking_Load(object sender, EventArgs e)
        {
        }
    }
}
