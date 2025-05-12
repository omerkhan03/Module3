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
        private SearchBar searchBar;
        private Sidebar sidebar;
        private Button wishlistButton;

        public TravelerDashboard(int userId = 1)
        {
            InitializeComponent();
            this.userId = userId;

            // Add Wishlist Button
            //InitializeWishlistButton();

            // Initialize the dashboard panels
            InitializeWorkPanel();
            InitializeRightPanel();

            // Show all trips in main panel
            ShowAllTrips();
        }

        // private void InitializeWishlistButton()
        // {
        //     wishlistButton = new Button();
        //     wishlistButton.Text = "My Wishlist";
        //     wishlistButton.Font = new Font("Century Gothic", 10F, FontStyle.Bold);
        //     wishlistButton.Size = new Size(100, 30);
        //     wishlistButton.Location = new Point(mainpanel.Width - 120, 10);
        //     wishlistButton.BackColor = Color.FromArgb(20, 25, 72);
        //     wishlistButton.ForeColor = Color.White;
        //     wishlistButton.FlatStyle = FlatStyle.Flat;
        //     wishlistButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        //     wishlistButton.Click += WishlistButton_Click;
        //     this.Controls.Add(wishlistButton);
        // }

        // private void WishlistButton_Click(object sender, EventArgs e)
        // {
        //     mainpanel.Controls.Clear();
        //     WishlistPanel wishlistPanel = new WishlistPanel(userId);
        //     wishlistPanel.Dock = DockStyle.Fill;
        //     mainpanel.Controls.Add(wishlistPanel);
        // }

        private void InitializeWorkPanel()
        {
            workpanel.Controls.Clear();
            searchBar = new SearchBar(userId);
            searchBar.Dock = DockStyle.Fill;
            searchBar.SearchRequested += SearchBar_SearchRequested;
            workpanel.Controls.Add(searchBar);
        }

        private void InitializeRightPanel()
        {
            rightpanel.Controls.Clear();
            sidebar = new Sidebar();
            sidebar.Dock = DockStyle.Fill;
            sidebar.FiltersChanged += Sidebar_FiltersChanged;
            rightpanel.Controls.Add(sidebar);
        }

        private void SearchBar_SearchRequested(object sender, SearchEventArgs e)
        {
            // When search is requested, we get advanced filters from sidebar
            ApplySearchAndFilters(e.SearchTerm);
        }

        private void Sidebar_FiltersChanged(object sender, EventArgs e)
        {
            // When filters are changed, we apply them with the current search term
            string searchTerm = searchBar?.GetSearchTerm() ?? string.Empty;
            ApplySearchAndFilters(searchTerm);
        }

        private void ApplySearchAndFilters(string searchTerm)
        {
            // Get filter values from the sidebar
            decimal? minBudget = sidebar?.MinimumBudget;
            decimal? maxBudget = sidebar?.MaximumBudget;
            int? duration = sidebar?.Duration;
            int? groupSize = sidebar?.GroupSizeValue;

            // Apply filters
            mainpanel.Controls.Clear();
            AllTrips filteredTrips = new AllTrips(searchTerm, minBudget, maxBudget, duration, groupSize);
            filteredTrips.Dock = DockStyle.Fill;
            mainpanel.Controls.Add(filteredTrips);
        }

        private void ShowAllTrips()
        {
            mainpanel.Controls.Clear();
            AllTrips allTrips = new AllTrips();
            allTrips.Dock = DockStyle.Fill;
            mainpanel.Controls.Add(allTrips);
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

        }        private void makereview_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();
            workpanel.Controls.Clear();
            rightpanel.Controls.Clear();

            ReviewMaker myControl = new ReviewMaker(userId);
            myControl.Dock = DockStyle.Fill;
            mainpanel.Controls.Add(myControl);
        }
        
        private void home_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();

            // Show all trips in main panel
            ShowAllTrips();

            // Re-initialize the side panels
            InitializeWorkPanel();
            InitializeRightPanel();
        }

        private void logout_Click(object sender, EventArgs e)
        {
            // Instead of exiting the application, return to login screen
            Landing landing = new Landing();
            landing.Show();
            this.Close();
        }

        private void manageprofile_Click(object sender, EventArgs e)
        {            mainpanel.Controls.Clear();
            TravelerProfileView profilecontrol = new TravelerProfileView(userId);
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
            mainpanel.Controls.Add(userBookings);            PayBooking cancelBooking = new PayBooking(userId);
            cancelBooking.Dock = DockStyle.Fill;
            workpanel.Controls.Add(cancelBooking);
        }

        private void travepasses_Click(object sender, EventArgs e)
        {
            mainpanel.Controls.Clear();
            rightpanel.Controls.Clear();
            workpanel.Controls.Clear();

            // Create TravelPasses control to display the user's travel passes in a DataGridView
            TravelPasses travelPasses = new TravelPasses(userId);
            travelPasses.Dock = DockStyle.Fill;
            mainpanel.Controls.Add(travelPasses);
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

            mainpanel.Controls.Clear();
            WishlistPanel wishlistPanel = new WishlistPanel(userId);
            wishlistPanel.Dock = DockStyle.Fill;
            mainpanel.Controls.Add(wishlistPanel);
        }
    }
}
