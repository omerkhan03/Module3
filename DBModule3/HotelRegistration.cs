using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DBModule3
{    
    public partial class HotelRegistration : Form
    {
        // Property to store the provider ID passed from ServiceProviderSignUp
        public int ProviderId { get; set; }
        
        // Property to store the hotel name
        public string HotelName { get; set; } = "Default Hotel";
        
        public HotelRegistration()
        {
            InitializeComponent();
        }

        private void title_Click(object sender, EventArgs e)
        {
        }
        
        private void title_Click_1(object sender, EventArgs e)
        {

        }

        private void back_Click(object sender, EventArgs e)
        {
            Landing landing = new Landing();
            landing.Show();
            this.Hide();
        }        
        
        private void register_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(locationTextBox.Text) ||
            !int.TryParse(totalRoomsTextBox.Text, out int totalRooms))
            {
                MessageBox.Show("Please fill all fields correctly. Total rooms must be a number.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (ProviderId <= 0)
            {
                MessageBox.Show("Invalid provider ID. Please start from the service provider registration page.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            try
            {                
                // Insert hotel-specific data into Hotel table
                RegisterHotel();
                MessageBox.Show("Hotel registered successfully!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                HotelDashboard hotelDash = new HotelDashboard(GetUserIdForProvider(ProviderId));
                hotelDash.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during registration: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void RegisterHotel()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                try
                {
                    connection.Open();
                    
                    string insertHotelQuery = @"INSERT INTO Hotel (provider_id, hotel_name, location, total_rooms) 
                                               VALUES (@ProviderId, @HotelName, @Location, @TotalRooms)";
                    using (SqlCommand cmd = new SqlCommand(insertHotelQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@ProviderId", ProviderId);
                        cmd.Parameters.AddWithValue("@HotelName", HotelName); // Use the hotel name property
                        cmd.Parameters.AddWithValue("@Location", locationTextBox.Text);
                        cmd.Parameters.AddWithValue("@TotalRooms", Convert.ToInt32(totalRoomsTextBox.Text));
                        
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error registering hotel: {ex.Message}", ex);
                }
            }
        }

        private int GetUserIdForProvider(int providerId)
        {
            int userId = 0;
            
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                try
                {
                    connection.Open();
                    
                    string query = "SELECT user_id FROM ServiceProvider WHERE provider_id = @ProviderId";
                    
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ProviderId", providerId);
                        
                        object result = cmd.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            userId = Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error retrieving user ID: {ex.Message}", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            
            return userId;
        }
    }
}