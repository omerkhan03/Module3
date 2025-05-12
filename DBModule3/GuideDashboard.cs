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
    public partial class GuideDashboard : Form
    {
        private int userId;

        public GuideDashboard(int userId = 65)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void GuideDashboard_Load(object sender, EventArgs e)
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
                    
                    string query = @"SELECT u.username, g.guide_name, g.specialization
                                    FROM [User] u
                                    JOIN ServiceProvider sp ON u.user_id = sp.user_id
                                    JOIN Guide g ON sp.provider_id = g.provider_id
                                    WHERE u.user_id = @UserId";
                    
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string username = reader.GetString(0);
                                string guideName = reader.GetString(1);
                                string specialization = reader.GetString(2);
                                
                                // Update UI with guide information
                                // For example, set a label with the guide name and specialization
                                // guideNameLabel.Text = guideName;
                                // specializationLabel.Text = specialization;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading guide data: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void logout_Click(object sender, EventArgs e)
        {
            Landing landing = new Landing();
            landing.Show();
            this.Close();
        }

        private void assignments_Click(object sender, EventArgs e)
        {            mainpanel.Controls.Clear();
            acceptrejectpanel.Controls.Clear();

            ServiceProviderAssignmentBar assignmentBar = new ServiceProviderAssignmentBar(userId);
            assignmentBar.Dock = DockStyle.Fill;
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
            GuideReport report = new GuideReport(userId);
            report.Dock = DockStyle.Fill;
            mainpanel.Controls.Add(report);
        }

        private void home_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();

        }

        private void GuideDashboard_Load_1(object sender, EventArgs e)
        {

        }
    }
}
