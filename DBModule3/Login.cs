using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBModule3
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }        
        private void Login_Load(object sender, EventArgs e)
        {
            // We're removing the automatic redirect to Landing
            // Landing landing = new Landing();
            // landing.Show();
            // this.Hide();    
        }
        
        private void loginButton_Click(object sender, EventArgs e)
        {
            // Get username and password from text boxes
            string username = sataTextBox1.Texts.Trim();
            string password = sataTextBox2.Texts.Trim();
            
            // Validate input
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", 
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Authentication logic
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    
                    string query = "SELECT user_id, username, role FROM [User] WHERE username = @Username AND password = @Password AND is_active = 1";
                    
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password); // In a real app, use proper password hashing
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int userId = reader.GetInt32(0);
                                string role = reader.GetString(2);
                                
                                // Open appropriate dashboard based on role
                                OpenDashboard(userId, role);
                            }
                            else
                            {
                                MessageBox.Show("Invalid username or password.", 
                                    "Authentication Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void OpenDashboard(int userId, string role)
        {
            Form dashboard = null;
              switch (role.ToLower())
            {
                case "admin":
                    dashboard = new AdminDashboard(userId);
                    break;
                    
                case "traveler":
                    Console.WriteLine($"Traveler role detected with userId: {userId}");
                    dashboard = new TravelerDashboard(userId);
                    break;
                    
                case "tour_operator":
                    dashboard = new TourOperatorDashboard(userId);
                    break;
                    
                case "service_provider":
                    // We need to determine the specific service provider type
                    OpenServiceProviderDashboard(userId);
                    return;
                    
                default:
                    MessageBox.Show($"Unknown role: {role}. Please contact administrator.", 
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }
            
            if (dashboard != null)
            {
                dashboard.Show();
                this.Hide();
            }
        }
          private void OpenServiceProviderDashboard(int userId)
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    
                    // First, get the provider_id
                    string providerQuery = "SELECT provider_id, provider_type FROM ServiceProvider WHERE user_id = @UserId";
                    
                    using (SqlCommand command = new SqlCommand(providerQuery, connection))
                    {
                        command.Parameters.AddWithValue("@UserId", userId);
                        
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int providerId = reader.GetInt32(0);
                                string providerType = reader.GetString(1);
                                
                                Form dashboard = null;
                                
                                // Open the appropriate dashboard based on provider type
                                switch (providerType.ToLower())
                                {
                                    case "hotel":
                                        dashboard = new HotelDashboard(userId);
                                        break;
                                        
                                    case "transport":
                                        dashboard = new TransportDashboard(userId);
                                        break;
                                        
                                    case "guide":
                                        dashboard = new GuideDashboard(userId);
                                        break;
                                        
                                    default:
                                        MessageBox.Show($"Unknown provider type: {providerType}.", 
                                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                }
                                
                                if (dashboard != null)
                                {
                                    dashboard.Show();
                                    this.Hide();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Service provider information not found.", 
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void signuplink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Landing landing = new Landing();
            landing.Show();
            this.Hide();
        }

        private void homebutton_Click(object sender, EventArgs e)
        {
            Landing landing = new Landing();
            landing.Show();
            this.Hide();
        }
    }
}
