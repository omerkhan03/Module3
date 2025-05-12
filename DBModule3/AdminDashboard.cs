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
        private TabControl reportsTabControl;

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
            ReviewManager reviewManager = new ReviewManager();
            reviewManager.Dock = DockStyle.Fill;
            mainpanel.Controls.Add(reviewManager);
            
        }

        private void mngbookings_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();
        }

        private void mngusers_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();
            UserManager userManager = new UserManager();
            userManager.Dock = DockStyle.Fill;
            mainpanel.Controls.Add(userManager);

        }

        private void analytics_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();
            
            if (reportsTabControl == null)
            {
                InitializeReportsTabControl();
            }
            
            mainpanel.Controls.Add(reportsTabControl);
        }

        private void home_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();
        }

        private void mngtrips_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();
            TripCategoryManager tripCategoryManager = new TripCategoryManager();
            tripCategoryManager.Dock = DockStyle.Fill;
            mainpanel.Controls.Add(tripCategoryManager);
        }

        private void InitializeReportsTabControl()
        {
            reportsTabControl = new TabControl
            {
                Dock = DockStyle.Fill,
                Font = new Font("Century Gothic", 10)
            };            // Create tabs for each report type
            var bookingRevenueTab = new TabPage("Booking & Revenue");
            bookingRevenueTab.Controls.Add(new BookingRevenueReport { Dock = DockStyle.Fill });

            var travelerDemographicsTab = new TabPage("Traveler Demographics");
            travelerDemographicsTab.Controls.Add(new TravelerDemographicsReport { Dock = DockStyle.Fill });

            var destinationAnalyticsTab = new TabPage("Destination Analytics");
            destinationAnalyticsTab.Controls.Add(new DestinationAnalyticsReport { Dock = DockStyle.Fill });

            var bookingAnalysisTab = new TabPage("Abandoned Bookings");
            bookingAnalysisTab.Controls.Add(new BookingAnalysisReport { Dock = DockStyle.Fill });

            var platformGrowthTab = new TabPage("Platform Growth");
            platformGrowthTab.Controls.Add(new PlatformGrowthReport { Dock = DockStyle.Fill });

            var paymentAnalyticsTab = new TabPage("Payment Analytics");
            paymentAnalyticsTab.Controls.Add(new PaymentAnalyticsReport { Dock = DockStyle.Fill });

            // Add tabs to the control
            reportsTabControl.TabPages.AddRange(new[] {
                bookingRevenueTab,
                travelerDemographicsTab,
                destinationAnalyticsTab,
                bookingAnalysisTab,
                platformGrowthTab,
                paymentAnalyticsTab
            });
        }
    }
}
