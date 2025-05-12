using System;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace DBModule3
{
    public partial class TourOperatorSignUp : Form
    {
        //private readonly Database db; 

        public TourOperatorSignUp()
        {
            InitializeComponent();
           // db = new Database();
        }

        private void title_Click(object sender, EventArgs e)
        {
        }



        private void phoneLabel_Click(object sender, EventArgs e)
        {

        }        private void registerbutton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(companyNameTextBox.Text) ||
                string.IsNullOrEmpty(descriptionTextBox.Text) ||
                string.IsNullOrEmpty(addressTextBox.Text) ||
                string.IsNullOrEmpty(usernameTextBox.Text) ||
                string.IsNullOrEmpty(passwordTextBox.Text) ||
                string.IsNullOrEmpty(emailTextBox.Text) ||
                string.IsNullOrEmpty(phoneTextBox.Text))
            {
                MessageBox.Show("Please fill in all required fields.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Insert data into database
                int userId = InsertTourOperatorData();                  if (userId > 0)
                {
                    MessageBox.Show("Tour Operator registered successfully!",
                        "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    TourOperatorDashboard tourOp = new TourOperatorDashboard(userId);
                    tourOp.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during registration: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
          private int InsertTourOperatorData()
        {
            int userId = -1;
            
            using (SqlConnection connection = DatabaseHelper.CreateConnection())
            {
                try
                {
                    connection.Open();
                    
                    // 1. First insert into [User] table
                    string insertUserQuery = @"INSERT INTO [User] (username, password, email, phone_number, role) 
                                             VALUES (@Username, @Password, @Email, @Phone, @Role); 
                                             SELECT SCOPE_IDENTITY();";
                    
                    using (SqlCommand cmd = new SqlCommand(insertUserQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Username", usernameTextBox.Text);
                        cmd.Parameters.AddWithValue("@Password", passwordTextBox.Text);
                        cmd.Parameters.AddWithValue("@Email", emailTextBox.Text);
                        cmd.Parameters.AddWithValue("@Phone", phoneTextBox.Text);
                        cmd.Parameters.AddWithValue("@Role", "tour_operator");
                        
                        // Get the newly inserted user's ID
                        userId = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    
                    // 2. Then insert into TourOperator table
                    if (userId > 0)
                    {
                        string insertOperatorQuery = @"INSERT INTO TourOperator (user_id, company_name, description, address) 
                                                    VALUES (@UserID, @CompanyName, @Description, @Address)";
                        
                        using (SqlCommand cmd = new SqlCommand(insertOperatorQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@UserID", userId);
                            cmd.Parameters.AddWithValue("@CompanyName", companyNameTextBox.Text);
                            cmd.Parameters.AddWithValue("@Description", descriptionTextBox.Text);
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
    }
}