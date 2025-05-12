using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text;

namespace DBModule3
{
    public partial class AllTrips : UserControl
    {
        public AllTrips()
        {
            InitializeComponent();
            LoadAllTrips();
        }

        // Overloaded constructor that takes search parameters
        public AllTrips(string searchTerm = "", decimal? minBudget = null, decimal? maxBudget = null, 
                        int? duration = null, int? groupSize = null)
        {
            InitializeComponent();
            LoadFilteredTrips(searchTerm, minBudget, maxBudget, duration, groupSize);
        }

        private void LoadAllTrips()
        {
            LoadFilteredTrips("", null, null, null, null);
        }

        private void LoadFilteredTrips(string searchTerm, decimal? minBudget, decimal? maxBudget, 
                                      int? duration, int? groupSize)
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();

                    // Build the base SQL query
                    StringBuilder queryBuilder = new StringBuilder(
                        @"SELECT t.trip_id, t.trip_name, d.destination_name, tc.category_name, 
                        t.start_date, t.end_date, t.duration, t.price, 
                        t.capacity, t.sustainability_score, 
                        CASE WHEN t.wheelchair_access = 1 THEN 'Yes' ELSE 'No' END AS wheelchair_access,
                        o.company_name
                        FROM Trip t
                        JOIN Destination d ON t.destination_id = d.destination_id
                        JOIN TripCategory tc ON t.category_id = tc.category_id
                        JOIN TourOperator o ON t.operator_id = o.operator_id
                        WHERE t.start_date > GETDATE() ");

                    // Create parameter collection
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = connection;

                    // Apply search term if provided
                    if (!string.IsNullOrWhiteSpace(searchTerm))
                    {
                        queryBuilder.Append("AND (t.trip_name LIKE @SearchTerm OR d.destination_name LIKE @SearchTerm OR tc.category_name LIKE @SearchTerm) ");
                        cmd.Parameters.AddWithValue("@SearchTerm", "%" + searchTerm + "%");
                    }

                    // Apply minimum budget filter if provided
                    if (minBudget.HasValue)
                    {
                        queryBuilder.Append("AND t.price >= @MinBudget ");
                        cmd.Parameters.AddWithValue("@MinBudget", minBudget.Value);
                    }

                    // Apply maximum budget filter if provided
                    if (maxBudget.HasValue)
                    {
                        queryBuilder.Append("AND t.price <= @MaxBudget ");
                        cmd.Parameters.AddWithValue("@MaxBudget", maxBudget.Value);
                    }

                    // Apply duration filter if provided
                    if (duration.HasValue)
                    {
                        queryBuilder.Append("AND t.duration <= @Duration ");
                        cmd.Parameters.AddWithValue("@Duration", duration.Value);
                    }

                    // Apply group size filter if provided
                    if (groupSize.HasValue)
                    {
                        queryBuilder.Append("AND t.capacity >= @GroupSize ");
                        cmd.Parameters.AddWithValue("@GroupSize", groupSize.Value);
                    }

                    // Sort by start date
                    queryBuilder.Append("ORDER BY t.start_date ASC");

                    // Set complete query to the command
                    cmd.CommandText = queryBuilder.ToString();

                    // Execute the query
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);

                    // Update title based on whether we're showing filtered results
                    bool isFiltered = !string.IsNullOrWhiteSpace(searchTerm) || minBudget.HasValue || 
                                     maxBudget.HasValue || duration.HasValue || groupSize.HasValue;
                    
                    titleLabel.Text = isFiltered ? "Filtered Travel Packages" : "Available Travel Packages";

                    // Rename columns for better display
                    dataTable.Columns["trip_id"].ColumnName = "Trip ID";
                    dataTable.Columns["trip_name"].ColumnName = "Trip Name";
                    dataTable.Columns["destination_name"].ColumnName = "Destination";
                    dataTable.Columns["category_name"].ColumnName = "Category";
                    dataTable.Columns["start_date"].ColumnName = "Start Date";
                    dataTable.Columns["end_date"].ColumnName = "End Date";
                    dataTable.Columns["duration"].ColumnName = "Duration (days)";
                    dataTable.Columns["price"].ColumnName = "Price";
                    dataTable.Columns["capacity"].ColumnName = "Capacity";
                    dataTable.Columns["sustainability_score"].ColumnName = "Sustainability";
                    dataTable.Columns["wheelchair_access"].ColumnName = "Wheelchair Access";
                    dataTable.Columns["company_name"].ColumnName = "Tour Operator";

                    // Display data in the DataGridView
                    tripsDataGridView.DataSource = dataTable;

                    // Format the DataGridView
                    FormatDataGridView();

                    // Show result count
                    if (isFiltered)
                    {
                        MessageBox.Show($"Found {dataTable.Rows.Count} trips matching your search criteria.",
                            "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading trips: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FormatDataGridView()
        {
            // Set column widths
            tripsDataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            
            // Format date columns
            foreach (DataGridViewColumn column in tripsDataGridView.Columns)
            {
                if (column.Name.Contains("Date"))
                {
                    column.DefaultCellStyle.Format = "dd/MM/yyyy";
                }
            }

            // Format the price column
            DataGridViewColumn priceColumn = tripsDataGridView.Columns["Price"];
            if (priceColumn != null)
            {
                priceColumn.DefaultCellStyle.Format = "C2";
            }

            // Style the grid
            tripsDataGridView.RowHeadersVisible = false;
            tripsDataGridView.BorderStyle = BorderStyle.None;
            tripsDataGridView.BackgroundColor = System.Drawing.Color.White;
            tripsDataGridView.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.LightBlue;
            tripsDataGridView.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.Black;
            tripsDataGridView.AllowUserToAddRows = false;
            tripsDataGridView.AllowUserToDeleteRows = false;
            tripsDataGridView.ReadOnly = true;
            tripsDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            tripsDataGridView.MultiSelect = false;
            tripsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void InitializeComponent()
        {
            this.tripsDataGridView = new System.Windows.Forms.DataGridView();
            this.titleLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.tripsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // tripsDataGridView
            // 
            this.tripsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tripsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tripsDataGridView.Location = new System.Drawing.Point(20, 70);
            this.tripsDataGridView.Name = "tripsDataGridView";
            this.tripsDataGridView.Size = new System.Drawing.Size(760, 360);
            this.tripsDataGridView.TabIndex = 0;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Font = new System.Drawing.Font("Century Gothic", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.Location = new System.Drawing.Point(15, 20);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(296, 26);
            this.titleLabel.TabIndex = 1;
            this.titleLabel.Text = "Available Travel Packages";
            // 
            // AllTrips
            // 
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.tripsDataGridView);
            this.Name = "AllTrips";
            this.Size = new System.Drawing.Size(800, 450);
            this.Load += new System.EventHandler(this.AllTrips_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tripsDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.DataGridView tripsDataGridView;
        private System.Windows.Forms.Label titleLabel;

        private void AllTrips_Load(object sender, EventArgs e)
        {
            // Already handled in constructor
        }
    }
}
