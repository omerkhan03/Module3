using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DBModule3
{
    public partial class UserManager : UserControl
    {
        public UserManager()
        {
            InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT 
                            u.user_id,
                            u.username,
                            u.email, 
                            u.phone_number,
                            u.registration_date,
                            u.is_active,
                            u.role,
                            CASE 
                                WHEN t.first_name IS NOT NULL THEN t.first_name + ' ' + t.last_name
                                WHEN to2.company_name IS NOT NULL THEN to2.company_name
                                WHEN sp.provider_type IS NOT NULL THEN 
                                    CASE 
                                        WHEN h.hotel_name IS NOT NULL THEN h.hotel_name
                                        WHEN ts.transport_type IS NOT NULL THEN ts.transport_type
                                        WHEN g.guide_name IS NOT NULL THEN g.guide_name
                                        ELSE 'Unknown'
                                    END
                                ELSE 'Admin'
                            END as display_name
                        FROM [User] u
                        LEFT JOIN Traveler t ON u.user_id = t.user_id
                        LEFT JOIN TourOperator to2 ON u.user_id = to2.user_id
                        LEFT JOIN ServiceProvider sp ON u.user_id = sp.user_id
                        LEFT JOIN Hotel h ON sp.provider_id = h.provider_id
                        LEFT JOIN TransportService ts ON sp.provider_id = ts.provider_id
                        LEFT JOIN Guide g ON sp.provider_id = g.provider_id
                        ORDER BY u.registration_date DESC";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Rename columns for display
                    dataTable.Columns["user_id"].ColumnName = "User ID";
                    dataTable.Columns["username"].ColumnName = "Username";
                    dataTable.Columns["email"].ColumnName = "Email";
                    dataTable.Columns["phone_number"].ColumnName = "Phone Number";
                    dataTable.Columns["registration_date"].ColumnName = "Registration Date";
                    dataTable.Columns["is_active"].ColumnName = "Active";
                    dataTable.Columns["role"].ColumnName = "Role";
                    dataTable.Columns["display_name"].ColumnName = "Name/Company";

                    usersDataGridView.DataSource = dataTable;
                    FormatDataGridView();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading users: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridView()
        {
            usersDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            usersDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            usersDataGridView.ReadOnly = true;
            usersDataGridView.AllowUserToAddRows = false;
            usersDataGridView.AllowUserToDeleteRows = false;
            usersDataGridView.AllowUserToOrderColumns = true;
            usersDataGridView.AllowUserToResizeRows = false;
            usersDataGridView.BackgroundColor = System.Drawing.Color.White;

            // Set visual styles
            usersDataGridView.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(238, 239, 249);
            usersDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            usersDataGridView.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.DarkTurquoise;
            usersDataGridView.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.WhiteSmoke;
            usersDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            usersDataGridView.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(20, 25, 72);
            usersDataGridView.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            usersDataGridView.EnableHeadersVisualStyles = false;            // Add Toggle Active button column if it doesn't exist
            if (!usersDataGridView.Columns.Contains("ToggleActiveButton"))
            {
                DataGridViewButtonColumn toggleActiveBtn = new DataGridViewButtonColumn();
                toggleActiveBtn.Name = "ToggleActiveButton";
                toggleActiveBtn.HeaderText = "Toggle Active";
                toggleActiveBtn.Text = "Toggle Active";
                toggleActiveBtn.UseColumnTextForButtonValue = true;
                usersDataGridView.Columns.Add(toggleActiveBtn);
                
                // Remove existing handler (if any) and add new one
                usersDataGridView.CellClick -= UsersDataGridView_CellClick;
                usersDataGridView.CellClick += UsersDataGridView_CellClick;
            }
        }

        private void UsersDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if it's a click on the toggle button
            if (e.ColumnIndex == usersDataGridView.Columns["ToggleActiveButton"].Index && e.RowIndex >= 0)
            {
                int userId = Convert.ToInt32(usersDataGridView.Rows[e.RowIndex].Cells["User ID"].Value);
                bool currentActive = Convert.ToBoolean(usersDataGridView.Rows[e.RowIndex].Cells["Active"].Value);

                try
                {
                    using (SqlConnection connection = DatabaseHelper.CreateConnection())
                    {
                        connection.Open();
                        SqlTransaction transaction = connection.BeginTransaction();

                        try
                        {
                            string updateQuery = "UPDATE [User] SET is_active = @IsActive WHERE user_id = @UserId";

                            using (SqlCommand cmd = new SqlCommand(updateQuery, connection, transaction))
                            {
                                cmd.Parameters.AddWithValue("@UserId", userId);
                                cmd.Parameters.AddWithValue("@IsActive", !currentActive); // Toggle the value
                                cmd.ExecuteNonQuery();
                            }

                            transaction.Commit();
                            MessageBox.Show("User status updated successfully!", "Success", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            LoadUsers(); // Refresh the grid
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw new Exception("Error updating user status: " + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
