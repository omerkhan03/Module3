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
    public partial class TourOperatorDashboard : Form
    {
        private int userId;

        public TourOperatorDashboard(int userId = 0)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void TourOperatorDashboard_Load(object sender, EventArgs e)
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
                    
                    string query = @"SELECT u.username, to2.company_name 
                                    FROM [User] u
                                    JOIN TourOperator to2 ON u.user_id = to2.user_id
                                    WHERE u.user_id = @UserId";
                    
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string username = reader.GetString(0);
                                string companyName = reader.GetString(1);
                                
                                // Update UI with user information
                                // For example, set a label with the company name
                                // companyNameLabel.Text = companyName;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading user data: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void home_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();
        }

        private void createtrips_Click(object sender, EventArgs e)
        {
            TripCreator tripcreator = new TripCreator();
            tripcreator.Dock = DockStyle.Fill;
            mainpanel.Controls.Clear();
            mainpanel.Controls.Add(tripcreator);
        }

        private void logout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
