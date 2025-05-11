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
    public partial class TravelerSignUp : Form
    {
        public TravelerSignUp()
        {
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(TravelerSignUp_FormClosing);
        }

        private void title_Click(object sender, EventArgs e)
        {

        }

        private void TravelerSignUp_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void TravelerSignUp_Load(object sender, EventArgs e)
        {

        }

        private void dateOfBirthPicker_ValueChanged(object sender, EventArgs e)
        {

        }        private void registerbutton_Click(object sender, EventArgs e)
        {
            // Validate required fields
            if (string.IsNullOrEmpty(firstNameTextBox.Text) ||
                string.IsNullOrEmpty(lastNameTextBox.Text) ||
                string.IsNullOrEmpty(usernameTextBox.Text) ||
                string.IsNullOrEmpty(passwordTextBox.Text) ||
                string.IsNullOrEmpty(emailTextBox.Text) ||
                string.IsNullOrEmpty(addressTextBox.Text) ||
                string.IsNullOrEmpty(phoneTextBox.Text) ||
                dateOfBirthPicker.Value == dateOfBirthPicker.MinDate ||
                string.IsNullOrEmpty(nationalityTextBox.Text))
            {
                MessageBox.Show("Please fill in all required fields.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // First create the user account and traveler profile in the database
                int userId = InsertUserData();
                
                if (userId > 0)
                {
                    // Ask if they want to set preferences now
                    var result = MessageBox.Show("Registration successful! Would you like to set your travel preferences now?",
                        "Success", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        TravelerPreferences preferencesForm = new TravelerPreferences();
                        preferencesForm.Show();
                    }
                    else
                    {
                        MessageBox.Show("You can set your preferences later from your profile.",
                            "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //TravelerDashboard travelerDashboard = new TravelerDashboard();
                        //travelerDashboard.Show();
                    }

                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during registration: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }        private int InsertUserData()
        {
            int userId = -1;
            
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                try
                {
                    connection.Open();
                      // 1. First insert into [User] table (with square brackets because user is a reserved keyword)
                    string insertUserQuery = @"INSERT INTO [User] (username, password, email, phone_number, role) 
                                             VALUES (@Username, @Password, @Email, @Phone, @Role); 
                                             SELECT SCOPE_IDENTITY();";
                    
                    using (SqlCommand cmd = new SqlCommand(insertUserQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Username", usernameTextBox.Text);
                        cmd.Parameters.AddWithValue("@Password", passwordTextBox.Text);
                        cmd.Parameters.AddWithValue("@Email", emailTextBox.Text);
                        cmd.Parameters.AddWithValue("@Phone", phoneTextBox.Text);
                        cmd.Parameters.AddWithValue("@Role", "traveler");
                        
                        // Get the newly inserted user's ID
                        userId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    
                    // 2. Then insert into Traveler table
                    if (userId > 0)
                    {
                        string insertTravelerQuery = @"INSERT INTO Traveler (user_id, first_name, last_name, date_of_birth, nationality, address) 
                                                    VALUES (@UserID, @FirstName, @LastName, @DateOfBirth, @Nationality, @Address)";
                        
                        using (SqlCommand cmd = new SqlCommand(insertTravelerQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@UserID", userId);
                            cmd.Parameters.AddWithValue("@FirstName", firstNameTextBox.Text);
                            cmd.Parameters.AddWithValue("@LastName", lastNameTextBox.Text);
                            cmd.Parameters.AddWithValue("@DateOfBirth", dateOfBirthPicker.Value);
                            cmd.Parameters.AddWithValue("@Nationality", nationalityTextBox.Text);
                            cmd.Parameters.AddWithValue("@Address", addressTextBox.Text);
                            
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
            }
            
            return userId;
        }

        private void back_Click(object sender, EventArgs e)
        {
            Landing landing = new Landing();
            landing.Show();
            this.Hide();
        }

        private void firstNameTextBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
