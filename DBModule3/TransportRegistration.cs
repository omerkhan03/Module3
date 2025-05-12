using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DBModule3
{    
    public partial class TransportRegistration : Form
    {
        // Property to store the provider ID passed from ServiceProviderSignUp
        public int ProviderId { get; set; }
        
        public TransportRegistration()
        {
            InitializeComponent();
            transportTypeComboBox.Items.AddRange(new string[] { "Car", "Bus", "Van", "Other" });
        }

        private void title_Click(object sender, EventArgs e)
        {
        }        
        
        private void registerbutton_Click(object sender, EventArgs e)
        {
            if (transportTypeComboBox.SelectedItem == null ||
                string.IsNullOrEmpty(vehicleDetailsTextBox.Text) ||
                !int.TryParse(capacityTextBox.Text, out int capacity))
            {
                MessageBox.Show("Please fill all fields correctly. Capacity must be a number.",
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
                // Insert transport-specific data into TransportService table
                RegisterTransport();                MessageBox.Show("Registration Successful");
                TransportDashboard transportDash = new TransportDashboard(GetUserIdForProvider(ProviderId));
                transportDash.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during registration: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void RegisterTransport()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                try
                {
                    connection.Open();
                    
                    string insertTransportQuery = @"INSERT INTO TransportService (provider_id, transport_type, vehicle_details, capacity) 
                                                  VALUES (@ProviderId, @TransportType, @VehicleDetails, @Capacity)";
                    
                    using (SqlCommand cmd = new SqlCommand(insertTransportQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@ProviderId", ProviderId);
                        cmd.Parameters.AddWithValue("@TransportType", transportTypeComboBox.SelectedItem.ToString());
                        cmd.Parameters.AddWithValue("@VehicleDetails", vehicleDetailsTextBox.Text);
                        cmd.Parameters.AddWithValue("@Capacity", Convert.ToInt32(capacityTextBox.Text));
                        
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Error registering transport service: {ex.Message}", ex);
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

        private void back_Click(object sender, EventArgs e)
        {
            Landing landing = new Landing();
            landing.Show();
            this.Hide();
        }
    }
}