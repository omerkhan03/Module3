using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        }

        private void registerbutton_Click(object sender, EventArgs e)
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
                // First create the user account
                //int userId = db.CreateUser(
                //    usernameTextBox.Text,
                //    passwordTextBox.Text,
                //    emailTextBox.Text,
                //    phoneTextBox.Text,
                //    "Traveler"
                //);

                //// Then create the traveler profile
                //db.CreateTraveler(
                //    userId,
                //    firstNameTextBox.Text,
                //    lastNameTextBox.Text,
                //    dateOfBirthPicker.Value,
                //    nationalityTextBox.Text,
                //    addressTextBox.Text
                //);

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
