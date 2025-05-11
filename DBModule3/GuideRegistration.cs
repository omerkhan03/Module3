using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DBModule3
{    public partial class GuideRegistration : Form
    {
        // Property to store the provider ID passed from ServiceProviderSignUp
        public int ProviderId { get; set; }
        
        public GuideRegistration()
        {
            InitializeComponent();
            // Add common languages to the checklist
            languageChecklist.Items.AddRange(new string[] { 
                "English", "Spanish", "French", "German", 
                "Italian", "Chinese", "Japanese", "Arabic" 
            });
        }

        private void title_Click(object sender, EventArgs e)
        {
        }

        private void title_Click_1(object sender, EventArgs e)
        {

        }

        private void back_Click(object sender, EventArgs e)
        {
            Landing landing = new Landing();
            landing.Show();
            this.Hide();
        }        private void register_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(guideNameTextBox.Text) ||
             string.IsNullOrEmpty(specializationTextBox.Text) ||
            languageChecklist.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please fill all fields and select at least one language.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            if (ProviderId <= 0)
            {
                MessageBox.Show("Invalid provider ID. Please start from the service provider registration page.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            try
            {
                // Insert guide-specific data into Guide table and languages
                RegisterGuide();
                
                MessageBox.Show("Registration Successful");
                //GuideDashboard guideDash = new GuideDashboard();
                //guideDash.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during registration: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void RegisterGuide()
        {
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                try
                {
                    connection.Open();
                    
                    // Begin a transaction
                    SqlTransaction transaction = connection.BeginTransaction();
                    
                    try
                    {
                        // 1. Insert into Guide table
                        string insertGuideQuery = @"INSERT INTO Guide (provider_id, guide_name, specialization) 
                                                  VALUES (@ProviderId, @GuideName, @Specialization);
                                                  SELECT SCOPE_IDENTITY();";
                        
                        int guideId;
                        using (SqlCommand cmd = new SqlCommand(insertGuideQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@ProviderId", ProviderId);
                            cmd.Parameters.AddWithValue("@GuideName", guideNameTextBox.Text);
                            cmd.Parameters.AddWithValue("@Specialization", specializationTextBox.Text);
                            
                            // Get the newly inserted guide's ID
                            guideId = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                        
                        // 2. Insert selected languages
                        foreach (var language in languageChecklist.CheckedItems)
                        {
                            string languageName = language.ToString();
                            
                            // 2.1 First check if language exists
                            string checkLanguageQuery = "SELECT language_id FROM Language WHERE language_name = @LanguageName";
                            int languageId = -1;
                            
                            using (SqlCommand cmd = new SqlCommand(checkLanguageQuery, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@LanguageName", languageName);
                                var result = cmd.ExecuteScalar();
                                
                                if (result != null)
                                {
                                    // Language exists
                                    languageId = Convert.ToInt32(result);
                                }
                            }
                            
                            // 2.2 If language doesn't exist, insert it
                            if (languageId == -1)
                            {
                                string insertLanguageQuery = @"INSERT INTO Language (language_name) 
                                                             VALUES (@LanguageName);
                                                             SELECT SCOPE_IDENTITY();";
                                
                                using (SqlCommand cmd = new SqlCommand(insertLanguageQuery, connection, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@LanguageName", languageName);
                                    languageId = Convert.ToInt32(cmd.ExecuteScalar());
                                }
                            }
                            
                            // 2.3 Create guide-language association
                            if (languageId > 0)
                            {
                                string insertGuideLanguageQuery = @"INSERT INTO GuideLanguage (guide_id, language_id) 
                                                                  VALUES (@GuideId, @LanguageId)";
                                
                                using (SqlCommand cmd = new SqlCommand(insertGuideLanguageQuery, connection, transaction))
                                {
                                    cmd.Parameters.AddWithValue("@GuideId", guideId);
                                    cmd.Parameters.AddWithValue("@LanguageId", languageId);
                                    
                                    cmd.ExecuteNonQuery();
                                }
                            }
                        }
                        
                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Roll back the transaction if something failed
                        transaction.Rollback();
                        throw new Exception($"Error registering guide: {ex.Message}", ex);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception($"Database connection error: {ex.Message}", ex);
                }
            }
        }
    }
}