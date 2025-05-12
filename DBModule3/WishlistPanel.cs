using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace DBModule3
{
    public class WishlistPanel : UserControl
    {
        private int userId;
        private DataGridView wishlistGrid;
        private TextBox tripIdTextBox;
        private Button addButton;
        private Label titleLabel;
        private Label addLabel;
        private DataGridViewButtonColumn RemoveButton;
        private int travelerId;

        public WishlistPanel(int userId)
        {
            this.userId = userId;
            InitializeComponent();
            LoadTravelerId();
            LoadWishlist();
        }

        private void InitializeComponent()
        {
            this.wishlistGrid = new System.Windows.Forms.DataGridView();
            this.RemoveButton = new System.Windows.Forms.DataGridViewButtonColumn();
            this.tripIdTextBox = new System.Windows.Forms.TextBox();
            this.addButton = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.addLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.wishlistGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // wishlistGrid
            // 
            this.wishlistGrid.AllowUserToAddRows = false;
            this.wishlistGrid.AllowUserToDeleteRows = false;
            this.wishlistGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.wishlistGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.wishlistGrid.BackgroundColor = System.Drawing.Color.White;
            this.wishlistGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.wishlistGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.wishlistGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.wishlistGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RemoveButton});
            this.wishlistGrid.Font = new System.Drawing.Font("Century Gothic", 10F);
            this.wishlistGrid.Location = new System.Drawing.Point(10, 90);
            this.wishlistGrid.MultiSelect = false;
            this.wishlistGrid.Name = "wishlistGrid";
            this.wishlistGrid.ReadOnly = true;
            this.wishlistGrid.RowHeadersVisible = false;
            this.wishlistGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.wishlistGrid.Size = new System.Drawing.Size(1978, 1108);
            this.wishlistGrid.TabIndex = 0;
            this.wishlistGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.WishlistGrid_CellClick);
            // 
            // RemoveButton
            // 
            this.RemoveButton.HeaderText = "Action";
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.ReadOnly = true;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseColumnTextForButtonValue = true;
            // 
            // tripIdTextBox
            // 
            this.tripIdTextBox.Font = new System.Drawing.Font("Century Gothic", 10F);
            this.tripIdTextBox.Location = new System.Drawing.Point(246, 50);
            this.tripIdTextBox.Name = "tripIdTextBox";
            this.tripIdTextBox.Size = new System.Drawing.Size(100, 24);
            this.tripIdTextBox.TabIndex = 2;
            // 
            // addButton
            // 
            this.addButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(25)))), ((int)(((byte)(72)))));
            this.addButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addButton.Font = new System.Drawing.Font("Century Gothic", 9F);
            this.addButton.ForeColor = System.Drawing.Color.White;
            this.addButton.Location = new System.Drawing.Point(362, 50);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(75, 25);
            this.addButton.TabIndex = 1;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = false;
            this.addButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleLabel.Font = new System.Drawing.Font("Century Gothic", 14F, System.Drawing.FontStyle.Bold);
            this.titleLabel.Location = new System.Drawing.Point(0, 0);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(108, 23);
            this.titleLabel.TabIndex = 3;
            this.titleLabel.Text = "My Wishlist";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // addLabel
            // 
            this.addLabel.AutoSize = true;
            this.addLabel.Font = new System.Drawing.Font("Century Gothic", 10F);
            this.addLabel.Location = new System.Drawing.Point(10, 50);
            this.addLabel.Name = "addLabel";
            this.addLabel.Size = new System.Drawing.Size(230, 19);
            this.addLabel.TabIndex = 4;
            this.addLabel.Text = "Add Trip to Wishlist (Enter Trip ID):";
            // 
            // WishlistPanel
            // 
            this.Controls.Add(this.wishlistGrid);
            this.Controls.Add(this.addButton);
            this.Controls.Add(this.tripIdTextBox);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.addLabel);
            this.Name = "WishlistPanel";
            this.Size = new System.Drawing.Size(1198, 708);
            ((System.ComponentModel.ISupportInitialize)(this.wishlistGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void LoadTravelerId()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.CreateConnection())
                {
                    conn.Open();
                    string query = "SELECT traveler_id FROM Traveler WHERE user_id = @UserId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@UserId", userId);
                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            travelerId = Convert.ToInt32(result);
                        }
                        else
                        {
                            MessageBox.Show("Error: Traveler not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading traveler data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadWishlist()
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.CreateConnection())
                {
                    conn.Open();
                    string query = @"
                        SELECT 
                            t.trip_id as [Trip ID],
                            t.trip_name as [Trip Name],
                            tc.category_name as [Category],
                            t.price as [Price],
                            t.start_date as [Start Date],
                            t.end_date as [End Date],
                            wt.added_date as [Added On]
                        FROM WishList w
                        JOIN WishListTrip wt ON w.wishlist_id = wt.wishlist_id
                        JOIN Trip t ON wt.trip_id = t.trip_id
                        JOIN TripCategory tc ON t.category_id = tc.category_id
                        WHERE w.traveler_id = @TravelerId
                        ORDER BY wt.added_date DESC";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TravelerId", travelerId);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            DataTable dt = new DataTable();
                            adapter.Fill(dt);
                            wishlistGrid.DataSource = dt;

                            // Format the columns
                            if (wishlistGrid.Columns["Price"] != null)
                                wishlistGrid.Columns["Price"].DefaultCellStyle.Format = "C2";
                            if (wishlistGrid.Columns["Start Date"] != null)
                                wishlistGrid.Columns["Start Date"].DefaultCellStyle.Format = "d";
                            if (wishlistGrid.Columns["End Date"] != null)
                                wishlistGrid.Columns["End Date"].DefaultCellStyle.Format = "d";
                            if (wishlistGrid.Columns["Added On"] != null)
                                wishlistGrid.Columns["Added On"].DefaultCellStyle.Format = "d";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading wishlist: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(tripIdTextBox.Text, out int tripId))
            {
                MessageBox.Show("Please enter a valid Trip ID.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection conn = DatabaseHelper.CreateConnection())
                {
                    conn.Open();
                    SqlTransaction transaction = conn.BeginTransaction();

                    try
                    {
                        // First verify the trip exists
                        string verifyQuery = "SELECT COUNT(*) FROM Trip WHERE trip_id = @TripId";
                        using (SqlCommand cmd = new SqlCommand(verifyQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@TripId", tripId);
                            int tripExists = (int)cmd.ExecuteScalar();
                            if (tripExists == 0)
                            {
                                MessageBox.Show("Trip not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                transaction.Rollback();
                                return;
                            }
                        }

                        // Get or create wishlist
                        int wishlistId;
                        string wishlistQuery = @"
                            SELECT wishlist_id FROM WishList WHERE traveler_id = @TravelerId;
                            IF @@ROWCOUNT = 0
                            BEGIN
                                INSERT INTO WishList (traveler_id, date_created)
                                VALUES (@TravelerId, GETDATE());
                                SELECT SCOPE_IDENTITY();
                            END";

                        using (SqlCommand cmd = new SqlCommand(wishlistQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@TravelerId", travelerId);
                            var result = cmd.ExecuteScalar();
                            wishlistId = Convert.ToInt32(result);
                        }

                        // Check if trip is already in wishlist
                        string checkQuery = "SELECT COUNT(*) FROM WishListTrip WHERE wishlist_id = @WishlistId AND trip_id = @TripId";
                        using (SqlCommand cmd = new SqlCommand(checkQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@WishlistId", wishlistId);
                            cmd.Parameters.AddWithValue("@TripId", tripId);
                            int exists = (int)cmd.ExecuteScalar();
                            if (exists > 0)
                            {
                                MessageBox.Show("This trip is already in your wishlist.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                transaction.Rollback();
                                return;
                            }
                        }

                        // Add to wishlist
                        string insertQuery = "INSERT INTO WishListTrip (wishlist_id, trip_id) VALUES (@WishlistId, @TripId)";
                        using (SqlCommand cmd = new SqlCommand(insertQuery, conn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@WishlistId", wishlistId);
                            cmd.Parameters.AddWithValue("@TripId", tripId);
                            cmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        MessageBox.Show("Trip added to wishlist successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        tripIdTextBox.Clear();
                        LoadWishlist(); // Refresh the grid
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error adding to wishlist: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WishlistGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if clicked cell is in the Remove button column
            if (e.ColumnIndex == wishlistGrid.Columns["RemoveButton"].Index && e.RowIndex >= 0)
            {
                int tripId = Convert.ToInt32(wishlistGrid.Rows[e.RowIndex].Cells["Trip ID"].Value);
                RemoveFromWishlist(tripId);
            }
        }

        private void RemoveFromWishlist(int tripId)
        {
            try
            {
                using (SqlConnection conn = DatabaseHelper.CreateConnection())
                {
                    conn.Open();
                    string query = @"
                        DELETE FROM WishListTrip 
                        WHERE wishlist_id IN (SELECT wishlist_id FROM WishList WHERE traveler_id = @TravelerId)
                        AND trip_id = @TripId";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@TravelerId", travelerId);
                        cmd.Parameters.AddWithValue("@TripId", tripId);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Trip removed from wishlist successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadWishlist(); // Refresh the grid
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error removing trip from wishlist: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
