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
    public partial class TransportDashboard : Form
    {
        private int userId;

        public TransportDashboard(int userId = 64)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void TransportDashboard_Load(object sender, EventArgs e)
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
                    
                    string query = @"SELECT u.username, t.transport_type, t.vehicle_details
                                    FROM [User] u
                                    JOIN ServiceProvider sp ON u.user_id = sp.user_id
                                    JOIN TransportService t ON sp.provider_id = t.provider_id
                                    WHERE u.user_id = @UserId";
                    
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string username = reader.GetString(0);
                                string transportType = reader.GetString(1);
                                string vehicleDetails = reader.GetString(2);
                                
                                // Update UI with transport information
                                // For example, set a label with the transport type
                                // transportTypeLabel.Text = transportType;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading transport data: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        }        private void myreports_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();
            acceptrejectpanel.Controls.Clear();
            TransportReport report = new TransportReport(userId);
            report.Dock = DockStyle.Fill;
            mainpanel.Controls.Add(report);
        }

        private void home_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();
            acceptrejectpanel.Controls.Clear();

        }

        private void logout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
