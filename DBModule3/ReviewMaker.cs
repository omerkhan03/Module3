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
    public partial class ReviewMaker : UserControl
    {
        private int userId;
        private enum ReviewType { Trip, Provider, Both }
        private ReviewType currentReviewType = ReviewType.Both;

        public ReviewMaker(int userId = 1)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            // Not needed
        }        
        
        private void ReviewMaker_Load(object sender, EventArgs e)
        {
            // Populate rating dropdown with values 1-5
            for (int i = 1; i <= 5; i++)
            {
                categoryType.Items.Add(i.ToString());
            }
            
            // Default to 5 stars
            categoryType.SelectedIndex = 4;
            
            // Add radio buttons for review type
            CreateReviewTypeOptions();
            
            // Load trips the user has booked to allow them to select one
            LoadUserTrips();
        }
        
        private void CreateReviewTypeOptions()
        {
            // Create panel to hold radio buttons
            Panel reviewTypePanel = new Panel();
            reviewTypePanel.Dock = DockStyle.Top;
            reviewTypePanel.Height = 40;
            reviewTypePanel.Padding = new Padding(5);
            
            // Create label
            Label reviewTypeLabel = new Label();
            reviewTypeLabel.Text = "Review Type:";
            reviewTypeLabel.AutoSize = true;
            reviewTypeLabel.Location = new Point(5, 6);
            reviewTypePanel.Controls.Add(reviewTypeLabel);
            
            // Create radio buttons
            RadioButton rbTrip = new RadioButton();
            rbTrip.Text = "Trip Only";
            rbTrip.AutoSize = true;
            rbTrip.Location = new Point(100, 6);
            rbTrip.Tag = ReviewType.Trip;
            rbTrip.CheckedChanged += ReviewType_CheckedChanged;
            
            RadioButton rbProvider = new RadioButton();
            rbProvider.Text = "Provider Only";
            rbProvider.AutoSize = true;
            rbProvider.Location = new Point(200, 6);
            rbProvider.Tag = ReviewType.Provider;
            rbProvider.CheckedChanged += ReviewType_CheckedChanged;
              RadioButton rbBoth = new RadioButton();
            rbBoth.Text = "Both";
            rbBoth.AutoSize = true;
            rbBoth.Location = new Point(320, 6);
            rbBoth.Tag = ReviewType.Both;
            rbBoth.CheckedChanged += ReviewType_CheckedChanged;
            rbBoth.Checked = true; // Default
            
            // Add radio buttons to panel
            reviewTypePanel.Controls.Add(rbTrip);
            reviewTypePanel.Controls.Add(rbProvider);
            reviewTypePanel.Controls.Add(rbBoth);
            
            // Add panel to form (assuming this control has a Controls collection)
            Controls.Add(reviewTypePanel);
        }
        
        private void ReviewType_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb != null && rb.Checked)
            {
                currentReviewType = (ReviewType)rb.Tag;
                UpdateUIBasedOnReviewType();
            }
        }
        
        private void UpdateUIBasedOnReviewType()
        {
            // Show/hide UI elements based on review type
            switch (currentReviewType)
            {
                case ReviewType.Trip:
                    // Show trip ID field, hide provider ID field
                    label.Text = "Trip ID:";
                    destinationtext.Enabled = true;
                    label1.Visible = false;
                    textBox2.Visible = false;
                    break;
                    
                case ReviewType.Provider:
                    // Hide trip ID field, show provider ID field
                    label.Text = "Provider ID:";
                    destinationtext.Enabled = true;
                    label1.Visible = false;
                    textBox2.Visible = false;
                    
                    // Load all providers (not just ones from a specific trip)
                    LoadAllProviders();
                    break;
                      case ReviewType.Both:
                    // Show both fields
                    label.Text = "Trip ID:";
                    label.Visible = true;
                    destinationtext.Visible = true;
                    destinationtext.Enabled = true;
                    LoadUserTrips(); // Reload trip options
                    
                    label1.Text = "Provider ID:";
                    label1.Visible = true;
                    textBox2.Visible = true;
                    textBox2.Enabled = true;
                    break;
            }
        }
        
        private void LoadUserTrips()
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    
                    // Get trips booked by this user
                    string query = @"SELECT b.booking_id, t.trip_id, t.trip_name 
                                    FROM Booking b
                                    JOIN Trip t ON b.trip_id = t.trip_id
                                    WHERE b.traveler_id = @UserId 
                                    ORDER BY b.booking_date DESC";
                    
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Store trip IDs in the destinationtext dropdown
                            destinationtext.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                            destinationtext.AutoCompleteSource = AutoCompleteSource.CustomSource;
                            AutoCompleteStringCollection tripIds = new AutoCompleteStringCollection();
                            
                            while (reader.Read())
                            {
                                string tripId = reader["trip_id"].ToString();
                                tripIds.Add(tripId);
                            }
                            
                            destinationtext.AutoCompleteCustomSource = tripIds;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading trips: {ex.Message}", 
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }          private void LoadServiceProviders(int tripId)
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    
                    // Get service providers associated with this trip
                    string query = @"
                    SELECT sp.provider_id, 
                        CASE 
                            WHEN h.hotel_id IS NOT NULL THEN 'Hotel: ' + h.hotel_name
                            WHEN ts.transport_id IS NOT NULL THEN 'Transport: ' + ts.transport_type
                            WHEN g.guide_id IS NOT NULL THEN 'Guide: ' + g.guide_name
                            ELSE 'Provider: ' + CAST(sp.provider_id AS VARCHAR)
                        END AS provider_name
                    FROM Trip t
                    JOIN ServiceAssignment sa ON sa.booking_id IN (SELECT booking_id FROM Booking WHERE trip_id = @TripId)
                    JOIN ServiceProvider sp ON sa.provider_id = sp.provider_id
                    LEFT JOIN Hotel h ON sp.provider_id = h.provider_id
                    LEFT JOIN TransportService ts ON sp.provider_id = ts.provider_id
                    LEFT JOIN Guide g ON sp.provider_id = g.provider_id
                    WHERE t.trip_id = @TripId";
                    
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@TripId", tripId);
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Use a HashSet to track unique provider IDs
                            HashSet<string> uniqueProviderIds = new HashSet<string>();
                            AutoCompleteStringCollection providerIds = new AutoCompleteStringCollection();
                            
                            while (reader.Read())
                            {
                                string providerId = reader["provider_id"].ToString();
                                
                                if (!string.IsNullOrEmpty(providerId) && !uniqueProviderIds.Contains(providerId))
                                {
                                    uniqueProviderIds.Add(providerId);
                                    providerIds.Add(providerId);
                                }
                            }
                            
                            // If any providers were found, enable autocomplete
                            if (uniqueProviderIds.Count > 0)
                            {
                                textBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;
                                textBox2.AutoCompleteCustomSource = providerIds;
                            }
                            else
                            {
                                // If no providers found in service assignments, try direct associations from trip
                                LoadTripDirectProviders(tripId);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading service providers: {ex.Message}", 
                    "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
          private void LoadTripDirectProviders(int tripId)
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    
                    // Some trips might have direct provider associations that aren't in the ServiceAssignment table
                    string query = @"
                        SELECT DISTINCT sp.provider_id,
                            CASE 
                                WHEN h.hotel_id IS NOT NULL THEN 'Hotel: ' + h.hotel_name
                                WHEN ts.transport_id IS NOT NULL THEN 'Transport: ' + ts.transport_type
                                WHEN g.guide_id IS NOT NULL THEN 'Guide: ' + g.guide_name
                                ELSE 'Provider: ' + CAST(sp.provider_id AS VARCHAR)
                            END AS provider_name
                        FROM Trip t
                        -- Join with service providers that could be associated with the trip
                        JOIN ServiceProvider sp ON sp.provider_type IN ('Hotel', 'Transport', 'Guide')  
                        LEFT JOIN Hotel h ON sp.provider_id = h.provider_id
                        LEFT JOIN TransportService ts ON sp.provider_id = ts.provider_id
                        LEFT JOIN Guide g ON sp.provider_id = g.provider_id
                        WHERE t.trip_id = @TripId";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@TripId", tripId);
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            HashSet<string> uniqueIds = new HashSet<string>(); // Use a HashSet to track unique IDs
                            AutoCompleteStringCollection providerIds = new AutoCompleteStringCollection();
                            
                            while (reader.Read())
                            {
                                string providerId = reader["provider_id"].ToString();
                                
                                if (!string.IsNullOrEmpty(providerId) && !uniqueIds.Contains(providerId))
                                {
                                    uniqueIds.Add(providerId);
                                    providerIds.Add(providerId);
                                }
                            }
                            
                            if (providerIds.Count > 0)
                            {
                                textBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                                textBox2.AutoCompleteSource = AutoCompleteSource.CustomSource;
                                textBox2.AutoCompleteCustomSource = providerIds;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Just log the error without showing a message box (we already showed one in the calling method)
                Console.WriteLine($"Error loading direct trip providers: {ex.Message}");
            }
        }private void LoadAllProviders()
    {
        try
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                
                // Query to get all service providers from different tables
                string query = @"
                    -- Get all hotels
                    SELECT h.hotel_id AS provider_id, 'Hotel: ' + h.hotel_name AS provider_name
                    FROM Hotel h
                    JOIN ServiceProvider sp ON h.provider_id = sp.provider_id
                    UNION ALL
                    -- Get all transport companies
                    SELECT ts.transport_id AS provider_id, 'Transport: ' + ts.transport_type AS provider_name
                    FROM TransportService ts
                    JOIN ServiceProvider sp ON ts.provider_id = sp.provider_id
                    UNION ALL
                    -- Get all guides
                    SELECT g.guide_id AS provider_id, 'Guide: ' + g.guide_name AS provider_name
                    FROM Guide g
                    JOIN ServiceProvider sp ON g.provider_id = sp.provider_id";
                
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Store provider IDs in the destinationtext dropdown (repurposing the trip field)
                        destinationtext.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                        destinationtext.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        AutoCompleteStringCollection providerIds = new AutoCompleteStringCollection();
                        
                        HashSet<string> addedIds = new HashSet<string>(); // Track already added IDs
                        
                        while (reader.Read())
                        {
                            string providerId = reader["provider_id"].ToString();
                            string providerName = reader["provider_name"].ToString();
                            
                            if (!string.IsNullOrEmpty(providerId) && !addedIds.Contains(providerId))
                            {
                                addedIds.Add(providerId); // Mark this ID as added
                                providerIds.Add(providerId);
                            }
                        }
                        
                        destinationtext.AutoCompleteCustomSource = providerIds;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading service providers: {ex.Message}", 
                "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }private void reviewbutton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input data
                if (string.IsNullOrWhiteSpace(destinationtext.Text))
                {
                    string fieldName = currentReviewType == ReviewType.Provider ? "Provider ID" : "Trip ID";
                    MessageBox.Show($"Please enter a {fieldName}.", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (currentReviewType == ReviewType.Both && string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    MessageBox.Show("Please enter a Provider ID.", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (categoryType.SelectedIndex == -1)
                {
                    MessageBox.Show("Please select a rating.", "Validation Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // Get rating and comment
                int rating = int.Parse(categoryType.SelectedItem.ToString());
                string comment = textBox1.Text.Trim();
                
                // Get ids based on review type
                int? tripId = null;
                int? providerId = null;
                
                switch (currentReviewType)
                {
                    case ReviewType.Trip:
                        if (!int.TryParse(destinationtext.Text, out int tId))
                        {
                            MessageBox.Show("Trip ID must be a number.", "Validation Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        tripId = tId;
                        break;
                        
                    case ReviewType.Provider:
                        if (!int.TryParse(destinationtext.Text, out int pId))
                        {
                            MessageBox.Show("Provider ID must be a number.", "Validation Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        providerId = pId;
                        break;
                        
                    case ReviewType.Both:
                        if (!int.TryParse(destinationtext.Text, out int tId2))
                        {
                            MessageBox.Show("Trip ID must be a number.", "Validation Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        tripId = tId2;
                        
                        if (!int.TryParse(textBox2.Text, out int pId2))
                        {
                            MessageBox.Show("Provider ID must be a number.", "Validation Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        providerId = pId2;
                        break;
                }
                  // Insert review into database
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    
                    // Validate that we're complying with the database constraint
                    // (trip_id IS NOT NULL AND provider_id IS NULL) OR
                    // (trip_id IS NULL AND provider_id IS NOT NULL) OR
                    // (trip_id IS NOT NULL AND provider_id IS NOT NULL)
                    if (tripId == null && providerId == null)
                    {
                        MessageBox.Show("At least one of Trip ID or Provider ID must be provided.",
                            "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                    
                    string query = @"INSERT INTO Review (traveler_id, trip_id, provider_id, rating, comment, review_date)
                                    VALUES (@TravelerId, @TripId, @ProviderId, @Rating, @Comment, @ReviewDate)";
                    
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@TravelerId", userId);
                        cmd.Parameters.AddWithValue("@TripId", tripId ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ProviderId", providerId ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Rating", rating);
                        cmd.Parameters.AddWithValue("@Comment", comment);
                        cmd.Parameters.AddWithValue("@ReviewDate", DateTime.Now);
                        
                        try
                        {
                            int rowsAffected = cmd.ExecuteNonQuery();
                            
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Review submitted successfully!", "Success", 
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                                
                                // Clear form for next review
                                destinationtext.Text = string.Empty;
                                textBox2.Text = string.Empty;
                                categoryType.SelectedIndex = 4; // Reset to 5 stars
                                textBox1.Text = string.Empty;
                            }
                            else
                            {
                                MessageBox.Show("Failed to submit review. Please try again.", 
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        catch (SqlException sqlEx)
                        {
                            if (sqlEx.Message.Contains("CK_Review_Target"))
                            {
                                MessageBox.Show("Review must have either a trip, a provider, or both. Cannot leave both empty.",
                                    "Database Constraint Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                throw; // Re-throw to be caught by outer catch
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
          // Add event handler for when trip ID is selected to load corresponding service providers
        private void destinationtext_Leave(object sender, EventArgs e)
        {
            if (currentReviewType != ReviewType.Provider && int.TryParse(destinationtext.Text, out int tripId))
            {
                LoadServiceProviders(tripId);
            }
        }
    }
}
