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
    public partial class AdminDashboard : Form
    {
        private int userId;

        public AdminDashboard(int userId = 0)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void AdminDashboard_Load(object sender, EventArgs e)
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
                    
                    string query = @"SELECT u.username, u.email
                                    FROM [User] u
                                    JOIN Admin a ON u.user_id = a.user_id
                                    WHERE u.user_id = @UserId";
                    
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string username = reader.GetString(0);
                                string email = reader.GetString(1);
                                
                                // Update UI with admin information
                                // For example, set a label with the admin username
                                // adminUsernameLabel.Text = username;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading admin data: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void logout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bookings_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();
        }

        private void mngreviews_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();
        }

        private void mngbookings_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();
        }

        private void mngusers_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();
        }

        private void analytics_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();
        }

        private void home_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();
        }
    }
}
