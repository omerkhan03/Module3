using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBModule3
{
    public partial class TripCreator : UserControl
    {
        private int userId;

        public TripCreator(int operatorId = 60)
        {
            InitializeComponent();
            this.userId = operatorId;
            LoadCategories();
            SetupWheelchairRadioButtons();
            createtripbutton.Click += createtripbutton_Click;
        }

        private void LoadCategories()
        {
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT category_name FROM TripCategory ORDER BY category_name";
                    
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                categoryType.Items.Add(reader["category_name"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading trip categories: {ex.Message}", 
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupWheelchairRadioButtons()
        {
            // Convert checkboxes to radio button behavior
            WheelChairYes.Click += (s, e) => 
            {
                WheelChairYes.Checked = true;
                WheelChairNo.Checked = false;
            };

            WheelChairNo.Click += (s, e) => 
            {
                WheelChairYes.Checked = false;
                WheelChairNo.Checked = true;
            };

            // Set default
            WheelChairNo.Checked = true;
        }

        private void createtripbutton_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    SqlTransaction transaction = connection.BeginTransaction();

                    try
                    {
                        // Get operator ID from user ID
                        string operatorQuery = "SELECT operator_id FROM TourOperator WHERE user_id = @UserId";
                        int operatorId;
                        
                        using (SqlCommand cmd = new SqlCommand(operatorQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@UserId", userId);
                            var result = cmd.ExecuteScalar();
                            if (result == null)
                                throw new Exception("Tour operator not found.");
                            operatorId = Convert.ToInt32(result);
                        }

                        // Get category ID
                        string categoryQuery = "SELECT category_id FROM TripCategory WHERE category_name = @CategoryName";
                        int categoryId;
                        
                        using (SqlCommand cmd = new SqlCommand(categoryQuery, connection, transaction))
                        {
                            cmd.Parameters.AddWithValue("@CategoryName", categoryType.SelectedItem.ToString());
                            var result = cmd.ExecuteScalar();
                            if (result == null)
                                throw new Exception("Category not found.");
                            categoryId = Convert.ToInt32(result);
                        }                        // Get destination ID
                        int destinationId = int.Parse(destinationtext.Text);

                        // Get itinerary ID
                        int itineraryId = int.Parse(ItineraryBox.Text);

                        // Calculate duration
                        int duration = (EndDateBox.Value - startDateBox.Value).Days + 1;                        // Insert trip
                        string insertQuery = @"INSERT INTO Trip (
                            operator_id, category_id, destination_id, itinerary_id, trip_name, 
                            description, start_date, end_date, duration, 
                            price, capacity, sustainability_score, wheelchair_access)
                        VALUES (
                            @OperatorId, @CategoryId, @DestinationId, @ItineraryId, @TripName,
                            @Description, @StartDate, @EndDate, @Duration,
                            @Price, @Capacity, @SustainabilityScore, @WheelchairAccess);
                        SELECT SCOPE_IDENTITY();";

                        int tripId;
                        using (SqlCommand cmd = new SqlCommand(insertQuery, connection, transaction))
                        {                            cmd.Parameters.AddWithValue("@OperatorId", operatorId);
                            cmd.Parameters.AddWithValue("@CategoryId", categoryId);
                            cmd.Parameters.AddWithValue("@DestinationId", destinationId);
                            cmd.Parameters.AddWithValue("@ItineraryId", itineraryId);
                            cmd.Parameters.AddWithValue("@TripName", tripnametext.Text);
                            cmd.Parameters.AddWithValue("@Description", DescriptionBox.Text);
                            cmd.Parameters.AddWithValue("@StartDate", startDateBox.Value);
                            cmd.Parameters.AddWithValue("@EndDate", EndDateBox.Value);                            cmd.Parameters.AddWithValue("@Duration", duration);
                            cmd.Parameters.AddWithValue("@Price", decimal.Parse(PriceBox.Text));
                            cmd.Parameters.AddWithValue("@Capacity", int.Parse(CapacityBox.Text));
                            cmd.Parameters.AddWithValue("@SustainabilityScore", SustainabilityScore.Text.Trim().ToUpper());
                            cmd.Parameters.AddWithValue("@WheelchairAccess", WheelChairYes.Checked);

                            tripId = Convert.ToInt32(cmd.ExecuteScalar());
                        }

                        transaction.Commit();

                        MessageBox.Show($"Trip created successfully! ID: {tripId}", 
                            "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        // Clear form
                        ClearForm();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Error creating trip: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(tripnametext.Text))
            {
                MessageBox.Show("Please enter a trip name.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(ItineraryBox.Text))
            {
                MessageBox.Show("Please enter an itinerary.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (categoryType.SelectedItem == null)
            {
                MessageBox.Show("Please select a trip category.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }            if (!int.TryParse(destinationtext.Text, out _))
            {
                MessageBox.Show("Please enter a valid destination ID.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }            if (!int.TryParse(ItineraryBox.Text, out int itineraryId))
            {
                MessageBox.Show("Please enter a valid itinerary ID.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Verify that the itinerary exists
            try
            {
                using (SqlConnection connection = DatabaseHelper.CreateConnection())
                {
                    connection.Open();
                    string query = "SELECT COUNT(*) FROM Itinerary WHERE itinerary_id = @ItineraryId";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ItineraryId", itineraryId);
                        int count = (int)cmd.ExecuteScalar();
                        if (count == 0)
                        {
                            MessageBox.Show("The specified itinerary ID does not exist.", "Validation Error", 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error verifying itinerary: {ex.Message}", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(DescriptionBox.Text))
            {
                MessageBox.Show("Please enter a trip description.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!decimal.TryParse(PriceBox.Text, out decimal price) || price <= 0)
            {
                MessageBox.Show("Please enter a valid price (greater than 0).", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!int.TryParse(CapacityBox.Text, out int capacity) || capacity <= 0)
            {
                MessageBox.Show("Please enter a valid capacity (greater than 0).", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }            string grade = SustainabilityScore.Text.Trim().ToUpper();
            if (!new[] { "A+", "A", "B","B+", "C", "D", "F" }.Contains(grade))
            {
                MessageBox.Show("Please enter a valid sustainability grade (A+, A, B, B+, C, D, or F).", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (startDateBox.Value > EndDateBox.Value)
            {
                MessageBox.Show("Start date cannot be after end date.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (startDateBox.Value.Date < DateTime.Now.Date)
            {
                MessageBox.Show("Start date cannot be in the past.", "Validation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ClearForm()
        {            tripnametext.Clear();
            categoryType.SelectedIndex = -1;
            destinationtext.Clear();
            DescriptionBox.Clear();
            ItineraryBox.Clear();
            PriceBox.Clear();
            CapacityBox.Clear();
            SustainabilityScore.Clear();
            startDateBox.Value = DateTime.Now;
            EndDateBox.Value = DateTime.Now;
            WheelChairNo.Checked = true;
            WheelChairYes.Checked = false;
        }

        private void TripCreator_Load(object sender, EventArgs e)
        {

        }
    }
}
