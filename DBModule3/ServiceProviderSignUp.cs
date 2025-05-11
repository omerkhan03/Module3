using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DBModule3
{    public partial class ServiceProviderSignUp : Form
    {
        // Properties to store user registration information
        public int ProviderId { get; private set; }
        
        public ServiceProviderSignUp()
        {
            InitializeComponent();
            providerTypeComboBox.Items.AddRange(new string[] { "Hotel", "Transport Service", "Guide Service" });
        }        private void title_Click(object sender, EventArgs e)
        {
        }
        
        private void title_Click_1(object sender, EventArgs e)
        {
            // Empty method to handle the title's click event
        }        private void next_Click(object sender, EventArgs e)
        {
            if (providerTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a provider type.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Validate that all required information is filled in
            if (string.IsNullOrEmpty(usernameTextBox.Text) ||
                string.IsNullOrEmpty(passwordTextBox.Text) ||
                string.IsNullOrEmpty(emailTextBox.Text) ||
                string.IsNullOrEmpty(providerNameTextBox.Text))
            {
                MessageBox.Show("Please fill in all required fields.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            
            // Register the user and create service provider entry
            string providerType = MapProviderType(providerTypeComboBox.SelectedItem.ToString());
            
            try
            {
                // Register in database and get provider ID
                int providerId = RegisterServiceProvider(usernameTextBox.Text, passwordTextBox.Text, 
                                                      emailTextBox.Text, phoneTextBox.Text, providerType);
                
                if (providerId > 0)
                {
                    // Store the provider ID
                    this.ProviderId = providerId;
                    
                    // Open the appropriate form based on the provider type
                    Form nextForm = null;
                    switch (providerTypeComboBox.SelectedItem.ToString())
                    {
                        case "Hotel":
                            nextForm = new HotelRegistration();
                            break;
                        case "Transport Service":
                            nextForm = new TransportRegistration();
                            break;
                        case "Guide Service":
                            nextForm = new GuideRegistration();
                            break;
                    }

                    if (nextForm != null)
                    {                        // Pass the provider ID to the next form if it has a ProviderId property
                        if (nextForm is HotelRegistration hotelForm)
                        {
                            hotelForm.ProviderId = providerId;
                            hotelForm.HotelName = providerNameTextBox.Text;
                        }
                        else if (nextForm is TransportRegistration transportForm)
                        {
                            transportForm.ProviderId = providerId;
                        }
                        else if (nextForm is GuideRegistration guideForm)
                        {
                            guideForm.ProviderId = providerId;
                        }
                        
                        nextForm.Show();
                        this.Hide();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during registration: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private string MapProviderType(string uiProviderType)
        {
            switch (uiProviderType)
            {
                case "Hotel": return "Hotel";
                case "Transport Service": return "Transport";
                case "Guide Service": return "Guide";
                default: return uiProviderType;
            }
        }
        
        private int RegisterServiceProvider(string username, string password, string email, string phone, string providerType)
        {
            int providerId = -1;
            
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                try
                {
                    connection.Open();
                    
                    // Begin transaction
                    SqlTransaction transaction = connection.BeginTransaction();
                    
                    try
                    {
                        // 1. First insert into [User] table
                        string insertUserQuery = @"INSERT INTO [User] (username, password, email, phone_number, role) 
                                                 VALUES (@Username, @Password, @Email, @Phone, @Role); 
                                                 SELECT SCOPE_IDENTITY();";
                        
                        int userId;
                        using (SqlCommand cmd = new SqlCommand(insertUserQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@Username", username);
                            cmd.Parameters.AddWithValue("@Password", password);
                            cmd.Parameters.AddWithValue("@Email", email);
                            cmd.Parameters.AddWithValue("@Phone", phone);
                            cmd.Parameters.AddWithValue("@Role", "service_provider");
                            
                            // Get the newly inserted user's ID
                            userId = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                        
                        // 2. Then insert into ServiceProvider table
                        if (userId > 0)
                        {
                            string insertProviderQuery = @"INSERT INTO ServiceProvider (user_id, provider_type) 
                                                         VALUES (@UserID, @ProviderType); 
                                                         SELECT SCOPE_IDENTITY();";
                            
                            using (SqlCommand cmd = new SqlCommand(insertProviderQuery, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@UserID", userId);
                                cmd.Parameters.AddWithValue("@ProviderType", providerType);
                                
                                providerId = Convert.ToInt32(cmd.ExecuteScalar());
                            }
                        }
                        
                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Roll back the transaction if something failed
                        transaction.Rollback();
                        MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return -1;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Connection error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
            }
            
            return providerId;
        }

        private void back_Click(object sender, EventArgs e)
        {
            Landing landing = new Landing();
            landing.Show();
            this.Hide();
        }

        private void back_Click_1(object sender, EventArgs e)
        {
            Landing landing = new Landing();
            landing.Show();
            this.Hide();
        }
    }
}