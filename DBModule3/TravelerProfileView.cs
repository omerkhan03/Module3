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
    public partial class TravelerProfileView : UserControl
    {
        private int userId;

        public TravelerProfileView(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            LoadUserProfile();
        }

        private void LoadUserProfile()
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            u.user_id,
                            u.phone_number,
                            u.email,
                            u.registration_date,
                            t.first_name,
                            t.last_name
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
                                // Display user information in labels
                                label10.Text = reader["user_id"].ToString();
                                label11.Text = reader["phone_number"]?.ToString() ?? "Not set";
                                label12.Text = reader["email"].ToString();
                                label13.Text = ((DateTime)reader["registration_date"]).ToString("dd/MM/yyyy");
                                
                                // Set textbox placeholders
                                phonenumchange.Text = reader["phone_number"]?.ToString() ?? "";
                                textBox1.Text = reader["email"].ToString();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading profile: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void reviewbutton_Click_1(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        bool changes = false;
                        string updateQuery = "UPDATE [User] SET ";
                        string updates = "";

                        // Check if phone number was changed
                        if (!string.IsNullOrWhiteSpace(phonenumchange.Text) && phonenumchange.Text != label11.Text)
                        {
                            updates += "phone_number = @Phone";
                            changes = true;
                        }

                        // Check if email was changed
                        if (!string.IsNullOrWhiteSpace(textBox1.Text) && textBox1.Text != label12.Text)
                        {
                            if (changes) updates += ", ";
                            updates += "email = @Email";
                            changes = true;
                        }

                        // Check if password was changed
                        if (!string.IsNullOrWhiteSpace(newPassword.Text))
                        {
                            if (changes) updates += ", ";
                            updates += "password = @Password";
                            changes = true;
                        }

                        if (changes)
                        {
                            updateQuery += updates + " WHERE user_id = @UserId";

                            using (SqlCommand cmd = new SqlCommand(updateQuery, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@UserId", userId);
                                if (!string.IsNullOrWhiteSpace(phonenumchange.Text) && phonenumchange.Text != label11.Text)
                                    cmd.Parameters.AddWithValue("@Phone", phonenumchange.Text);
                                if (!string.IsNullOrWhiteSpace(textBox1.Text) && textBox1.Text != label12.Text)
                                    cmd.Parameters.AddWithValue("@Email", textBox1.Text);
                                if (!string.IsNullOrWhiteSpace(newPassword.Text))
                                    cmd.Parameters.AddWithValue("@Password", newPassword.Text);

                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            MessageBox.Show("Profile updated successfully!", 
                                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadUserProfile(); // Reload the profile to show updated information
                            newPassword.Text = ""; // Clear password field
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error updating profile: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void TravelerProfileView_Load(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
