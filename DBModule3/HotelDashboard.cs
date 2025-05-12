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
    public partial class HotelDashboard : Form
    {
        private int userId;

        public HotelDashboard(int userId = 63)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void HotelDashboard_Load(object sender, EventArgs e)
        {
            if (userId > 0)
            {
                LoadUserData();
            }
        }
        
        private void LoadUserData()
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    
                    string query = @"SELECT u.username, h.hotel_name, h.location
                                    FROM [User] u
                                    JOIN ServiceProvider sp ON u.user_id = sp.user_id
                                    JOIN Hotel h ON sp.provider_id = h.provider_id
                                    WHERE u.user_id = @UserId";
                    
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string username = reader.GetString(0);
                                string hotelName = reader.GetString(1);
                                string location = reader.GetString(2);
                                
                                // Update UI with hotel information
                                // For example, set a label with the hotel name
                                // hotelNameLabel.Text = hotelName;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading hotel data: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void logout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void home_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();

        }

        private void assignments_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();
            acceptrejectpanel.Controls.Clear();

            ServiceProviderAssignmentBar assignmentBar = new ServiceProviderAssignmentBar(userId);
            assignmentBar.Dock = DockStyle.Fill; // <- THIS is what you need
            mainpanel.Controls.Add(assignmentBar);
        }

       private void reviews_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();
            acceptrejectpanel.Controls.Clear();

            ServiceProviderReviews reviewsControl = new ServiceProviderReviews(userId);
            reviewsControl.Dock = DockStyle.Fill;
            mainpanel.Controls.Add(reviewsControl);
        }

        private void myreports_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();

        }

        private void acceptrejectpanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
