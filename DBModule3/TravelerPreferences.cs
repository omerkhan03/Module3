using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DBModule3
{
    public partial class TravelerPreferences : Form
    {
        public int TravelerId { get; set; }
        public int UserId { get; set; }        public TravelerPreferences(int userId = 0)
        {
            InitializeComponent();
            
            // Set user ID
            this.UserId = userId;
            
            // Add some common travel preferences to start with
            preferencesChecklist.Items.AddRange(new string[] { 
                "Adventure Travel",
                "Cultural Experience",
                "Beach Vacation",
                "Mountain Hiking",
                "City Tours",
                "Historical Sites",
                "Food & Cuisine",
                "Luxury Travel",
                "Budget Travel",
                "Eco Tourism",
                "Photography Tours",
                "Shopping Trips",
                "Religious Tourism",
                "Sports & Recreation",
                "Wellness & Spa"
            });
        }

        private void title_Click(object sender, EventArgs e)
        {
        }        private void TravelerPreferences_Load(object sender, EventArgs e)
        {
            // If the user ID is set but traveler ID is not, try to get the traveler ID
            if (UserId > 0 && TravelerId <= 0)
            {
                TravelerId = GetTravelerId(UserId);
                
                if (TravelerId <= 0)
                {
                    MessageBox.Show("Could not find traveler information. Some features may be limited.",
                        "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }

            // Load existing preferences
            LoadExistingPreferences();
        }

        private void preferencesChecklist_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void registerbutton_Click(object sender, EventArgs e)
        {
            if (preferencesChecklist.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please select at least one preference.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            try
            {
                // Save preferences to database
                SavePreferencesToDatabase();
                
                MessageBox.Show("Preferences saved successfully!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TravelerDashboard travelerDashboard = new TravelerDashboard(UserId);
                travelerDashboard.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving preferences: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }        private void SavePreferencesToDatabase()
        {
            // If TravelerId is not set, we need to get it
            if (TravelerId <= 0 && UserId > 0)
            {
                TravelerId = GetTravelerId(UserId);
            }
            
            if (TravelerId <= 0)
            {
                throw new Exception("Cannot identify traveler to save preferences.");
            }
            
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                
                // Start a transaction for data consistency
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // First, delete existing preferences for this traveler
                        DeleteExistingPreferences(TravelerId, connection, transaction);
                        
                        // For each checked preference
                        foreach (var item in preferencesChecklist.CheckedItems)
                        {
                            string preferenceName = item.ToString();
                            int preferenceId = EnsurePreferenceExists(preferenceName, connection, transaction);
                            
                            // Add to TravelerPreference if not already exists
                            if (!TravelerPreferenceExists(TravelerId, preferenceId, connection, transaction))
                            {
                                // Insert into TravelerPreference
                                string insertPreferenceQuery = @"
                                    INSERT INTO TravelerPreference (traveler_id, preference_id)
                                    VALUES (@TravelerId, @PreferenceId)";
                                
                                using (SqlCommand cmd = new SqlCommand(insertPreferenceQuery, connection, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@TravelerId", TravelerId);
                                    cmd.Parameters.AddWithValue("@PreferenceId", preferenceId);
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        
                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        // Rollback in case of any error
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
        
        private int GetTravelerId(int userId)
        {
            int travelerId = 0;
            
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                connection.Open();
                
                string query = "SELECT traveler_id FROM Traveler WHERE user_id = @UserId";
                
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    object result = cmd.ExecuteScalar();
                    
                    if (result != null && result != DBNull.Value)
                    {
                        travelerId = Convert.ToInt32(result);
                    }
                }
            }
            
            return travelerId;
        }
        
        private int EnsurePreferenceExists(string preferenceName, SqlConnection connection, SqlTransaction transaction)
        {
            // Check if preference already exists
            string checkQuery = "SELECT preference_id FROM Preference WHERE preference_name = @PreferenceName";
            
            using (SqlCommand cmd = new SqlCommand(checkQuery, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@PreferenceName", preferenceName);
                object result = cmd.ExecuteScalar();
                
                if (result != null && result != DBNull.Value)
                {
                    return Convert.ToInt32(result);
                }
            }
            
            // Preference does not exist, insert it
            string insertQuery = @"
                INSERT INTO Preference (preference_name, description)
                VALUES (@PreferenceName, @Description);
                SELECT SCOPE_IDENTITY();";
            
            using (SqlCommand cmd = new SqlCommand(insertQuery, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@PreferenceName", preferenceName);
                cmd.Parameters.AddWithValue("@Description", $"User preference for {preferenceName}");
                
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
        
        private bool TravelerPreferenceExists(int travelerId, int preferenceId, SqlConnection connection, SqlTransaction transaction)
        {
            string query = @"
                SELECT COUNT(1) 
                FROM TravelerPreference 
                WHERE traveler_id = @TravelerId AND preference_id = @PreferenceId";
            
            using (SqlCommand cmd = new SqlCommand(query, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@TravelerId", travelerId);
                cmd.Parameters.AddWithValue("@PreferenceId", preferenceId);
                
                return (int)cmd.ExecuteScalar() > 0;
            }
        }

        private void LoadExistingPreferences()
        {
            if (TravelerId <= 0)
                return;
                
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    
                    string query = @"
                        SELECT p.preference_name
                        FROM Preference p
                        JOIN TravelerPreference tp ON p.preference_id = tp.preference_id
                        WHERE tp.traveler_id = @TravelerId";
                    
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@TravelerId", TravelerId);
                        
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string preferenceName = reader.GetString(0);
                                
                                // Check the preference in the list if it exists
                                for (int i = 0; i < preferencesChecklist.Items.Count; i++)
                                {
                                    if (preferencesChecklist.Items[i].ToString() == preferenceName)
                                    {
                                        preferencesChecklist.SetItemChecked(i, true);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading existing preferences: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteExistingPreferences(int travelerId, SqlConnection connection, SqlTransaction transaction)
        {
            string deleteQuery = "DELETE FROM TravelerPreference WHERE traveler_id = @TravelerId";
            
            using (SqlCommand cmd = new SqlCommand(deleteQuery, connection, transaction))
            {
                cmd.Parameters.AddWithValue("@TravelerId", travelerId);
                cmd.ExecuteNonQuery();
            }
        }

        private void sataButton1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(customPreferenceTextBox.Text))
            {
                preferencesChecklist.Items.Add(customPreferenceTextBox.Text);
                customPreferenceTextBox.Clear();
            }
        }

        private void back_Click(object sender, EventArgs e)
        {
            Landing landing = new Landing();
            landing.Show();
            this.Hide();
        }
    }
}