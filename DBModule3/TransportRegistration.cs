using System;
using System.Windows.Forms;

namespace DBModule3
{
    public partial class TransportRegistration : Form
    {
        public TransportRegistration()
        {
            InitializeComponent();
            transportTypeComboBox.Items.AddRange(new string[] { "Car", "Bus", "Van", "Other" });
        }

        private void title_Click(object sender, EventArgs e)
        {
        }
        private void registerbutton_Click(object sender, EventArgs e)
        {
            if (transportTypeComboBox.SelectedItem == null ||
        string.IsNullOrEmpty(vehicleDetailsTextBox.Text) ||
        !int.TryParse(capacityTextBox.Text, out int capacity))
            {
                MessageBox.Show("Please fill all fields correctly. Capacity must be a number.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                MessageBox.Show("Registration Successful");
                //TransportProviderDashboard transportDash = new TransportProviderDashboard();
                //transportDash.Show();
                this.Close();
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