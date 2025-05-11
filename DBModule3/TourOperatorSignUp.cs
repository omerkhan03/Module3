using System;
using System.Windows.Forms;

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

        }

        private void registerbutton_Click(object sender, EventArgs e)
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
                // Create the user account first
                //int userId = db.CreateUser(
                //    usernameTextBox.Text,
                //    passwordTextBox.Text,
                //    emailTextBox.Text,
                //    phoneTextBox.Text,
                //    "TourOperator"
                //);

                // Then create the tour operator profile
                //db.CreateTourOperator(
                //    userId,
                //    companyNameTextBox.Text,
                //    descriptionTextBox.Text,
                //    addressTextBox.Text
                //);

                MessageBox.Show("Tour Operator registered successfully!",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //TourOperatorDashboard tourOp = new TourOperatorDashboard();
                //tourOp.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during registration: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void back_Click(object sender, EventArgs e)
        {
            Landing landing = new Landing();
            landing.Show();
            this.Hide();
        }
    }
}