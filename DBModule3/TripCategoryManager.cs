using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DBModule3
{
    public partial class TripCategoryManager : UserControl
    {
        private DataGridView categoriesGrid;
        private TextBox categoryNameBox;
        private TextBox descriptionBox;
        private Button addButton;
        private Label titleLabel;

        public TripCategoryManager()
        {
            InitializeComponent();
            InitializeUI();
            LoadCategories();
        }

        private void InitializeUI()
        {
            // Title
            titleLabel = new Label();
            titleLabel.Text = "TRIP CATEGORY MANAGEMENT";
            titleLabel.Font = new System.Drawing.Font("Century Gothic", 16F, System.Drawing.FontStyle.Bold);
            titleLabel.Location = new System.Drawing.Point(20, 20);
            titleLabel.AutoSize = true;
            Controls.Add(titleLabel);

            // Category name input
            Label nameLabel = new Label();
            nameLabel.Text = "Category Name:";
            nameLabel.Font = new System.Drawing.Font("Century Gothic", 10F);
            nameLabel.Location = new System.Drawing.Point(20, 70);
            nameLabel.AutoSize = true;
            Controls.Add(nameLabel);

            categoryNameBox = new TextBox();
            categoryNameBox.Font = new System.Drawing.Font("Century Gothic", 10F);
            categoryNameBox.Location = new System.Drawing.Point(150, 70);
            categoryNameBox.Size = new System.Drawing.Size(200, 25);
            Controls.Add(categoryNameBox);

            // Description input
            Label descLabel = new Label();
            descLabel.Text = "Description:";
            descLabel.Font = new System.Drawing.Font("Century Gothic", 10F);
            descLabel.Location = new System.Drawing.Point(20, 110);
            descLabel.AutoSize = true;
            Controls.Add(descLabel);

            descriptionBox = new TextBox();
            descriptionBox.Font = new System.Drawing.Font("Century Gothic", 10F);
            descriptionBox.Location = new System.Drawing.Point(150, 110);
            descriptionBox.Size = new System.Drawing.Size(400, 25);
            descriptionBox.Multiline = true;
            descriptionBox.Height = 60;
            Controls.Add(descriptionBox);

            // Add button
            addButton = new Button();
            addButton.Text = "Add Category";
            addButton.Font = new System.Drawing.Font("Century Gothic", 10F);
            addButton.Location = new System.Drawing.Point(150, 190);
            addButton.Size = new System.Drawing.Size(120, 30);
            addButton.BackColor = System.Drawing.Color.Orange;
            addButton.ForeColor = System.Drawing.Color.White;
            addButton.Click += AddButton_Click;
            Controls.Add(addButton);

            // Categories grid
            categoriesGrid = new DataGridView();
            categoriesGrid.Location = new System.Drawing.Point(20, 240);
            categoriesGrid.Size = new System.Drawing.Size(700, 300);
            categoriesGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            categoriesGrid.BackgroundColor = System.Drawing.Color.White;
            categoriesGrid.Font = new System.Drawing.Font("Century Gothic", 10F);
            categoriesGrid.AutoGenerateColumns = false;

            // Add columns
            categoriesGrid.Columns.Add("category_id", "ID");
            categoriesGrid.Columns.Add("category_name", "Category Name");
            categoriesGrid.Columns.Add("description", "Description");
            categoriesGrid.Columns.Add("trip_count", "Active Trips");

            // Add delete button column
            DataGridViewButtonColumn deleteButton = new DataGridViewButtonColumn();
            deleteButton.HeaderText = "Action";
            deleteButton.Text = "Delete";
            deleteButton.UseColumnTextForButtonValue = true;
            categoriesGrid.Columns.Add(deleteButton);

            categoriesGrid.CellClick += CategoriesGrid_CellClick;
            Controls.Add(categoriesGrid);
        }

        private void LoadCategories()
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    string query = @"
                        SELECT tc.category_id, tc.category_name, tc.description,
                               COUNT(t.trip_id) as trip_count
                        FROM TripCategory tc
                        LEFT JOIN Trip t ON tc.category_id = t.category_id
                        GROUP BY tc.category_id, tc.category_name, tc.description
                        ORDER BY tc.category_name";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        categoriesGrid.Rows.Clear();
                        foreach (DataRow row in dataTable.Rows)
                        {
                            categoriesGrid.Rows.Add(
                                row["category_id"],
                                row["category_name"],
                                row["description"],
                                row["trip_count"]
                            );
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(categoryNameBox.Text))
            {
                MessageBox.Show("Please enter a category name", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    string query = @"INSERT INTO TripCategory (category_name, description)
                                   VALUES (@CategoryName, @Description)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@CategoryName", categoryNameBox.Text);
                        cmd.Parameters.AddWithValue("@Description", descriptionBox.Text ?? "");

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Category added successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear inputs and reload grid
                    categoryNameBox.Clear();
                    descriptionBox.Clear();
                    LoadCategories();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding category: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CategoriesGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if delete button was clicked
            if (e.ColumnIndex == categoriesGrid.Columns.Count - 1 && e.RowIndex >= 0)
            {
                int categoryId = Convert.ToInt32(categoriesGrid.Rows[e.RowIndex].Cells["category_id"].Value);
                int tripCount = Convert.ToInt32(categoriesGrid.Rows[e.RowIndex].Cells["trip_count"].Value);
                string categoryName = categoriesGrid.Rows[e.RowIndex].Cells["category_name"].Value.ToString();

                if (tripCount > 0)
                {
                    MessageBox.Show($"Cannot delete category '{categoryName}' because it has active trips.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (MessageBox.Show($"Are you sure you want to delete the category '{categoryName}'?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    DeleteCategory(categoryId);
                }
            }
        }

        private void DeleteCategory(int categoryId)
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    string query = "DELETE FROM TripCategory WHERE category_id = @CategoryId";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Category deleted successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    LoadCategories();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting category: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
