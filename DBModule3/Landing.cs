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
    public partial class Landing : Form
    {
        public Landing()
        {
            InitializeComponent();
        }

        private void Landing_Load(object sender, EventArgs e)
        {

        }

        private void sataButton4_Click(object sender, EventArgs e)
        {

        }

        private void loginLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void travelerbutton_Click(object sender, EventArgs e)
        {
            TravelerSignUp travelerForm = new TravelerSignUp();
            travelerForm.Show();
            this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void tourop_Click(object sender, EventArgs e)
        {
            TourOperatorSignUp touropform = new TourOperatorSignUp();
            touropform.Show();
            this.Hide();
        }

        private void serviceprovider_Click(object sender, EventArgs e)
        {
            ServiceProviderSignUp serviceprovider = new ServiceProviderSignUp();
            serviceprovider.Show();
            this.Hide();    
        }

        private void loginLink_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login loginform = new Login();
            loginform.Show();
            this.Hide();
        }
    }
}
