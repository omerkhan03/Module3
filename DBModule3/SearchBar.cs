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
    public partial class SearchBar : UserControl
    {
        // Event to notify parent when search is triggered
        public event EventHandler<SearchEventArgs> SearchRequested;
        private int userId;

        public SearchBar(int userId = 0)
        {
            InitializeComponent();
            this.userId = userId;
            InitializeControls();
        }

        private void InitializeControls()
        {
            sataTextBox1.Texts = ""; // Clear default text
            TripIDbox.Texts = ""; // Clear default text
            searchbutton.Click += Searchbutton_Click;
            BookButton.Click += BookButton_Click;
        }

        private void Searchbutton_Click(object sender, EventArgs e)
        {
            // Trigger search event with the search term
            string searchTerm = sataTextBox1.Texts.Trim();
            SearchRequested?.Invoke(this, new SearchEventArgs(searchTerm));
        }

        private void BookButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TripIDbox.Texts))
            {
                MessageBox.Show("Please enter a Trip ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(TripIDbox.Texts, out int tripId))
            {
                MessageBox.Show("Please enter a valid Trip ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        // First get the traveler_id for the current user
                        string travelerQuery = "SELECT traveler_id FROM Traveler WHERE user_id = @UserId";
                        int travelerId;

                        using (SqlCommand travelerCmd = new SqlCommand(travelerQuery, connection, transaction))
                        {
                            travelerCmd.Parameters.AddWithValue("@UserId", userId);
                            object result = travelerCmd.ExecuteScalar();
                            if (result == null)
                            {
                                throw new Exception("User is not registered as a traveler.");
                            }
                            travelerId = (int)result;
                        }

                        // Check if the trip exists and get its details
                        decimal tripPrice = 0;
                        int tripCapacity = 0;
                        int bookedCount = 0;
                        bool isPast = false;

                        string tripQuery = @"SELECT price, capacity, 
                            (SELECT COUNT(*) FROM Booking WHERE trip_id = @TripId AND is_cancelled = 0) as booked_count,
                            CASE WHEN start_date <= GETDATE() THEN 1 ELSE 0 END as is_past
                            FROM Trip WHERE trip_id = @TripId";

                        using (SqlCommand tripCmd = new SqlCommand(tripQuery, connection, transaction))
                        {
                            tripCmd.Parameters.AddWithValue("@TripId", tripId);
                            
                            using (SqlDataReader reader = tripCmd.ExecuteReader())
                            {
                                if (!reader.Read())
                                {
                                    throw new Exception("Trip not found.");
                                }

                                tripPrice = reader.GetDecimal(0);
                                tripCapacity = reader.GetInt32(1);
                                bookedCount = reader.GetInt32(2);
                                isPast = reader.GetInt32(3) == 1;
                            }
                        }

                        // Validate trip status
                        if (isPast)
                        {
                            throw new Exception("Cannot book a trip that has already started.");
                        }

                        if (bookedCount >= tripCapacity)
                        {
                            throw new Exception("Trip is fully booked.");
                        }

                        // Create the booking
                        string bookingQuery = @"INSERT INTO Booking (traveler_id, trip_id, status, total_amount, is_cancelled)
                                            VALUES (@TravelerId, @TripId, 'Pending', @TotalAmount, 0)";

                        using (SqlCommand bookingCmd = new SqlCommand(bookingQuery, connection, transaction))
                        {
                            bookingCmd.Parameters.AddWithValue("@TravelerId", travelerId);
                            bookingCmd.Parameters.AddWithValue("@TripId", tripId);
                            bookingCmd.Parameters.AddWithValue("@TotalAmount", tripPrice);
                            bookingCmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        MessageBox.Show("Trip booked successfully! Please proceed to My Bookings to complete the payment.", 
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        TripIDbox.Texts = "";
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error during booking process: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error booking trip: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Public method to get the current search term
        public string GetSearchTerm()
        {
            return sataTextBox1.Texts.Trim();
        }

        private void searchbutton_Click_1(object sender, EventArgs e)
        {

        }

        private void sataTextBox1_Click(object sender, EventArgs e)
        {

        }
    }

    // Event arguments class for search events
    public class SearchEventArgs : EventArgs
    {
        public string SearchTerm { get; }

        public SearchEventArgs(string searchTerm)
        {
            SearchTerm = searchTerm;
        }
    }
}
