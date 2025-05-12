using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DBModule3
{
    public partial class ServiceProviderAssignmentBar : UserControl
    {
        private int userId;        private DataGridView assignmentsGridView;

        public ServiceProviderAssignmentBar(int userId = 1)
        {
            InitializeComponent();
            this.userId = userId;
            searchbutton.Click += AcceptButton_Click;
            sataButton1.Click += RejectButton_Click;
            InitializeGrid();
            LoadAssignments();
        }

        private void InitializeGrid()
        {
            assignmentsGridView = new DataGridView();
            assignmentsGridView.Dock = DockStyle.Bottom;
            assignmentsGridView.Height = 300;
            assignmentsGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            assignmentsGridView.BackgroundColor = System.Drawing.Color.White;
            assignmentsGridView.Font = new System.Drawing.Font("Century Gothic", 10F);
            assignmentsGridView.AutoGenerateColumns = false;
            assignmentsGridView.ReadOnly = true;
              // Add columns
            assignmentsGridView.Columns.Add("assignment_id", "Assignment ID");
            assignmentsGridView.Columns.Add("booking_id", "Booking ID");
            assignmentsGridView.Columns.Add("trip_name", "Trip Name");
            assignmentsGridView.Columns.Add("assignment_date", "Assignment Date");
            assignmentsGridView.Columns.Add("status", "Status");

            Controls.Add(assignmentsGridView);
        }

        private void LoadAssignments()
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    string query = @"
                        SELECT sa.assignment_id, sa.booking_id, t.trip_name, sa.assignment_date, sa.status
                        FROM ServiceAssignment sa
                        JOIN Booking b ON sa.booking_id = b.booking_id
                        JOIN Trip t ON b.trip_id = t.trip_id
                        JOIN ServiceProvider sp ON sa.provider_id = sp.provider_id
                        WHERE sp.user_id = @UserId
                        ORDER BY sa.assignment_date DESC";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        assignmentsGridView.Rows.Clear();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            assignmentsGridView.Rows.Add(
                                row["assignment_id"],
                                row["booking_id"],
                                row["trip_name"],
                                ((DateTime)row["assignment_date"]).ToShortDateString(),
                                row["status"]
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading assignments: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AcceptButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(assignment.Texts))
            {
                MessageBox.Show("Please enter an assignment ID", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(assignment.Texts, out int assignmentId))
            {
                MessageBox.Show("Please enter a valid assignment ID", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // First verify the assignment exists and belongs to this provider
                        string verifyQuery = @"SELECT sa.status 
                                            FROM ServiceAssignment sa
                                            JOIN ServiceProvider sp ON sa.provider_id = sp.provider_id
                                            WHERE sa.assignment_id = @AssignmentId 
                                            AND sp.user_id = @UserId";

                        using (SqlCommand cmd = new SqlCommand(verifyQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@AssignmentId", assignmentId);
                            cmd.Parameters.AddWithValue("@UserId", userId);
                            
                            object result = cmd.ExecuteScalar();
                            if (result == null)
                            {
                                MessageBox.Show("Assignment not found or you don't have permission to modify it.", 
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            string currentStatus = result.ToString();
                            if (currentStatus != "Pending")
                            {
                                MessageBox.Show($"Cannot accept assignment with status: {currentStatus}", 
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        // Update the status to Confirmed
                        string updateQuery = @"UPDATE ServiceAssignment 
                                            SET status = 'Confirmed'
                                            WHERE assignment_id = @AssignmentId";

                        using (SqlCommand cmd = new SqlCommand(updateQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@AssignmentId", assignmentId);
                            cmd.ExecuteNonQuery();
                        }                        transaction.Commit();
                        MessageBox.Show("Assignment successfully accepted!", 
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        assignment.Texts = "";
                        LoadAssignments(); // Refresh the grid
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error accepting assignment: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RejectButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(assignment.Texts))
            {
                MessageBox.Show("Please enter an assignment ID", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(assignment.Texts, out int assignmentId))
            {
                MessageBox.Show("Please enter a valid assignment ID", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // First verify the assignment exists and belongs to this provider
                        string verifyQuery = @"SELECT sa.status 
                                            FROM ServiceAssignment sa
                                            JOIN ServiceProvider sp ON sa.provider_id = sp.provider_id
                                            WHERE sa.assignment_id = @AssignmentId 
                                            AND sp.user_id = @UserId";

                        using (SqlCommand cmd = new SqlCommand(verifyQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@AssignmentId", assignmentId);
                            cmd.Parameters.AddWithValue("@UserId", userId);
                            
                            object result = cmd.ExecuteScalar();
                            if (result == null)
                            {
                                MessageBox.Show("Assignment not found or you don't have permission to modify it.", 
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            string currentStatus = result.ToString();
                            if (currentStatus != "Pending")
                            {
                                MessageBox.Show($"Cannot reject assignment with status: {currentStatus}", 
                                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }

                        // Update the status to Rejected
                        string updateQuery = @"UPDATE ServiceAssignment 
                                            SET status = 'Rejected'
                                            WHERE assignment_id = @AssignmentId";

                        using (SqlCommand cmd = new SqlCommand(updateQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@AssignmentId", assignmentId);
                            cmd.ExecuteNonQuery();
                        }                        transaction.Commit();
                        MessageBox.Show("Assignment successfully rejected!", 
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        assignment.Texts = "";
                        LoadAssignments(); // Refresh the grid
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error rejecting assignment: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ServiceProviderAssignmentBar_Load(object sender, EventArgs e)
        {

        }
    }
}
