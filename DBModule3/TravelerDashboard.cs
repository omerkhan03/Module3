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
    public partial class TravelerDashboard : Form
    {
        private int userId;

        public TravelerDashboard(int userId = 0)
        {
            InitializeComponent();
            this.userId = userId;

            // Initialize the dashboard panels
            workpanel.Controls.Clear();
            SearchBar searchBar = new SearchBar();
            searchBar.Dock = DockStyle.Fill;
            workpanel.Controls.Add(searchBar);

            rightpanel.Controls.Clear();
            Sidebar sidebar = new Sidebar();
            sidebar.Dock = DockStyle.Fill;
            rightpanel.Controls.Add(sidebar);
        }

        private void TravelerDashboard_Load(object sender, EventArgs e)
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

                    string query = @"SELECT u.username, t.first_name, t.last_name, t.nationality
                                    FROM [User] u
                                    JOIN Traveler t ON u.user_id = t.user_id
                                    WHERE u.user_id = @UserId";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string username = reader.GetString(0);
                                string firstName = reader.GetString(1);
                                string lastName = reader.GetString(2);
                                string nationality = reader.GetString(3);

                                // Update UI with traveler information
                                // Set window title to include traveler name
                                this.Text = $"Traveler Dashboard - {firstName} {lastName}";

                                // You can update other UI elements with user data here
                                // For example, you could set the text of a label or update the profile picture
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading traveler data: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void makereview_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();
            workpanel.Controls.Clear();
            rightpanel.Controls.Clear();

            ReviewMaker myControl = new ReviewMaker();
            myControl.Dock = DockStyle.Fill;
            mainpanel.Controls.Add(myControl);
        }

        private void home_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();

            workpanel.Controls.Clear();
            SearchBar searchBar = new SearchBar();
            searchBar.Dock = DockStyle.Top;
            workpanel.Controls.Add(searchBar);

            rightpanel.Controls.Clear();
            Sidebar sidebar = new Sidebar();
            sidebar.Dock = DockStyle.Fill;
            rightpanel.Controls.Add(sidebar);
        }

        private void logout_Click(object sender, EventArgs e)
        {
            // Instead of exiting the application, return to login screen
            Landing landing = new Landing();
            landing.Show();
            this.Close();
        }

        private void manageprofile_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();
            TravelerProfileView profilecontrol = new TravelerProfileView();
            profilecontrol.Dock = DockStyle.Fill;
            mainpanel.Controls.Add(profilecontrol);

            workpanel.Controls.Clear();
            rightpanel.Controls.Clear();
        }       
        private void bookings_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();
            rightpanel.Controls.Clear();
            workpanel.Controls.Clear();



            // Create UserBookings control to display the user's bookings in a DataGridView
            UserBookings userBookings = new UserBookings(userId);
            userBookings.Dock = DockStyle.Fill;
            mainpanel.Controls.Add(userBookings);

            CancelBooking cancelBooking = new CancelBooking();
            cancelBooking.Dock = DockStyle.Fill;
            workpanel.Controls.Add(cancelBooking);

        }

        private void travepasses_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();
            rightpanel.Controls.Clear();
            workpanel.Controls.Clear();
        }

        private void sataTextBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void wishlistbutton_Click(object sender, EventArgs e)
        {
            // Add wishlist functionality here
        }
    }
}
