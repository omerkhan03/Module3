using System;
using System.Windows.Forms;

namespace DBModule3
{
    public partial class ServiceProviderSignUp : Form
    {
        public ServiceProviderSignUp()
        {
            InitializeComponent();
            providerTypeComboBox.Items.AddRange(new string[] { "Hotel", "Transport Service", "Guide Service" });
        }

        private void title_Click(object sender, EventArgs e)
        {
        }


        private void next_Click(object sender, EventArgs e)
        {
            if (providerTypeComboBox.SelectedItem == null)
            {
                MessageBox.Show("Please select a provider type.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Form nextForm = null;
            switch (providerTypeComboBox.SelectedItem.ToString())
            {
                case "Hotel":
                   nextForm = new HotelRegistration();
                    break;
                case "Transport Service":
                    nextForm = new TransportRegistration();
                    break;
                case "Guide Service":
                   nextForm = new GuideRegistration();
                    break;
            }

            if (nextForm != null)
            {
                nextForm.Show();
                this.Hide();
            }
        }

        private void back_Click(object sender, EventArgs e)
        {
            Landing landing = new Landing();
            landing.Show();
            this.Hide();
        }

        private void back_Click_1(object sender, EventArgs e)
        {
            Landing landing = new Landing();
            landing.Show();
            this.Hide();
        }
    }
}