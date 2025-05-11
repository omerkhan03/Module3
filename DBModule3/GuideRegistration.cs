using System;
using System.Windows.Forms;

namespace DBModule3
{
    public partial class GuideRegistration : Form
    {
        public GuideRegistration()
        {
            InitializeComponent();
            // Add common languages to the checklist
            languageChecklist.Items.AddRange(new string[] { 
                "English", "Spanish", "French", "German", 
                "Italian", "Chinese", "Japanese", "Arabic" 
            });
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
            if (string.IsNullOrEmpty(guideNameTextBox.Text) ||
             string.IsNullOrEmpty(specializationTextBox.Text) ||
            languageChecklist.CheckedItems.Count == 0)
            {
                MessageBox.Show("Please fill all fields and select at least one language.",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                MessageBox.Show("Registration Successful");
                //GuideDashboard guideDash = new GuideDashboard();
                //guideDash.Show();
                this.Close();
            }

        }
    }
}