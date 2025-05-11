using System;
using System.Windows.Forms;

namespace DBModule3
{
    public partial class HotelRegistration : Form
    {
        public HotelRegistration()
        {
            InitializeComponent();
        }

        private void title_Click(object sender, EventArgs e)
        {
        }
        private void title_Click_1(object sender, EventArgs e)
        {

        }

        private void back_Click(object sender, EventArgs e)
        {
            Landing landing = new Landing();
            landing.Show();
            this.Hide();
        }

        private void register_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(locationTextBox.Text) ||
            !int.TryParse(totalRoomsTextBox.Text, out int totalRooms))
            {
                MessageBox.Show("Please fill all fields correctly. Total rooms must be a number.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show("Hotel registered successfully!",
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //HotelDashboard hotelDash = new HotelDashboard();
            //hotelDash.Show();
            this.Close();
        }
    }
}